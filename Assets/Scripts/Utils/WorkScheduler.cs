using System;
using System.Collections.Generic;
using System.Threading;
using Minecraft.Collections;
using UnityEngine;

namespace Minecraft.Utils
{
    public abstract class WorkScheduler<TMainThreadWork, TAsyncWork> : MonoBehaviour
    {
        private List<TMainThreadWork> m_MainThreadWorks;
        private PriorityQueue<TAsyncWork> m_AsyncWorks;
        private SpinLock m_AsyncWorksLock;
        private Thread m_WorkerThread;
        private bool m_IsAlive;


        private void Start()
        {
            m_MainThreadWorks = new List<TMainThreadWork>();
            m_AsyncWorks = new PriorityQueue<TAsyncWork>(CompareAsyncWork);
            m_AsyncWorksLock = new SpinLock(false);
            m_WorkerThread = new Thread(DoAsyncWorks) { IsBackground = true };
            m_IsAlive = true;

            OnInitialize();
        }

        protected void StartWorkerThread()
        {
            m_WorkerThread.Start();
        }

        private void OnDestroy()
        {
            m_IsAlive = false;
            m_WorkerThread.Join();
            OnDispose();
        }

        protected void AddWork(in TMainThreadWork work)
        {
            m_MainThreadWorks.Add(work);
        }

        protected void AddWork(in TAsyncWork work)
        {
            bool lockTaken = false;

            try
            {
                m_AsyncWorksLock.Enter(ref lockTaken);
                m_AsyncWorks.Enqueue(work);
            }
            finally
            {
                if (lockTaken)
                {
                    m_AsyncWorksLock.Exit(false);
                }
            }
        }

        public virtual void ClearWorks(Action<TAsyncWork> callback)
        {
            m_MainThreadWorks.Clear();

            bool lockTaken = false;

            try
            {
                m_AsyncWorksLock.Enter(ref lockTaken);

                while (m_AsyncWorks.Count > 0)
                {
                    callback?.Invoke(m_AsyncWorks.Dequeue());
                }
            }
            finally
            {
                if (lockTaken)
                {
                    m_AsyncWorksLock.Exit(false);
                }
            }
        }

        private void LateUpdate()
        {
            if (m_MainThreadWorks.Count == 0)
            {
                return;
            }

            DoMainThreadWorks(m_MainThreadWorks);
        }

        private bool GetNextAsyncWork(out TAsyncWork work)
        {
            bool lockTaken = false;

            try
            {
                m_AsyncWorksLock.Enter(ref lockTaken);

                if (m_AsyncWorks.Count > 0)
                {
                    work = m_AsyncWorks.Dequeue();
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                if (lockTaken)
                {
                    m_AsyncWorksLock.Exit(false);
                }
            }

            work = default;
            return false;
        }

        private void DoAsyncWorks()
        {
            SpinWait wait = new SpinWait();

            while (m_IsAlive)
            {
                if (GetNextAsyncWork(out TAsyncWork work))
                {
                    wait.Reset();
                    DoAsyncWork(in work);
                }
                else
                {
                    wait.SpinOnce();
                }
            }
        }


        protected virtual void OnInitialize() { }

        protected virtual void OnDispose() { }

        protected abstract void DoMainThreadWorks(List<TMainThreadWork> works);

        protected abstract void DoAsyncWork(in TAsyncWork work);

        protected abstract int CompareAsyncWork(TAsyncWork x, TAsyncWork y);
    }
}
