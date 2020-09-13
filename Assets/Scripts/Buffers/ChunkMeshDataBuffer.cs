using System;
using System.Threading;
using UnityEngine;
using static Minecraft.WorldConsts;

namespace Minecraft.Buffers
{
    public sealed class ChunkMeshDataBuffer : MeshDataBuffer, IDisposable
    {
        public int SectionIndex { get; private set; }

        private readonly ManualResetEventSlim m_ResetEvent;
        
        public ChunkMeshDataBuffer()
        {
            SectionIndex = 0;
            m_ResetEvent = new ManualResetEventSlim(false);
        }

        public void Dispose()
        {
            SectionIndex = -1;
            m_ResetEvent.Dispose();
        }

        /// <summary>
        /// 开始重写缓冲区内容
        /// </summary>
        /// <param name="sectionIndex">重写的区块部分（section）的索引</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="sectionIndex"/>超出范围</exception>
        public void BeginRewriting(int sectionIndex)
        {
            if (SectionIndex == -1)
            {
                throw new ObjectDisposedException(nameof(ChunkMeshDataBuffer));
            }

            if (sectionIndex < 0 || sectionIndex >= SectionCountInChunk)
            {
                throw new ArgumentOutOfRangeException(nameof(sectionIndex), sectionIndex.ToString(), "sectionIndex应该在[0,15]范围内");
            }

            SectionIndex = sectionIndex;
            m_ResetEvent.Reset();

            BeginRewriting();
        }

        /// <summary>
        /// 在主线程将数据提交至指定的mesh中，并等待提交完成
        /// </summary>
        /// <param name="meshes">mesh数组，此方法将设置<see cref="SectionIndex"/>索引处的mesh</param>
        /// <param name="context">同步上下文</param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"><paramref name="context"/>为null</exception>
        public void ApplyToMesh(Mesh[] meshes, SynchronizationContext context)
        {
            if (SectionIndex == -1)
            {
                throw new ObjectDisposedException(nameof(ChunkMeshDataBuffer));
            }

            if (VertexCount == 0 || TriangleCount == 0)
            {
                return;
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // 最好不用send方法，避免构造过多ManualResetEvent
            context.Post(obj =>
            {
                try
                {
                    ApplyToMesh(ref (obj as Mesh[])[SectionIndex]);
                }
                finally
                {
                    m_ResetEvent.Set();
                }

            }, meshes);

            m_ResetEvent.Wait();
        }
    }
}