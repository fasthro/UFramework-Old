/*
 * @Author: fasthro
 * @Date: 2019-11-28 17:52:03
 * @Description: 线程任务(线程任务同时只能执行一个任务)
 */
using System.Collections.Generic;
using System.Threading;
using FastEngine;
using UnityEngine;

namespace FastEngine.Core
{
    [MonoSingletonPath("FastEngine/ThreadTaskService")]
    public class ThreadTaskService : MonoSingleton<ThreadTaskService>
    {
        private Thread m_thread;
        private readonly object m_lockObject = new object();
        static Queue<IThreadTask> tasks = new Queue<IThreadTask>();

        public override void InitializeSingleton()
        {
            m_thread = new Thread(OnUpdate);
        }

        protected override void OnDestroy()
        {
            m_thread.Abort();
        }

        void Start()
        {
            m_thread.Start();
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(IThreadTask task)
        {
            lock (m_lockObject)
            {
                tasks.Enqueue(task);
            }
        }

        void OnUpdate()
        {
            while (true)
            {
                lock (m_lockObject)
                {
                    if (tasks.Count > 0)
                    {
                        IThreadTask task = tasks.Dequeue();
                        try
                        {
                            task.OnExecute();
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError("[ThreadTaskService] Exception: " + ex.Message);
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }
    }
}