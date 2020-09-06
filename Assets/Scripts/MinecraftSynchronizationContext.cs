using Minecraft.DebugUtils;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

namespace Minecraft
{
    internal sealed class MinecraftSynchronizationContext : SynchronizationContext, IDebugMessageSender
    {
        private readonly struct WorkRequest
        {
            private readonly SendOrPostCallback m_Callback;
            private readonly object m_State;
            private readonly ManualResetEventSlim m_WaitHandle;

            public WorkRequest(SendOrPostCallback callback, object state, ManualResetEventSlim waitHandle)
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
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }

                m_WaitHandle?.Set();
            }
        }


        private const int InitialCapacity = 20;

        private readonly Queue<WorkRequest> m_AsyncWorkQueue;
        private readonly List<WorkRequest> m_CurrentFrameWork;
        private readonly int m_MainThreadID;

#if UNITY_EDITOR
        private readonly System.Diagnostics.Stopwatch m_Stopwatch = new System.Diagnostics.Stopwatch();
#endif


        string IDebugMessageSender.DisplayName => "MCSyncContext";

        public bool DisableLog { get; set; }


        private MinecraftSynchronizationContext(int mainThreadID) : this(new Queue<WorkRequest>(InitialCapacity), mainThreadID) { }

        private MinecraftSynchronizationContext(Queue<WorkRequest> asyncWorkQueue, int mainThreadID)
        {
            m_AsyncWorkQueue = asyncWorkQueue;
            m_CurrentFrameWork = new List<WorkRequest>(InitialCapacity);
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
                using (ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false))
                {
                    lock (m_AsyncWorkQueue)
                    {
                        WorkRequest work = new WorkRequest(callback, state, manualResetEvent);
                        m_AsyncWorkQueue.Enqueue(work);
                    }

                    manualResetEvent.Wait();
                }
            }
        }

        public override void Post(SendOrPostCallback callback, object state)
        {
            lock (m_AsyncWorkQueue)
            {
                WorkRequest work = new WorkRequest(callback, state, null);
                m_AsyncWorkQueue.Enqueue(work);
            }
        }

        public override SynchronizationContext CreateCopy()
        {
            return new MinecraftSynchronizationContext(m_AsyncWorkQueue, m_MainThreadID);
        }

        private void Execute()
        {
#if UNITY_EDITOR
            m_Stopwatch.Restart();
#endif

            int count = GlobalSettings.Instance.MaxTaskCountPerFrame;

            lock (m_AsyncWorkQueue)
            {
                while (count-- > 0)
                {
                    if (m_AsyncWorkQueue.Count == 0)
                    {
                        break;
                    }

                    WorkRequest work = m_AsyncWorkQueue.Dequeue();
                    m_CurrentFrameWork.Add(work);
                }
            }

            foreach (WorkRequest current in m_CurrentFrameWork)
            {
                current.Invoke();
            }

#if UNITY_EDITOR
            m_Stopwatch.Stop();

            this.Log("执行", m_CurrentFrameWork.Count.ToString(), "个任务，共计", m_Stopwatch.ElapsedMilliseconds.ToString(),"毫秒");
#endif

            m_CurrentFrameWork.Clear();            
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
            try
            {
                Profiler.BeginSample("Execute Tasks");

                if (Current is MinecraftSynchronizationContext unitySynchronizationContext)
                {
                    unitySynchronizationContext.Execute();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                Profiler.EndSample();
            }
        }
    }
}
