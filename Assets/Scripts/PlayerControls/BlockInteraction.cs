using System;
using Minecraft.Configurations;
using Minecraft.Entities;
using Minecraft.Lua;
using Minecraft.PhysicSystem;
using Minecraft.Rendering;
using UnityEngine;
using UnityEngine.UI;
using Physics = Minecraft.PhysicSystem.Physics;

namespace Minecraft.PlayerControls
{
    [DisallowMultipleComponent]
    public class BlockInteraction : MonoBehaviour, ILuaCallCSharp
    {
        [Range(3, 12)] public float RaycastMaxDistance = 8;
        [Min(0.1f)] public float MaxClickSpacing = 0.4f;

        [SerializeField] private Text m_CurrentHandBlockText;
        [SerializeField] private InputField m_HandBlockInput;
        [SerializeField] private MonoBehaviour[] m_DisableWhenEditHandBlock;

        [NonSerialized] private Camera m_Camera;
        [NonSerialized] private IAABBEntity m_PlayerEntity;
        [NonSerialized] private Func<BlockData, bool> m_DestroyRaycastSelector;
        [NonSerialized] private Func<BlockData, bool> m_PlaceRaycastSelector;

        [NonSerialized] private bool m_IsDigging;
        [NonSerialized] private float m_DiggingDamage;
        [NonSerialized] private Vector3Int m_FirstDigPos;
        [NonSerialized] private Vector3Int m_ClickedPos;
        [NonSerialized] private float m_ClickTime;

        [NonSerialized] private GameObject m_HandBlockInputGO;

        public void Initialize(Camera camera, IAABBEntity playerEntity)
        {
            m_Camera = camera;
            m_PlayerEntity = playerEntity;
            m_DestroyRaycastSelector = DestroyRaycastSelect;
            m_PlaceRaycastSelector = PlaceRaycastSelect;
        }

        private void OnEnable()
        {
            m_IsDigging = false;
            m_DiggingDamage = 0;
            m_FirstDigPos = Vector3Int.down;
            m_ClickedPos = Vector3Int.down;
            m_ClickTime = 0;
            SetDigProgress(0);

            m_HandBlockInputGO = m_HandBlockInput.gameObject;
        }

        private void OnDisable()
        {
            SetDigProgress(0);
            ShaderUtility.TargetedBlockPosition = Vector3.down;
        }

        private void Update()
        {
            if (ChangeHandBlock())
            {
                return;
            }

            if (!m_Camera)
            {
                return;
            }

            Ray ray = GetRay();
            IWorld world = m_PlayerEntity.World;
            DigBlock(ray, world);
            PlaceBlock(ray, world);
        }

        private bool ChangeHandBlock()
        {
            if (!Input.GetKeyDown(KeyCode.Return))
            {
                return m_HandBlockInputGO.activeInHierarchy;
            }

            if (m_HandBlockInputGO.activeInHierarchy)
            {
                // 不用 onSubmit 了

                m_CurrentHandBlockText.text = m_HandBlockInput.text;
                m_HandBlockInput.DeactivateInputField();
                m_HandBlockInputGO.SetActive(false);

                for (int i = 0; i < m_DisableWhenEditHandBlock.Length; i++)
                {
                    m_DisableWhenEditHandBlock[i].enabled = true;
                }

                return false;
            }
            else
            {
                for (int i = 0; i < m_DisableWhenEditHandBlock.Length; i++)
                {
                    m_DisableWhenEditHandBlock[i].enabled = false;
                }

                m_HandBlockInputGO.SetActive(true);
                m_HandBlockInput.text = string.Empty;
                m_HandBlockInput.ActivateInputField();

                return true;
            }
        }

        private void DigBlock(Ray ray, IWorld world)
        {
            if (Physics.RaycastBlock(ray, RaycastMaxDistance, world, m_DestroyRaycastSelector, out BlockRaycastHit hit))
            {
                ShaderUtility.TargetedBlockPosition = hit.Position;

                if (Input.GetMouseButton(0))
                {
                    if (m_IsDigging)
                    {
                        if (hit.Position == m_FirstDigPos)
                        {
                            m_DiggingDamage += Time.deltaTime * 5;
                            SetDigProgress(m_DiggingDamage / hit.Block.Hardness);

                            if (m_DiggingDamage >= hit.Block.Hardness)
                            {
                                SetDigProgress(0);
                                m_IsDigging = false;
                                world.RWAccessor.SetBlock(hit.Position.x, hit.Position.y, hit.Position.z, world.BlockDataTable.GetBlock(0), Quaternion.identity, ModificationSource.PlayerAction);

                                //block.PlayDigAudio(m_AudioSource);

                                // if (Setting.SettingManager.Active.RenderingSetting.EnableDestroyEffect)
                                // {
                                //     ParticleSystem effect = Instantiate(m_DestroyEffectPrefab, firstHitPos + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity).GetComponent<ParticleSystem>();
                                //     ParticleSystem.MainModule main = effect.main;
                                //     main.startColor = block.DestoryEffectColor;
                                // }
                            }
                        }
                        else
                        {
                            SetDigProgress(0);
                            m_IsDigging = false;
                        }
                    }
                    else
                    {
                        BlockData block = world.RWAccessor.GetBlock(hit.Position.x, hit.Position.y, hit.Position.z);
                        m_IsDigging = true;
                        m_DiggingDamage = 0;
                        m_FirstDigPos = hit.Position;
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    m_ClickedPos = hit.Position;
                    m_ClickTime = Time.time;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    SetDigProgress(0);
                    m_IsDigging = false;

                    BlockData block = world.RWAccessor.GetBlock(hit.Position.x, hit.Position.y, hit.Position.z);

                    if ((hit.Position == m_ClickedPos) && (Time.time - m_ClickTime <= MaxClickSpacing))
                    {
                        block.Click(world, hit.Position.x, hit.Position.y, hit.Position.z);
                    }

                    m_ClickedPos = Vector3Int.down;
                    m_ClickTime = 0;
                }
            }
            else
            {
                // 无选定方块
                ShaderUtility.TargetedBlockPosition = Vector3.down;
            }
        }

        private void PlaceBlock(Ray ray, IWorld world)
        {
            if (Input.GetMouseButtonDown(1))
            {
                BlockData block = world.BlockDataTable.GetBlock(m_CurrentHandBlockText.text);

                if (Physics.RaycastBlock(ray, RaycastMaxDistance, world, m_PlaceRaycastSelector, out BlockRaycastHit hit))
                {
                    Vector3Int pos = (hit.Position + hit.Normal).FloorToInt();
                    AABB playerBB = m_PlayerEntity.BoundingBox + m_PlayerEntity.Position;
                    AABB blockBB = hit.Block.GetBoundingBox(pos, world, false).Value;

                    if (!playerBB.Intersects(blockBB))
                    {
                        Quaternion rotation = Quaternion.identity;

                        // !!! 一定要按下面的顺序相乘，才能保证先绕 y 再绕 xz

                        if ((block.RotationAxes & BlockRotationAxes.AroundXOrZAxis) == BlockRotationAxes.AroundXOrZAxis)
                        {
                            rotation *= Quaternion.FromToRotation(Vector3.up, hit.Normal); // 底部朝向法线
                        }

                        if ((block.RotationAxes & BlockRotationAxes.AroundYAxis) == BlockRotationAxes.AroundYAxis)
                        {
                            Vector3 forward = m_PlayerEntity.Forward;
                            forward = Mathf.Abs(forward.x) > Mathf.Abs(forward.z) ? new Vector3(forward.x, 0, 0) : new Vector3(0, 0, forward.z);
                            rotation *= Quaternion.LookRotation(-forward.normalized, Vector3.up); // 看向玩家
                        }

                        world.RWAccessor.SetBlock(pos.x, pos.y, pos.z, block, rotation, ModificationSource.PlayerAction);
                    }
                }
            }
        }

        private bool DestroyRaycastSelect(BlockData block)
        {
            return !block.HasFlag(BlockFlags.IgnoreDestroyBlockRaycast) && block.PhysicState == PhysicState.Solid;
        }

        private bool PlaceRaycastSelect(BlockData block)
        {
            return !block.HasFlag(BlockFlags.IgnorePlaceBlockRaycast) && block.PhysicState == PhysicState.Solid;
        }

        private Ray GetRay()
        {
            return m_Camera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        }

        private void SetDigProgress(float progress)
        {
            ShaderUtility.DigProgress = (int)(progress * m_PlayerEntity.World.RenderingManager.DigProgressTextureCount) - 1;
        }
    }
}
