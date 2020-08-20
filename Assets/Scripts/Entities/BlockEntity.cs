using Minecraft.BlocksData;
using System.Collections.Generic;
using UnityEngine;
using static Minecraft.BlocksData.BlockVertexHelper;
using static Minecraft.WorldConsts;

#pragma warning disable CS0649

namespace Minecraft
{
    [DisallowMultipleComponent]
    public class BlockEntity : Entity
    {
        private Transform m_Transform;

        private bool m_PreviouslyGrounded;
        private Vector3 m_MoveDir;
        private Vector3 m_Velocity;

        private Mesh m_Mesh;
        private List<VertexData> m_VertexBuffer;
        private List<int> m_TrianglesBuffer;

        private float m_TimeCounter;


        public sealed override AABB BoundingBox
        {
            get
            {
                Vector3 pos = m_Transform.position;
                return AABB.CreateBlockAABB(pos.x, pos.y, pos.z);
            }
        }

        public sealed override bool IsGrounded
        {
            get
            {
                AABB aabb = BoundingBox;
                WorldManager world = WorldManager.Active;

                int y = Mathf.FloorToInt(aabb.Min.y) - 1;

                if (y >= WorldHeight || y < 0)
                {
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
                        Block block = world.GetBlock(x, y, z);

                        if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                        {
                            AABB blockAABB = AABB.CreateBlockAABB(x, y, z);

                            if (aabb.Intersects(blockAABB))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }

        public sealed override Vector3 Velocity
        {
            get => m_Velocity;
            set => m_MoveDir = value;
        }

        public Vector3Int BlockPosition
        {
            get
            {
                Vector3 pos = m_Transform.position;

                int x = Mathf.FloorToInt(pos.x);
                int y = Mathf.FloorToInt(pos.y);
                int z = Mathf.FloorToInt(pos.z);

                return new Vector3Int(x, y, z);
            }
        }

        public bool Initialized { get; private set; }

        public float LifeTime { get; protected set; }

        public Block Block { get; protected set; }

        protected virtual MaterialPropertyBlock MaterialPropertyBlock => null;

        protected virtual bool ShouldRenderBlock => true;


        public void Initialize(int x, int y, int z, Block block, float lifeTime = 3, float gravityMultiplier = 3)
        {
            m_Transform = GetComponent<Transform>();

            m_PreviouslyGrounded = false;
            m_MoveDir = Vector3.zero;
            Initialized = false;

            m_Mesh = new Mesh();
            m_Mesh.MarkDynamic();
            m_VertexBuffer = new List<VertexData>();
            m_TrianglesBuffer = new List<int>();

            m_TimeCounter = 0;

            m_Transform.position = new Vector3(x, y, z);
            WorldManager world = WorldManager.Active;
            LifeTime = lifeTime;
            GravityMultiplier = gravityMultiplier;
            Block = block;

            switch (block.VertexType)
            {
                case BlockVertexType.Cube:
                    {
                        float light = block.LightValue * OverMaxLight;
                        float lb, rb, rt, lt;

                        lb = (world.GetFinalLightLevel(x + 1, y, z) + world.GetFinalLightLevel(x + 1, y - 1, z) + world.GetFinalLightLevel(x + 1, y, z - 1) + world.GetFinalLightLevel(x + 1, y - 1, z - 1)) * 0.25f * OverMaxLight;
                        rb = (world.GetFinalLightLevel(x + 1, y, z) + world.GetFinalLightLevel(x + 1, y - 1, z) + world.GetFinalLightLevel(x + 1, y, z + 1) + world.GetFinalLightLevel(x + 1, y - 1, z + 1)) * 0.25f * OverMaxLight;
                        rt = (world.GetFinalLightLevel(x + 1, y, z) + world.GetFinalLightLevel(x + 1, y + 1, z) + world.GetFinalLightLevel(x + 1, y, z + 1) + world.GetFinalLightLevel(x + 1, y + 1, z + 1)) * 0.25f * OverMaxLight;
                        lt = (world.GetFinalLightLevel(x + 1, y, z) + world.GetFinalLightLevel(x + 1, y + 1, z) + world.GetFinalLightLevel(x + 1, y, z - 1) + world.GetFinalLightLevel(x + 1, y + 1, z - 1)) * 0.25f * OverMaxLight;

                        AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                        AddCubeVertexDataPX(0, 0, 0, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);

                        lb = (world.GetFinalLightLevel(x - 1, y, z) + world.GetFinalLightLevel(x - 1, y - 1, z) + world.GetFinalLightLevel(x - 1, y, z + 1) + world.GetFinalLightLevel(x - 1, y - 1, z + 1)) * 0.25f * OverMaxLight;
                        rb = (world.GetFinalLightLevel(x - 1, y, z) + world.GetFinalLightLevel(x - 1, y - 1, z) + world.GetFinalLightLevel(x - 1, y, z - 1) + world.GetFinalLightLevel(x - 1, y - 1, z - 1)) * 0.25f * OverMaxLight;
                        rt = (world.GetFinalLightLevel(x - 1, y, z) + world.GetFinalLightLevel(x - 1, y + 1, z) + world.GetFinalLightLevel(x - 1, y, z - 1) + world.GetFinalLightLevel(x - 1, y + 1, z - 1)) * 0.25f * OverMaxLight;
                        lt = (world.GetFinalLightLevel(x - 1, y, z) + world.GetFinalLightLevel(x - 1, y + 1, z) + world.GetFinalLightLevel(x - 1, y, z + 1) + world.GetFinalLightLevel(x - 1, y + 1, z + 1)) * 0.25f * OverMaxLight;

                        AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                        AddCubeVertexDataNX(0, 0, 0, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);

                        lb = (world.GetFinalLightLevel(x, y + 1, z) + world.GetFinalLightLevel(x, y + 1, z - 1) + world.GetFinalLightLevel(x - 1, y + 1, z) + world.GetFinalLightLevel(x - 1, y + 1, z - 1)) * 0.25f * OverMaxLight;
                        rb = (world.GetFinalLightLevel(x, y + 1, z) + world.GetFinalLightLevel(x, y + 1, z - 1) + world.GetFinalLightLevel(x + 1, y + 1, z) + world.GetFinalLightLevel(x + 1, y + 1, z - 1)) * 0.25f * OverMaxLight;
                        rt = (world.GetFinalLightLevel(x, y + 1, z) + world.GetFinalLightLevel(x, y + 1, z + 1) + world.GetFinalLightLevel(x + 1, y + 1, z) + world.GetFinalLightLevel(x + 1, y + 1, z + 1)) * 0.25f * OverMaxLight;
                        lt = (world.GetFinalLightLevel(x, y + 1, z) + world.GetFinalLightLevel(x, y + 1, z + 1) + world.GetFinalLightLevel(x - 1, y + 1, z) + world.GetFinalLightLevel(x - 1, y + 1, z + 1)) * 0.25f * OverMaxLight;

                        AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                        AddCubeVertexDataPY(0, 0, 0, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);

                        lb = (world.GetFinalLightLevel(x, y - 1, z) + world.GetFinalLightLevel(x, y - 1, z - 1) + world.GetFinalLightLevel(x + 1, y - 1, z) + world.GetFinalLightLevel(x + 1, y - 1, z - 1)) * 0.25f * OverMaxLight;
                        rb = (world.GetFinalLightLevel(x, y - 1, z) + world.GetFinalLightLevel(x, y - 1, z - 1) + world.GetFinalLightLevel(x - 1, y - 1, z) + world.GetFinalLightLevel(x - 1, y - 1, z - 1)) * 0.25f * OverMaxLight;
                        rt = (world.GetFinalLightLevel(x, y - 1, z) + world.GetFinalLightLevel(x, y - 1, z + 1) + world.GetFinalLightLevel(x - 1, y - 1, z) + world.GetFinalLightLevel(x - 1, y - 1, z + 1)) * 0.25f * OverMaxLight;
                        lt = (world.GetFinalLightLevel(x, y - 1, z) + world.GetFinalLightLevel(x, y - 1, z + 1) + world.GetFinalLightLevel(x + 1, y - 1, z) + world.GetFinalLightLevel(x + 1, y - 1, z + 1)) * 0.25f * OverMaxLight;

                        AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                        AddCubeVertexDataNY(0, 0, 0, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);

                        lb = (world.GetFinalLightLevel(x, y, z + 1) + world.GetFinalLightLevel(x, y - 1, z + 1) + world.GetFinalLightLevel(x + 1, y, z + 1) + world.GetFinalLightLevel(x + 1, y - 1, z + 1)) * 0.25f * OverMaxLight;
                        rb = (world.GetFinalLightLevel(x, y, z + 1) + world.GetFinalLightLevel(x, y - 1, z + 1) + world.GetFinalLightLevel(x - 1, y, z + 1) + world.GetFinalLightLevel(x - 1, y - 1, z + 1)) * 0.25f * OverMaxLight;
                        rt = (world.GetFinalLightLevel(x, y, z + 1) + world.GetFinalLightLevel(x, y + 1, z + 1) + world.GetFinalLightLevel(x - 1, y, z + 1) + world.GetFinalLightLevel(x - 1, y + 1, z + 1)) * 0.25f * OverMaxLight;
                        lt = (world.GetFinalLightLevel(x, y, z + 1) + world.GetFinalLightLevel(x, y + 1, z + 1) + world.GetFinalLightLevel(x + 1, y, z + 1) + world.GetFinalLightLevel(x + 1, y + 1, z + 1)) * 0.25f * OverMaxLight;

                        AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                        AddCubeVertexDataPZ(0, 0, 0, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);

                        lb = (world.GetFinalLightLevel(x, y, z - 1) + world.GetFinalLightLevel(x, y - 1, z - 1) + world.GetFinalLightLevel(x - 1, y, z - 1) + world.GetFinalLightLevel(x - 1, y - 1, z - 1)) * 0.25f * OverMaxLight;
                        rb = (world.GetFinalLightLevel(x, y, z - 1) + world.GetFinalLightLevel(x, y - 1, z - 1) + world.GetFinalLightLevel(x + 1, y, z - 1) + world.GetFinalLightLevel(x + 1, y - 1, z - 1)) * 0.25f * OverMaxLight;
                        rt = (world.GetFinalLightLevel(x, y, z - 1) + world.GetFinalLightLevel(x, y + 1, z - 1) + world.GetFinalLightLevel(x + 1, y, z - 1) + world.GetFinalLightLevel(x + 1, y + 1, z - 1)) * 0.25f * OverMaxLight;
                        lt = (world.GetFinalLightLevel(x, y, z - 1) + world.GetFinalLightLevel(x, y + 1, z - 1) + world.GetFinalLightLevel(x - 1, y, z - 1) + world.GetFinalLightLevel(x - 1, y + 1, z - 1)) * 0.25f * OverMaxLight;

                        AddCubeVertexTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                        AddCubeVertexDataNZ(0, 0, 0, Mathf.Max(lb, light), Mathf.Max(rb, light), Mathf.Max(rt, light), Mathf.Max(lt, light), block, m_VertexBuffer);
                    }
                    break;
                case BlockVertexType.PerpendicularQuads:
                    {
                        float light = world.GetFinalLightLevel(x, y, z) * OverMaxLight;

                        AddPerpendicularQuadsTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                        AddPerpendicularQuadsVertexDataFirstQuad(0, 0, 0, light, block, m_VertexBuffer);

                        AddPerpendicularQuadsTriangles(m_TrianglesBuffer, m_VertexBuffer.Count);
                        AddPerpendicularQuadsVertexDataSecondQuad(0, 0, 0, light, block, m_VertexBuffer);
                    }
                    break;
            }

            if (m_VertexBuffer.Count > 0 && m_TrianglesBuffer.Count > 0)
            {
                m_Mesh.SetVertexBufferParams(m_VertexBuffer.Count, VertexLayout);
                m_Mesh.SetVertexBufferData(m_VertexBuffer, 0, 0, m_VertexBuffer.Count);
                m_Mesh.SetTriangles(m_TrianglesBuffer, 0);
                m_Mesh.UploadMeshData(false);
            }

            OnInitialize();

            Initialized = true;
        }


        private void Update()
        {
            m_TimeCounter += Time.deltaTime;
            WorldManager world = WorldManager.Active;

            if (m_TimeCounter > LifeTime)
            {
                world.EntityManager.DestroyEntity(this);
            }
            else if (m_Mesh.vertexCount > 0 && ShouldRenderBlock)
            {
                Graphics.DrawMesh(m_Mesh, m_Transform.position, Quaternion.identity, world.EntityManager.BlockEntityMaterial, BlockLayer, world.MainCamera, 0, MaterialPropertyBlock, false, false, false);
            }
        }

        private void FixedUpdate()
        {
            if (!Initialized)
                return;

            bool isGrounded = IsGrounded;

            if (!m_PreviouslyGrounded && isGrounded)
            {
                OnGrounded();

                m_MoveDir.y = 0;
            }
            else if (!isGrounded && m_PreviouslyGrounded)
            {
                OnLeaveGround();
            }

            m_PreviouslyGrounded = isGrounded;

            if (!isGrounded)
            {
                m_MoveDir += Physics.gravity * GravityMultiplier * Time.fixedDeltaTime;
            }

            Move(m_MoveDir * Time.fixedDeltaTime, Time.fixedDeltaTime, isGrounded);
        }


        protected virtual void OnInitialize() { }

        protected virtual void OnGrounded() { }

        protected virtual void OnLeaveGround() { }

        protected virtual void OnCollisions(CollisionFlags flags) { }


        private void Move(Vector3 translation, float deltaTime, bool isGrounded)
        {
            AABB aabb = BoundingBox;
            CollisionFlags flags = CheckCollisions(aabb, ref translation.x, ref translation.y, ref translation.z);

            if (isGrounded)
            {
                flags |= CollisionFlags.Below;
            }

            m_Transform.position = aabb.Min + translation;
            Velocity = translation / deltaTime;

            if (flags != CollisionFlags.None)
            {
                OnCollisions(flags);
            }
        }

        private CollisionFlags CheckCollisions(AABB aabb, ref float moveX, ref float moveY, ref float moveZ)
        {
            WorldManager world = WorldManager.Active;
            CollisionFlags flags = CollisionFlags.None;

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
                            Block block = world.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = AABB.CreateBlockAABB(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Above;
                                    moveY = 0;
                                }
                                else
                                {
                                    moveY = Mathf.Min(moveY, blockAABB.Min.y - aabb.Max.y);
                                }
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
                            Block block = world.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = AABB.CreateBlockAABB(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Below;
                                    moveY = 0;
                                }
                                else
                                {
                                    moveY = Mathf.Max(moveY, blockAABB.Max.y - aabb.Min.y);
                                }
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
                            Block block = world.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = AABB.CreateBlockAABB(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Sides;
                                    moveX = 0;
                                }
                                else
                                {
                                    moveX = Mathf.Min(moveX, blockAABB.Min.x - aabb.Max.x);
                                }
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
                            Block block = world.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = AABB.CreateBlockAABB(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Sides;
                                    moveX = 0;
                                }
                                else
                                {
                                    moveX = Mathf.Max(moveX, blockAABB.Max.x - aabb.Min.x);
                                }
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
                            Block block = world.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = AABB.CreateBlockAABB(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Sides;
                                    moveZ = 0;
                                }
                                else
                                {
                                    moveZ = Mathf.Min(moveZ, blockAABB.Min.z - aabb.Max.z);
                                }
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
                            Block block = world.GetBlock(x, y, z);

                            if (!block.HasAllFlags(BlockFlags.IgnoreCollisions))
                            {
                                AABB blockAABB = AABB.CreateBlockAABB(x, y, z);

                                if (aabb.Intersects(blockAABB))
                                {
                                    flags |= CollisionFlags.Sides;
                                    moveZ = 0;
                                }
                                else
                                {
                                    moveZ = Mathf.Max(moveZ, blockAABB.Max.z - aabb.Min.z);
                                }
                                break;
                            }
                        }
                    }
                }
            }

            return flags;
        }
    }
}