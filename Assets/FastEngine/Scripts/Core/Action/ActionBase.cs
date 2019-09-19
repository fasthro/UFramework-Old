/*
 * @Author: fasthro
 * @Date: 2019-09-18 15:09:28
 * @Description: Action Node
 */

using System;
using System.Collections.Generic;
using FastEngine.Common;
using UnityEngine;

namespace FastEngine.Core
{
    public class ActionBase : IAction
    {
        // 完成标志
        public bool isCompleted { get; protected set; }

        // 销毁标志
        public bool isDispose { get; protected set; }

        // callback dic
        private Dictionary<ACTION_CALLBACK_TYPE, Delegate> m_callbackDic;

        public ActionBase() { }

        /// <summary>
        /// 绑定 Callback
        /// </summary>
        /// <param name="act">callback type</param>
        /// <param name="callback">callback</param>
        public void BindCallback(ACTION_CALLBACK_TYPE act, ActionCallback callback)
        {
            if (m_callbackDic == null)
                m_callbackDic = new Dictionary<ACTION_CALLBACK_TYPE, Delegate>();

            if (!m_callbackDic.ContainsKey(act))
                m_callbackDic.Add(act, null);

            m_callbackDic[act] = (ActionCallback)m_callbackDic[act] + callback;
        }

        /// <summary>
        /// 广播 Callback
        /// </summary>
        /// <param name="act">callback type</param>
        protected void BroadcastCallback(ACTION_CALLBACK_TYPE act)
        {
            if (m_callbackDic.ContainsKey(act))
                ((ActionCallback)m_callbackDic[act]).InvokeGracefully();
        }

        /// <summary>
        /// 执行 Action
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public bool Execute(float deltaTime)
        {
            if (!isCompleted) OnExecute(deltaTime);
            if (isCompleted) BroadcastCallback(ACTION_CALLBACK_TYPE.COMPLETED);
            return isCompleted;
        }

        /// <summary>
        /// 子类执行触发
        /// </summary>
        /// <param name="deltaTime"></param>
        protected virtual void OnExecute(float deltaTime) { }

        /// <summary>
        /// 重置 Action
        /// </summary>
        public void Reset()
        {
            isCompleted = false;
            OnReset();
        }

        /// <summary>
        /// 子类重置触发
        /// </summary>
        /// <param name="deltaTime"></param>
        protected virtual void OnReset() { }

        /// <summary>
        /// 销毁 Action
        /// </summary>
        public void Dispose()
        {
            if (isDispose) return;
            isDispose = true;

            BroadcastCallback(ACTION_CALLBACK_TYPE.DISPOSED);

            foreach (KeyValuePair<ACTION_CALLBACK_TYPE, Delegate> item in m_callbackDic)
                m_callbackDic[item.Key] = null;
            m_callbackDic.Clear();

            OnDispose();
        }

        /// <summary>
        /// 子类销毁触发
        /// </summary>
        protected virtual void OnDispose() { }

        /// <summary>
        /// 启动 Action
        /// </summary>
        public void Start()
        {
            ActionSystem.Execute(this);
        }

        /// <summary>
        /// 停止 Action
        /// </summary>
        public void Stop()
        {
            ActionSystem.Remove(this);
        }
    }
}
