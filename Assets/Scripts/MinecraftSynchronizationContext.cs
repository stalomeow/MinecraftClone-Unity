using Minecraft.DebugUtils;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine.Profiling;

namespace Minecraft
{
    internal sealed class MinecraftSynchronizationContext : SynchronizationContext, IDebugMessageSender
    {
        private readonly struct WorkRequest
        {
            private readonly SendOrPostCallback m_Callback;
            private readonly object m_State;
            private readonly ManualResetEvent m_WaitHandle;

            public WorkRequest(SendOrPostCallback callback, object state, ManualResetEvent waitHandle)
            {
                m_Callback = callback;
                m_State = state;
                m_WaitHandle = waitHandle;
            }

            public void Invoke()
            {
                try
                {
                    m_Callback(m_State);
                }
                finally
                {
                    m_WaitHandle?.Set();
                }
            }
        }


        private readonly ConcurrentQueue<WorkRequest> m_AsyncWorkQueue;
        private readonly int m_MainThreadID;

#if UNITY_EDITOR
        private readonly System.Diagnostics.Stopwatch m_Stopwatch = new System.Diagnostics.Stopwatch();
#endif


        string IDebugMessageSender.DisplayName => "MCSyncContext";

        public bool DisableLog { get; set; }


        private MinecraftSynchronizationContext(int mainThreadID) : this(new ConcurrentQueue<WorkRequest>(), mainThreadID) { }

        private MinecraftSynchronizationContext(ConcurrentQueue<WorkRequest> asyncWorkQueue, int mainThreadID)
        {
            m_AsyncWorkQueue = asyncWorkQueue;
            m_MainThreadID = mainThreadID;
        }

        public override void Send(SendOrPostCallback callback, object state)
        {
            if (m_MainThreadID == Thread.CurrentThread.ManagedThreadId)
            {
                callback(state);
            }
            else
            {
                using (ManualResetEvent manualResetEvent = new ManualResetEvent(false))
                {
                    WorkRequest work = new WorkRequest(callback, state, manualResetEvent);
                    m_AsyncWorkQueue.Enqueue(work);

                    manualResetEvent.WaitOne();
                }
            }
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            WorkRequest work = new WorkRequest(callback, state, null);
            m_AsyncWorkQueue.Enqueue(work);
        }

        public override SynchronizationContext CreateCopy()
        {
            return new MinecraftSynchronizationContext(m_AsyncWorkQueue, m_MainThreadID);
        }

        private void Execute()
        {
#if UNITY_EDITOR
            m_Stopwatch.Restart();
            int executedCount = 0;
#endif

            int count = GlobalSettings.Instance.MaxTaskCountPerFrame;

            while (count-- > 0 && m_AsyncWorkQueue.TryDequeue(out WorkRequest work))
            {
                work.Invoke();

#if UNITY_EDITOR
                executedCount++;
#endif
            }
            
#if UNITY_EDITOR
            m_Stopwatch.Stop();

            if (executedCount > 0)
            {
                this.Log("执行", executedCount.ToString(), "个任务，共计", m_Stopwatch.ElapsedMilliseconds.ToString(), "毫秒");
            }
#endif   
        }

        public static void InitializeSynchronizationContext()
        {
            if (!(Current is MinecraftSynchronizationContext))
            {
                MinecraftSynchronizationContext context = new MinecraftSynchronizationContext(Thread.CurrentThread.ManagedThreadId);
                SetSynchronizationContext(context);
            }
        }

        public static void ExecuteTasks()
        {
            Profiler.BeginSample("MinecraftSynchronizationContext.ExecuteTasks");

            if (Current is MinecraftSynchronizationContext context)
            {
                context.Execute();
            }

            Profiler.EndSample();
        }
    }
}
