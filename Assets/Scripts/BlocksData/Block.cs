using UnityEngine;
using XLua;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

#pragma warning disable IDE0044 // 添加只读修饰符
#pragma warning disable CS0649

namespace Minecraft.BlocksData
{
    /// <summary>
    /// 保存方块的元数据，提供部分操作方块的接口
    /// </summary>
    [LuaCallCSharp]
    [CreateAssetMenu(menuName = "Minecraft/Block")]
    public sealed class Block : ScriptableObject
    {
        [SerializeField] [Tooltip("方块的名称")] private string m_BlockName;
        [SerializeField] [Tooltip("方块的类型")] private BlockType m_Type;
        [SerializeField] [Tooltip("方块的设置")] private BlockFlags m_Flags = BlockFlags.None;
        [SerializeField] [Tooltip("方块的顶点类型")] private BlockVertexType m_VertexType = BlockVertexType.Cube;
        [SerializeField] [Range(-5, 5)] [Tooltip("玩家在方块上移动的阻力")] private float m_MoveResistance = 1;
        [SerializeField] [Range(0, 15)] [Tooltip("方块阻挡的亮度值")] private byte m_LightOpacity = 15;
        [SerializeField] [Range(0, 15)] [Tooltip("方块的亮度值")] private byte m_LightValue = 0;
        [SerializeField] [Range(0, 60)] [Tooltip("方块的硬度")] private byte m_Hardness = 16;
        [SerializeField] [Tooltip("方块摧毁特效颜色")] private ParticleSystem.MinMaxGradient m_DestoryEffectColor;

        // uv groups
        [SerializeField] private Vector2 PositiveXLB; // main
        [SerializeField] private Vector2 PositiveXRB; // main
        [SerializeField] private Vector2 PositiveXRT; // main
        [SerializeField] private Vector2 PositiveXLT; // main

        [SerializeField] private Vector2 PositiveYLB;
        [SerializeField] private Vector2 PositiveYRB;
        [SerializeField] private Vector2 PositiveYRT;
        [SerializeField] private Vector2 PositiveYLT;

        [SerializeField] private Vector2 PositiveZLB;
        [SerializeField] private Vector2 PositiveZRB;
        [SerializeField] private Vector2 PositiveZRT;
        [SerializeField] private Vector2 PositiveZLT;

        [SerializeField] private Vector2 NegativeXLB;
        [SerializeField] private Vector2 NegativeXRB;
        [SerializeField] private Vector2 NegativeXRT;
        [SerializeField] private Vector2 NegativeXLT;

        [SerializeField] private Vector2 NegativeYLB;
        [SerializeField] private Vector2 NegativeYRB;
        [SerializeField] private Vector2 NegativeYRT;
        [SerializeField] private Vector2 NegativeYLT;

        [SerializeField] private Vector2 NegativeZLB;
        [SerializeField] private Vector2 NegativeZRB;
        [SerializeField] private Vector2 NegativeZRT;
        [SerializeField] private Vector2 NegativeZLT;


        [SerializeField] [Tooltip("方块被挖掘的音效")] private AudioClip m_DigAudio;
        [SerializeField] [Tooltip("方块被放置的音效")] private AudioClip m_PlaceAudio;
        [SerializeField] [Tooltip("方块被踩的音效")] private AudioClip[] m_StepAudios;


        [SerializeField] [Tooltip("方块的额外资源")] private Object[] m_ExtraAssets;


        private event BlockEventAction m_OnTick;
        private event BlockEventAction m_OnRandomTick;
        private event BlockEventAction m_OnBlockDestroy;
        private event BlockEventAction m_OnBlockPlace;
        private event BlockEventAction m_OnClick;


        public string BlockName => m_BlockName;

        public BlockType Type => m_Type;

        public BlockFlags Flags => m_Flags;

        public BlockVertexType VertexType => m_VertexType;

        public float MoveResistance => m_MoveResistance;

        public byte LightOpacity => m_LightOpacity;

        public byte LightValue => m_LightValue;

        public int Hardness => m_Hardness;

        public ParticleSystem.MinMaxGradient DestoryEffectColor => m_DestoryEffectColor;


        public event BlockEventAction OnTickEvent
        {
            add => m_OnTick += value;
            remove => m_OnTick -= value;
        }

        public event BlockEventAction OnRandomTickEvent
        {
            add
            {
                if ((m_Flags & BlockFlags.NeedsRandomTick) == BlockFlags.NeedsRandomTick)
                {
                    m_OnRandomTick += value;
                }
            }
            remove
            {
                if ((m_Flags & BlockFlags.NeedsRandomTick) == BlockFlags.NeedsRandomTick)
                {
                    m_OnRandomTick -= value;
                }
            }
        }

        public event BlockEventAction OnBlockDestroyEvent
        {
            add => m_OnBlockDestroy += value;
            remove => m_OnBlockDestroy -= value;
        }

        public event BlockEventAction OnBlockPlaceEvent
        {
            add => m_OnBlockPlace += value;
            remove => m_OnBlockPlace -= value;
        }

        public event BlockEventAction OnClickEvent
        {
            add => m_OnClick += value;
            remove => m_OnClick -= value;
        }


        public Object GetExtraAsset(int index)
        {
            return m_ExtraAssets[index];
        }

        public T GetExtraAsset<T>(int index) where T : Object
        {
            return m_ExtraAssets[index] as T;
        }


        public void OnTick(int x, int y, int z)
        {
            m_OnTick?.Invoke(x, y, z, this);
        }

        public void OnRandomTick(int x, int y, int z)
        {
            if ((m_Flags & BlockFlags.NeedsRandomTick) == BlockFlags.NeedsRandomTick)
            {
                m_OnRandomTick?.Invoke(x, y, z, this);
            }
        }

        public void OnBlockDestroy(int x, int y, int z)
        {
            m_OnBlockDestroy?.Invoke(x, y, z, this);
        }

        public void OnBlockPlace(int x, int y, int z)
        {
            m_OnBlockPlace?.Invoke(x, y, z, this);
        }

        public void OnClick(int x, int y, int z)
        {
            m_OnClick?.Invoke(x, y, z, this);
        }

        public void ClearEvents()
        {
            m_OnTick = null;
            m_OnRandomTick = null;
            m_OnBlockDestroy = null;
            m_OnBlockPlace = null;
            m_OnClick = null;
        }


        public bool HasAllFlags(BlockFlags flags)
        {
            return (m_Flags & flags) == flags;
        }

        public bool HasAnyFlag(BlockFlags flags)
        {
            return (m_Flags & flags) != BlockFlags.None;
        }


        public void GetMainUVForPerpendicularQuadsVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt)
        {
            lb = PositiveXLB;
            rb = PositiveXRB;
            rt = PositiveXRT;
            lt = PositiveXLT;
        }

        public void GetPositiveXUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt)
        {
            lb = PositiveXLB;
            rb = PositiveXRB;
            rt = PositiveXRT;
            lt = PositiveXLT;
        }

        public void GetPositiveYUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt)
        {
            lb = PositiveYLB;
            rb = PositiveYRB;
            rt = PositiveYRT;
            lt = PositiveYLT;
        }

        public void GetPositiveZUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt)
        {
            lb = PositiveZLB;
            rb = PositiveZRB;
            rt = PositiveZRT;
            lt = PositiveZLT;
        }

        public void GetNegativeXUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt)
        {
            lb = NegativeXLB;
            rb = NegativeXRB;
            rt = NegativeXRT;
            lt = NegativeXLT;
        }

        public void GetNegativeYUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt)
        {
            lb = NegativeYLB;
            rb = NegativeYRB;
            rt = NegativeYRT;
            lt = NegativeYLT;
        }

        public void GetNegativeZUVForCubeVertex(out Vector2 lb, out Vector2 rb, out Vector2 rt, out Vector2 lt)
        {
            lb = NegativeZLB;
            rb = NegativeZRB;
            rt = NegativeZRT;
            lt = NegativeZLT;
        }


        public void PlayDigAudio(AudioSource source)
        {
            if (m_DigAudio && source)
            {
                source.clip = m_DigAudio;
                source.Play();
            }
        }

        public void PlayPlaceAudio(AudioSource source)
        {
            if (m_PlaceAudio && source)
            {
                source.clip = m_PlaceAudio;
                source.Play();
            }
        }

        public void PlayStepAutio(AudioSource source)
        {
            if (m_StepAudios != null && m_StepAudios.Length > 0 && source && !source.isPlaying)
            {
                source.PlayOneShot(m_StepAudios[Random.Range(0, m_StepAudios.Length)]);
            }
        }        
    }
}