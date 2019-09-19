/*
 * @Author: fasthro
 * @Date: 2019-09-18 15:09:28
 * @Description: Action 接口
 */

namespace FastEngine.Core
{
    public interface IAction
    {
        /// <summary>
        /// 重置 Action
        /// </summary>
        void Reset();

        /// <summary>
        /// 执行 Action
        /// </summary>
        bool Execute(float deltaTime);

        /// <summary>
        /// 销毁 Action
        /// </summary>
        void Dispose();
    }
}
