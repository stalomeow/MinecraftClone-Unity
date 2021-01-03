using Minecraft.Rendering;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using XLua;
using Object = UnityEngine.Object;
using Random = System.Random;

#pragma warning disable IDE0044 // 添加只读修饰符
#pragma warning disable CS0649

namespace Minecraft
{
    /// <summary>
    /// 保存方块的元数据，提供部分操作方块的接口
    /// </summary>
    [LuaCallCSharp]
    [CreateAssetMenu(menuName = "Minecraft/Block")]
    public class Block : ScriptableObject
    {
        /// <summary>
        /// 空气方块Id
        /// </summary>
        public const byte AirId = 0;


        [SerializeField] private byte m_Id = 1;
        [SerializeField] private string m_NameKey;
        [SerializeField] private BlockFlags m_Flags = BlockFlags.None;
        [SerializeField] [Range(-5, 5)] private float m_MoveResistance = 1;
        [SerializeField] [Range(0, 15)] private byte m_LightOpacity = 15;
        [SerializeField] [Range(0, 15)] private byte m_LightValue = 0;
        [SerializeField] [Range(0, 60)] private byte m_Hardness = 16;
        [SerializeField] private BlockLogics m_Logics;
        [SerializeField] private BlockMeshWriter m_MeshWriter;
        [SerializeField] private ParticleSystem.MinMaxGradient m_DestoryEffectColor;

        [SerializeField] private Texture2D[] m_Textures;

        // audio groups
        [SerializeField] private AudioClip m_DigAudio;
        [SerializeField] private AudioClip m_PlaceAudio;
        [SerializeField] private AudioClip[] m_StepAudios;

        [SerializeField] private Object[] m_ExtraAssets;


        public byte Id => m_Id;

        public string NameKey => m_NameKey;

        public float MoveResistance => m_MoveResistance;

        public byte LightOpacity => m_LightOpacity;

        public byte LightValue => m_LightValue;

        public int Hardness => m_Hardness;

        public ParticleSystem.MinMaxGradient DestoryEffectColor => m_DestoryEffectColor;

        public AudioClip DigAudio => m_DigAudio;

        public AudioClip PlaceAudio => m_PlaceAudio;

        public BlockLogics Logics => m_Logics;

        public BlockMeshWriter MeshWriter => m_MeshWriter;

        public IEnumerable<Texture2D> Textures => m_Textures;

        public IEnumerable<Object> ExtraAssets => m_ExtraAssets;


        public bool IsAir
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Id == AirId;
        }

        public bool IsFluid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => HasAnyFlag(BlockFlags.Fluid);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAllFlags(BlockFlags flags)
        {
            return (m_Flags & flags) == flags;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasAnyFlag(BlockFlags flags)
        {
            return (m_Flags & flags) != BlockFlags.None;
        }

        public Texture2D GetTexture(int index)
        {
            return m_Textures[index];
        }

        public AudioClip GetRandomStepAudio(Random random)
        {
            if (m_StepAudios.Length == 0)
                return null;

            int index = random.Next(0, m_StepAudios.Length);
            return m_StepAudios[index];
        }

        public Object GetExtraAsset(int index)
        {
            return m_ExtraAssets[index];
        }

        public T GetExtraAsset<T>(int index) where T : Object
        {
            return m_ExtraAssets[index] as T;
        }
    }
}