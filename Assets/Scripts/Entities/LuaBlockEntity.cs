using System;
using System.Collections.Generic;
using Minecraft.Configurations;
using Minecraft.Rendering;
using Unity.Collections;
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
        [NonSerialized] private BlockMeshBuilder<ushort> m_MeshBuilder;

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
            m_MeshBuilder = BlockMeshBuilder<ushort>.CreateBlockEntityMeshBuilder(true);

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
            // m_MeshBuilder.ClearBuffers();

            m_EnableRendering = true;
            m_Material = null;
            m_Block = null;
            m_ContextTable.Dispose();
            m_ContextTable = null;
        }

        private void BuildMesh(Vector3Int position)
        {
            // 渲染在原点的位置
            m_MeshBuilder.AddBlock(position, -position, m_Block, World.RWAccessor);
            m_MeshBuilder.ApplyAndClearBuffers(m_Mesh, MeshTopology.Triangles, false, Allocator.Temp);

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
