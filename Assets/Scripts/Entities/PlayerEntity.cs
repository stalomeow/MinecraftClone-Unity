using System;
using Minecraft.XPhysics;
using Minecraft.Rendering;
using System.Collections;
using UnityEngine;
using static Minecraft.WorldConsts;

#pragma warning disable CS0649

namespace Minecraft.Entities
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public sealed class PlayerEntity : Entity
    {
        private enum Direction
        {
            PositiveX,
            PositiveY,
            PositiveZ,
            NegativeX,
            NegativeY,
            NegativeZ
        }

        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0, 1)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private float m_StepInterval;

        [Space]

        [SerializeField] [Range(0, 1)] private float m_SpeedMultiplierWhenMovingInWater = 0.5f;
        [SerializeField] [Range(-2, 2)] private float m_GravityMultiplierWhenJumpingInWater = -1.5f;
        [SerializeField] [Range(0, 2)] private float m_GravityMultiplierWhenStayingInWater = 0.05f;
        [SerializeField] [Range(3, 15)] private int m_ViewDistanceUnderWater = 6;
        [SerializeField] private Color m_WaterAmbientColor = new Color(0f, 0.3490196f, 1f, 1f);

        [Space]

        [SerializeField] private MouseLook m_MouseLook;

        [Space]

        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob;
        [SerializeField] private LerpControlledBob m_JumpBob;

        [Space]

        [SerializeField] [Range(3, 12)] private float m_BlockRaycastDistance = 8;
        [SerializeField] private GameObject m_DestroyEffectPrefab;
        [SerializeField] private GameObject m_DestroyBlockPrefab;


        private Camera m_Camera;
        private Transform m_CameraTransform;
        private Transform m_Transform;
        private AudioSource m_AudioSource;
        private Transform m_DestroyBlockObj;
        private MeshRenderer m_DestroyBlockObjRenderer;
        private MaterialPropertyBlock m_DestroyBlockObjProperty;

        private World m_World;

        private Vector3 m_OriginalCameraPosition;
        private bool m_IsWalking;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private Vector3 m_AddedVelocity = Vector3.zero;
        private bool m_PreviouslyGrounded;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;

        private bool m_IsInWater;
        private Vector3 m_Velocity = Vector3.zero;
        private byte m_LastBlockAtHead = Block.AirId;
        private byte m_LastBlockAtFeet = Block.AirId;
        private bool m_IsDigging;
        private Vector3Int m_ClickedPos = Vector3Int.down;
        private float m_ClickTime = 0;


        public override AABB BoundingBox
        {
            get
            {
                Vector3 pos = m_Transform.localPosition;

                Vector3 min = new Vector3(pos.x - 0.35f, pos.y - 1, pos.z - 0.35f);
                Vector3 max = new Vector3(pos.x + 0.35f, pos.y + 1, pos.z + 0.35f);

                return new AABB(min, max);
            }
        }

        public override bool IsGrounded => CheckIsGrounded(out _);

        public override Vector3 Velocity
        {
            get => m_Velocity;
            set => m_AddedVelocity = value;
        }


        public MouseLook MouseLook => m_MouseLook;

        public AudioSource AudioSource => m_AudioSource;


        private void Start()
        {
            m_Camera = GetComponentInChildren<Camera>();
            m_CameraTransform = m_Camera.GetComponent<Transform>();
            m_Transform = GetComponent<Transform>();
            m_AudioSource = GetComponent<AudioSource>();

            m_DestroyBlockObj = Instantiate(m_DestroyBlockPrefab, Vector3.zero, Quaternion.identity).transform;
            m_DestroyBlockObjRenderer = m_DestroyBlockObj.GetComponent<MeshRenderer>();
            m_DestroyBlockObjProperty = new MaterialPropertyBlock();

            m_MouseLook.Init(m_Transform, m_CameraTransform);
            m_HeadBob.Setup(m_CameraTransform, m_StepInterval);

            m_World = World.Active;

            m_OriginalCameraPosition = m_CameraTransform.localPosition;
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle * 0.5f;
            m_Jumping = false;
            m_IsInWater = false;
            m_IsDigging = false;
        }

        private void Update()
        {
            m_MouseLook.LookRotation(m_Transform, m_CameraTransform, Time.deltaTime);
            m_MouseLook.UpdateCursorLock();

            InteractWithBlocks();

            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = m_IsInWater ? Input.GetKey(KeyCode.Space) : Input.GetKeyDown(KeyCode.Space);
            }

            bool isGrounded = CheckIsGrounded(out Block blockUnderFeet);

            if (!m_PreviouslyGrounded && isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayBlockStepSound(blockUnderFeet);

                m_NextStep = m_StepCycle + 0.5f;
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }

            if (!isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = isGrounded;
        }

        private void FixedUpdate()
        {
            if (m_AddedVelocity != Vector3.zero)
            {
                m_MoveDir = m_AddedVelocity;
                m_AddedVelocity = Vector3.zero;
                Move(m_MoveDir * Time.fixedDeltaTime, Time.fixedDeltaTime);
            }
            else
            {
                float speed = GetInput();

                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = m_Transform.forward * m_Input.y + m_Transform.right * m_Input.x;

                m_MoveDir.x = desiredMove.x * speed;
                m_MoveDir.z = desiredMove.z * speed;

                CheckIsInWater();
                bool isGrounded = CheckIsGrounded(out Block blockUnderFeet);

                if (m_IsInWater)
                {
                    if (m_MoveDir.x != 0)
                        m_MoveDir.x *= m_SpeedMultiplierWhenMovingInWater;

                    if (m_MoveDir.z != 0)
                        m_MoveDir.z *= m_SpeedMultiplierWhenMovingInWater;

                    if (m_Jump)
                    {
                        m_MoveDir += UnityEngine.Physics.gravity * m_GravityMultiplierWhenJumpingInWater * Time.fixedDeltaTime;
                        m_Jump = false;
                    }
                    else if (!isGrounded)
                    {
                        m_MoveDir += UnityEngine.Physics.gravity * m_GravityMultiplierWhenStayingInWater * Time.fixedDeltaTime;
                    }
                }
                else if (isGrounded)
                {
                    if (m_Jump)
                    {
                        m_MoveDir.y = m_JumpSpeed;

                        PlayBlockStepSound(blockUnderFeet);

                        m_Jump = false;
                        m_Jumping = true;
                    }
                }
                else
                {
                    m_MoveDir += UnityEngine.Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                }

                Move(m_MoveDir * Time.fixedDeltaTime, Time.fixedDeltaTime);

                ProgressStepCycle(speed, isGrounded, blockUnderFeet);
                UpdateCameraPosition(speed, isGrounded);
            }            
        }


        private void InteractWithBlocks()
        {
            if (!m_IsDigging && Input.GetMouseButton(0))
            {
                Ray ray = m_Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
                if (XPhysics.Physics.RaycastBlock(ray, 8, m_World, b => (!b.HasAnyFlag(BlockFlags.IgnoreDestroyBlockRaycast) && !b.IsFluid), out BlockRaycastHit hit))
                {
                    Block block = m_World.GetBlock(hit.Position.x, hit.Position.y, hit.Position.z);
                    StartCoroutine(DigBlock(block, hit.Position, false));

                    if (Input.GetMouseButtonDown(0))
                    {
                        m_ClickedPos = hit.Position;
                        m_ClickTime = Time.time;
                    }
                }
            }
            //else if (Input.GetMouseButtonDown(1))
            //{
            //    //Item item = WorldManager.Active.GetCurrentItem();

            //    //if (item.MappedBlockType != BlockType.Air)
            //    {
            //        if (RaycastBlock(false, out Vector3Int hit, out Vector3Int normal, b => b.HasAnyFlag(BlockFlags.IgnorePlaceBlockRaycast)))
            //        {
            //            Vector3Int pos = hit + normal;

            //            Vector3 min = new Vector3(pos.x + 0.01f, pos.y + 0.01f, pos.z + 0.01f);
            //            Vector3 max = new Vector3(pos.x - 0.01f + 1, pos.y - 0.01f + 1, pos.z - 0.01f + 1);
            //            AABB blockAABB = new AABB(min, max);

            //            if (!BoundingBox.Intersects(blockAABB))
            //            {
            //                m_World.SetBlockType(pos.x, pos.y, pos.z, item.MappedBlockType);

            //                Block block = m_World.AssetManager.GetBlockByType(item.MappedBlockType);
            //                block.OnBlockPlace(pos.x, pos.y, pos.z);
            //                block.PlayPlaceAudio(m_AudioSource);
            //            }
            //        }
            //    }
            //}

            //if (Input.GetMouseButtonUp(0))
            //{
            //    if (RaycastBlock(false, out Vector3Int hit, out _, b => b.HasAnyFlag(BlockFlags.IgnoreDestroyBlockRaycast)))
            //    {
            //        Block block = m_World.GetBlock(hit.x, hit.y, hit.z);

            //        if ((hit == m_ClickedPos) && (Time.time - m_ClickTime < 1))
            //        {
            //            block.OnClick(hit.x, hit.y, hit.z);
            //        }

            //        m_ClickedPos = Vector3Int.down;
            //        m_ClickTime = 0;
            //    }
            //}
        }

        private IEnumerator DigBlock(Block block, Vector3Int firstHitPos, bool raycastLiquid)
        {
            m_IsDigging = true;
            m_DestroyBlockObj.position = firstHitPos + new Vector3(0.5f, 0.5f, 0.5f);

            int hardness = block.Hardness;
            float damage = 0;

            yield return null;

            while (damage < hardness)
            {
                if (Input.GetMouseButton(0))
                {
                    Ray ray = m_Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

                    if (XPhysics.Physics.RaycastBlock(ray, 8, m_World, b => (!b.HasAnyFlag(BlockFlags.IgnoreDestroyBlockRaycast) && !b.IsFluid), out BlockRaycastHit hit))
                    {
                        if (hit.Position == firstHitPos)
                        {
                            damage += Time.deltaTime * 5;

                            SetDigProgress(block.MeshWriter is CubeMeshWriter ? (damage / hardness) : 0);

                            yield return null;
                            continue;
                        }
                    }
                }

                SetDigProgress(0);
                m_IsDigging = false;
                yield break;
            }

            m_World.SetBlock(firstHitPos.x, firstHitPos.y, firstHitPos.z, Block.AirId);
            //block.Logics.OnBlockDestroy(firstHitPos.x, firstHitPos.y, firstHitPos.z);
            //block.PlayDigAudio(m_AudioSource);

            if (GlobalSettings.Instance.EnableDestroyEffect)
            {
                ParticleSystem effect = Instantiate(m_DestroyEffectPrefab, firstHitPos + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity).GetComponent<ParticleSystem>();
                ParticleSystem.MainModule main = effect.main;
                main.startColor = block.DestoryEffectColor;
            }

            SetDigProgress(0);
            m_IsDigging = false;
        }

        private void SetDigProgress(float progress)
        {
            m_DestroyBlockObjProperty.SetFloat("_DestroyProgress", progress);
            m_DestroyBlockObjRenderer.SetPropertyBlock(m_DestroyBlockObjProperty);
        }

        private bool CheckIsGrounded(out Block blockUnderFeet)
        {
            AABB aabb = BoundingBox;

            int y = Mathf.FloorToInt(aabb.Min.y) - 1;

            if (y >= WorldHeight || y < 0)
            {
                blockUnderFeet = null;
                return false;
            }

            int startX = Mathf.FloorToInt(aabb.Min.x);
            int endX = Mathf.FloorToInt(aabb.Max.x - 0.01f);
            int startZ = Mathf.FloorToInt(aabb.Min.z);
            int endZ = Mathf.FloorToInt(aabb.Max.z - 0.01f);

            for (int x = startX; x <= endX; x++)
            {
                for (int z = startZ; z <= endZ; z++)
                {
                    blockUnderFeet = m_World.GetBlock(x, y, z);

                    if (blockUnderFeet == null)
                    {
                        continue;
                    }

                    if (!blockUnderFeet.HasAllFlags(BlockFlags.IgnoreCollisions))
                    {
                        AABB blockAABB = blockUnderFeet.GetBoundingBox(x, y, z);

                        if (aabb.Intersects(blockAABB))
                        {
                            return true;
                        }
                    }
                }
            }

            blockUnderFeet = null;
            return false;
        }

        private void CheckIsInWater()
        {
            //AABB aabb = BoundingBox;
            //Vector3 center = aabb.Center;

            //int x = Mathf.FloorToInt(center.x);
            //int z = Mathf.FloorToInt(center.z);
            //int y1 = Mathf.FloorToInt(aabb.Min.y);

            //bool result = false;

            //if (y1 > -1 || y1 < WorldHeight)
            //{
            //    BlockType type = world.GetBlockType(x, y1, z);
            //    result |= (type == BlockType.Water);

            //    if (m_LastBlockAtFeet != BlockType.Water && type == BlockType.Water)
            //    {
            //        OnEnterWater();
            //    }
            //    else if (m_LastBlockAtFeet == BlockType.Water && type != BlockType.Water)
            //    {
            //        OnExitWater();
            //    }

            //    m_LastBlockAtFeet = type;
            //}

            //int y2 = Mathf.FloorToInt(center.y + 0.5f);

            //if (y2 > -1 || y2 < WorldHeight)
            //{
            //    BlockType type = world.GetBlockType(x, y2, z);

            //    result |= (type == BlockType.Water);

            //    if (m_LastBlockAtHead != BlockType.Water && type == BlockType.Water)
            //    {
            //        OnHeadEnterWater();
            //    }
            //    else if (m_LastBlockAtHead == BlockType.Water && type != BlockType.Water)
            //    {
            //        OnHeadExitWater();
            //    }

            //    m_LastBlockAtHead = type;
            //}

            //m_IsInWater = result;
        }

        private void OnHeadEnterWater()
        {
            m_UseHeadBob = false;
            //m_World.ChunkManager.MaterialProperties.RenderRadius = m_ViewDistanceUnderWater;
            //m_World.ChunkManager.MaterialProperties.AmbientColor = m_WaterAmbientColor;
        }

        private void OnHeadExitWater()
        {
            m_UseHeadBob = true;
            //m_World.ChunkManager.MaterialProperties.RenderRadius = GlobalSettings.Instance.RenderRadius;
            //m_World.ChunkManager.MaterialProperties.AmbientColor = GlobalSettings.Instance.DefaultAmbientColor;
        }

        private void OnEnterWater()
        {
            if (m_AudioSource.isPlaying)
                return;

            //m_World.AssetManager.GetBlockByType(BlockType.Water).PlayPlaceAudio(m_AudioSource);
        }

        private void OnExitWater()
        {
            if (m_AudioSource.isPlaying)
                return;

            //m_World.AssetManager.GetBlockByType(BlockType.Water).PlayDigAudio(m_AudioSource);
        }


        private void Move(Vector3 translation, float deltaTime)
        {
            AABB aabb = BoundingBox;

            CheckCollisions(aabb, ref translation.x, ref translation.y, ref translation.z);

            m_Transform.localPosition += translation;
            m_Velocity = translation / deltaTime;
        }

        private void CheckCollisions(AABB aabb, ref float moveX, ref float moveY, ref float moveZ)
        {
            if (moveY > 0)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Max.x - 0.01f);
                int startY = Mathf.FloorToInt(aabb.Max.y);
                int endY = Mathf.FloorToInt(aabb.Max.y + moveY);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z - 0.01f);

                for (int x = startX; x <= endX; x++)
                {
                    for (int z = startZ; z <= endZ; z++)
                    {
                        for (int y = startY; y <= endY; y++)
                        {
                            Block block = m_World.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);
                                moveY = Mathf.Min(moveY, aabb.Intersects(blockAABB) ? 0 : (blockAABB.Min.y - aabb.Max.y));
                                break;
                            }
                        }
                    }
                }
            }
            else if (moveY < 0)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Max.x - 0.01f);
                int startY = Mathf.FloorToInt(aabb.Min.y);
                int endY = Mathf.FloorToInt(aabb.Min.y + moveY);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z - 0.01f);

                for (int x = startX; x <= endX; x++)
                {
                    for (int z = startZ; z <= endZ; z++)
                    {
                        for (int y = startY; y >= endY; y--)
                        {
                            Block block = m_World.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);
                                moveY = Mathf.Max(moveY, aabb.Intersects(blockAABB) ? 0 : (blockAABB.Max.y - aabb.Min.y));
                                break;
                            }
                        }
                    }
                }
            }

            aabb += new Vector3(0, moveY, 0);

            if (moveX > 0)
            {
                int startX = Mathf.FloorToInt(aabb.Max.x);
                int endX = Mathf.FloorToInt(aabb.Max.x + moveX);
                int startY = Mathf.FloorToInt(aabb.Min.y);
                int endY = Mathf.FloorToInt(aabb.Max.y - 0.01f);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z - 0.01f);

                for (int z = startZ; z <= endZ; z++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        for (int x = startX; x <= endX; x++)
                        {
                            Block block = m_World.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);
                                moveX = Mathf.Min(moveX, aabb.Intersects(blockAABB) ? 0 : (blockAABB.Min.x - aabb.Max.x));
                                break;
                            }
                        }
                    }
                }
            }
            else if (moveX < 0)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Min.x + moveX);
                int startY = Mathf.FloorToInt(aabb.Min.y);
                int endY = Mathf.FloorToInt(aabb.Max.y - 0.01f);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z - 0.01f);

                for (int z = startZ; z <= endZ; z++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        for (int x = startX; x >= endX; x--)
                        {
                            Block block = m_World.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);
                                moveX = Mathf.Max(moveX, aabb.Intersects(blockAABB) ? 0 : (blockAABB.Max.x - aabb.Min.x));
                                break;
                            }
                        }
                    }
                }
            }

            aabb += new Vector3(moveX, 0, 0);

            if (moveZ > 0)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Max.x - 0.01f);
                int startY = Mathf.FloorToInt(aabb.Min.y);
                int endY = Mathf.FloorToInt(aabb.Max.y - 0.01f);
                int startZ = Mathf.FloorToInt(aabb.Max.z);
                int endZ = Mathf.FloorToInt(aabb.Max.z + moveZ);

                for (int x = startX; x <= endX; x++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        for (int z = startZ; z <= endZ; z++)
                        {
                            Block block = m_World.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);
                                moveZ = Mathf.Min(moveZ, aabb.Intersects(blockAABB) ? 0 : (blockAABB.Min.z - aabb.Max.z));
                                break;
                            }
                        }
                    }
                }
            }
            else if (moveZ < 0)
            {
                int startX = Mathf.FloorToInt(aabb.Min.x);
                int endX = Mathf.FloorToInt(aabb.Max.x - 0.01f);
                int startY = Mathf.FloorToInt(aabb.Min.y);
                int endY = Mathf.FloorToInt(aabb.Max.y - 0.01f);
                int startZ = Mathf.FloorToInt(aabb.Min.z);
                int endZ = Mathf.FloorToInt(aabb.Min.z + moveZ);

                for (int x = startX; x <= endX; x++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        for (int z = startZ; z >= endZ; z--)
                        {
                            Block block = m_World.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = block.GetBoundingBox(x, y, z);
                                moveZ = Mathf.Max(moveZ, aabb.Intersects(blockAABB) ? 0 : (blockAABB.Max.z - aabb.Min.z));
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void ProgressStepCycle(float speed, bool isGrounded, Block blockUnderFeet)
        {
            if (m_Velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_Velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) * Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            if (isGrounded)
            {
                PlayBlockStepSound(blockUnderFeet);
            }
        }

        private void UpdateCameraPosition(float speed, bool isGrounded)
        {
            Vector3 newCameraPosition;

            if (!m_UseHeadBob)
            {
                return;
            }

            if (m_Velocity.sqrMagnitude > 0 && isGrounded)
            {
                m_CameraTransform.localPosition = m_HeadBob.DoHeadBob(m_Velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_CameraTransform.localPosition;
                newCameraPosition.y = m_CameraTransform.localPosition.y - m_JumpBob.Offset;
            }
            else
            {
                newCameraPosition = m_CameraTransform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset;
            }

            m_CameraTransform.localPosition = newCameraPosition;
        }

        private float GetInput()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);

            float speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            return speed;
        }

        private void PlayBlockStepSound(Block block)
        {
            //block.PlayStepAutio(m_AudioSource);
        }
    }
}