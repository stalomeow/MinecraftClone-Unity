using System;
using System.Collections.Generic;
using Minecraft.Configurations;
using Minecraft.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using XLua;

namespace Minecraft.Entities
{
    [DisallowMultipleComponent]
    public class LuaBlockEntity : Entity, IRenderableEntity
    {
        [NonSerialized] private Mesh m_Mesh;
        [NonSerialized] private MaterialPropertyBlock m_MaterialProperty;
        [NonSerialized] private List<SectionMeshVertexData> m_VertexBuffer;
        [NonSerialized] private List<ushort> m_IndexBuffer;

        [NonSerialized] private bool m_EnableRendering;
        [NonSerialized] private Material m_Material;
        [NonSerialized] private BlockData m_Block;
        [NonSerialized] private LuaTable m_ContextTable;


        public bool EnableRendering
        {
            get => m_EnableRendering;
            set => m_EnableRendering = value;
        }

        public Mesh SharedMesh => m_Mesh;

        public Material SharedMaterial => m_Material;

        public MaterialPropertyBlock MaterialProperty => m_MaterialProperty;


        protected override void OnInitialize()
        {
            base.OnInitialize();

            m_Mesh = new Mesh();
            m_Mesh.MarkDynamic();
            m_MaterialProperty = new MaterialPropertyBlock();
            m_VertexBuffer = new List<SectionMeshVertexData>();
            m_IndexBuffer = new List<ushort>();

            m_EnableRendering = true;
            m_Material = null;
            m_Block = null;

            OnCollisions += OnCollisionsCallback;
        }

        public void SetBlockAndPosition(BlockData block, Vector3Int position)
        {
            InitializeEntityIfNot();

            m_Block = block;
            m_Material = World.BlockDataTable.GetMaterial(block.Material.Value);
            m_Transform.position = position;

            BuildMesh(position);

            m_ContextTable = World.LuaManager.CreateTable();
            m_Block.EntityInit(World, this, m_ContextTable);
        }

        public override void OnRecycle()
        {
            m_Block.EntityDestroy(World, this, m_ContextTable);

            base.OnRecycle();

            m_Mesh.Clear(false);
            m_MaterialProperty.Clear();
            m_VertexBuffer.Clear();
            m_IndexBuffer.Clear();

            m_EnableRendering = true;
            m_Material = null;
            m_Block = null;
            m_ContextTable.Dispose();
            m_ContextTable = null;
        }

        private void BuildMesh(Vector3Int position)
        {
            BlockMesh mesh = World.BlockDataTable.GetMesh(m_Block.Mesh.Value);

            for (int i = 0; i < mesh.Faces.Length; i++)
            {
                BlockMesh.FaceData face = mesh.Faces[i];
                int?[] texIndices = m_Block.Textures[i];

                // !!! must add indices first
                for (int j = 0; j < face.Indices.Length; j++)
                {
                    m_IndexBuffer.Add((ushort)(m_VertexBuffer.Count + face.Indices[j]));
                }

                for (int j = 0; j < face.Vertices.Length; j++)
                {
                    BlockVertexData vertex = face.Vertices[j];
                    float emission = m_Block.GetEmissionValue();
                    Vector2 ambient = LightingUtility.AmbientOcclusion(position.x, position.y, position.z, face.Face, vertex.CornerInFace, World.RWAccessor);

                    m_VertexBuffer.Add(new SectionMeshVertexData
                    {
                        PositionOS = vertex.Position,
                        UV = vertex.UV,
                        TexIndices = new Vector3Int(texIndices[0].Value, texIndices[1].Value, texIndices[2].Value),
                        Lights = new Vector3(emission, ambient.x, ambient.y),
                        BlockPositionWS = new Vector3(-1, -1, -1)
                    });
                }
            }

            m_Mesh.SetVertexBufferParams(m_VertexBuffer.Count, SectionMeshVertexData.VertexAttributes);
            m_Mesh.SetVertexBufferData(m_VertexBuffer, 0, 0, m_VertexBuffer.Count);
            m_Mesh.SetIndexBufferParams(m_IndexBuffer.Count, IndexFormat.UInt16);
            m_Mesh.SetIndexBufferData(m_IndexBuffer, 0, 0, m_IndexBuffer.Count);
            m_Mesh.SetSubMesh(0, new SubMeshDescriptor(0, m_IndexBuffer.Count));
            m_Mesh.UploadMeshData(false);
            m_Mesh.RecalculateNormals();
            m_Mesh.RecalculateTangents();
            m_Mesh.RecalculateBounds();
        }

        private void Update()
        {
            m_Block.EntityUpdate(World, this, m_ContextTable);
        }

        protected override void FixedUpdate()
        {
            m_Block.EntityFixedUpdate(World, this, m_ContextTable);
            base.FixedUpdate();

            if (m_Transform.position.y < -20)
            {
                // 掉出世界了
                World.EntityManager.DestroyEntity(this);
            }
        }

        private void OnCollisionsCallback(CollisionFlags flags)
        {
            m_Block.EntityOnCollisions(World, this, flags, m_ContextTable);
        }

        public void Render(int layer, Camera camera, ShadowCastingMode castShadows, bool receiveShadows)
        {
            Matrix4x4 matrix = Matrix4x4.TRS(m_Transform.position, Quaternion.identity, Vector3.one);
            Graphics.DrawMesh(m_Mesh, matrix, m_Material, layer, camera, 0, m_MaterialProperty, castShadows, receiveShadows, null, false);
        }
    }
}
