/*
 * @Author: fasthro
 * @Date: 2019-10-24 16:55:36
 * @Description: 协同工厂
 */

using System.Collections;

namespace FastEngine.Core
{
    public static class CoroutineFactory
    {
        /// <summary>
        /// 创建协同
        /// </summary>
        /// <param name="coroutine"></param>
        /// <param name="completeCallback"></param>
        /// <returns></returns>
        public static Coroutine Create(IEnumerator coroutine, CoroutineEventCallback completeCallback = null)
        {
            return new Coroutine(coroutine, false, completeCallback);
        }

        /// <summary>
        /// 创建协同并且直接开始
        /// </summary>
        /// <param name="coroutine"></param>
        /// <param name="completeCallback"></param>
        /// <returns></returns>
        public static Coroutine CreateAndStart(IEnumerator coroutine, CoroutineEventCallback completeCallback = null)
        {
            return new Coroutine(coroutine, true, completeCallback); ;
        }
    }
}