using Minecraft.DebugUtils;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Minecraft.AssetManagement
{
    public sealed class AsyncHandler : IEnumerator, IDebugMessageSender
    {
        private readonly AsyncOperation[] m_Operations;
        private readonly int m_MainOperationIndex;
        private Action<AsyncOperation> m_OnCompleted;
        private int m_CompletedCount;

        public bool DisableLog { get; set; }

        public float Progress
        {
            get
            {
                if (m_Operations == null || m_Operations.Length == 0)
                    return 1;

                float progress = 0;

                for (int i = 0; i < m_Operations.Length; i++)
                {
                    progress += m_Operations[i].progress;
                }

                return progress / m_Operations.Length;
            }
        }

        public float ProgressFast => 
            (m_Operations == null || m_Operations.Length == 0) ? 1 : (float)m_CompletedCount / m_Operations.Length;

        public bool IsDone => m_Operations == null ? true : m_CompletedCount >= m_Operations.Length;


        public event Action<AsyncOperation> OnCompleted
        {
            add
            {
                if (value != null)
                {
                    if (IsDone)
                    {
                        AsyncOperation op = m_Operations[m_MainOperationIndex];
                        value(op);
                    }
                    else
                    {
                        m_OnCompleted += value;
                    }
                }
            }

            remove
            {
                if (value != null)
                {
                    m_OnCompleted -= value;
                }
            }
        }

        public AsyncHandler(AsyncOperation[] ops, int mainOperationIndex)
        {
            m_Operations = ops;
            m_MainOperationIndex = mainOperationIndex;
            m_OnCompleted = null;
            m_CompletedCount = 0;

            if (m_Operations != null && m_Operations.Length > 0)
            {
                Init();
            }
        }

        private void Init()
        {
            for (int i = 0; i < m_Operations.Length; i++)
            {
                AsyncOperation op = m_Operations[i];

                if (op.isDone)
                {
                    OnCompletedCallback(op);
                }
                else
                {
                    m_Operations[i].completed += OnCompletedCallback;
                }
            }
        }

        private void OnCompletedCallback(AsyncOperation operation)
        {
            int count = Interlocked.Increment(ref m_CompletedCount);

            this.Log("completed callback");

            if (count >= m_Operations.Length)
            {
                AsyncOperation op = m_Operations[m_MainOperationIndex];
                m_OnCompleted?.Invoke(op);

                this.Log("加载完成");
            }
        }


        public static AsyncHandler CreateCompleted()
        {
            return new AsyncHandler(null, 0);
        }


        object IEnumerator.Current => null;

        string IDebugMessageSender.DisplayName => $"AsyncHandler({m_CompletedCount}/{(m_Operations == null ? 0 : m_Operations.Length)})";

        bool IEnumerator.MoveNext() => m_Operations != null && m_CompletedCount < m_Operations.Length;

        void IEnumerator.Reset() { }
    }
}