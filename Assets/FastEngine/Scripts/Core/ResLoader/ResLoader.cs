/*
 * @Author: fasthro
 * @Date: 2019-06-26 17:52:39
 * @Description: 加载基类
 */
namespace FastEngine.Core
{
    public abstract class ResLoader
    {
        // 通知
        protected ResNotificationListener m_listener;

        /// <summary>
        /// 同步加载
        /// </summary>
        public abstract bool LoadSync();

        /// <summary>
        /// 异步加载
        /// </summary>
        public abstract void LoadAsync();

        /// <summary>
        /// 卸载资源
        /// </summary>
        public abstract void Unload();

        /// <summary>
        /// 接收通知
        /// </summary>
        /// <param name="ready"></param>
        /// <param name="res"></param>
        protected abstract void OnReceiveNotification(bool ready, Res res);
    }
}