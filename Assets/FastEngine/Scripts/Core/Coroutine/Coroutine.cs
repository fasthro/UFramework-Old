/*
 * @Author: fasthro
 * @Date: 2019-10-24 17:08:12
 * @Description: 协同包装 (支持停止，暂停，取消暂停)，扩展了Unity自带的协同程序
 * 注释:停止之后不能再做其他操作
 */
using System.Collections;

namespace FastEngine.Core
{
    /// <summary>
    /// 协同事件
    /// </summary>
    /// <param name="manual"></param>
    public delegate void CoroutineEventCallback(bool manual);

    public class Coroutine
    {
        public bool running { get; private set; }
        public bool paused { get; private set; }
        public bool stopped { get; private set; }

        // 完成回调
        private CoroutineEventCallback m_completeCallback;

        private IEnumerator m_coroutine;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coroutine"></param>
        /// <param name="autoStart"></param>
        /// <param name="completeCallback"></param>
        public Coroutine(IEnumerator coroutine, bool autoStart, CoroutineEventCallback completeCallback = null)
        {
            this.m_coroutine = coroutine;
            this.m_completeCallback = completeCallback;
            if (autoStart) this.Start();
        }

        public void Start()
        {
            running = true;
            App.Instance.StartCoroutine(CallWrapper());
        }

        public void Pause() { paused = true; }

        public void Unpause() { paused = false; }

        public void Stop()
        {
            stopped = true;
            running = false;
        }

        IEnumerator CallWrapper()
        {
            yield return null;
            IEnumerator e = this.m_coroutine;
            while (running)
            {
                if (paused)
                    yield return null;
                else
                {
                    if (e != null && e.MoveNext()) yield return e.Current;
                    else running = false;
                }
            }
            m_completeCallback.InvokeGracefully(stopped);
        }
    }
}