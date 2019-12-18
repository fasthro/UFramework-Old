/*
 * @Author: fasthro
 * @Date: 2019-12-10 11:46:03
 * @Description: 简单的线程任务
 */
using System;
using FastEngine;

namespace FastEngine.Core
{
    public class SimpleTreadTask : IThreadTask
    {
        private Action m_taskAction;
        public SimpleTreadTask(Action task) { m_taskAction = task; }
        public void OnExecute() { m_taskAction.InvokeGracefully(); }
    }
}