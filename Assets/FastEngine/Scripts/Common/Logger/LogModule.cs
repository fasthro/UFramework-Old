/*
 * @Author: fasthro
 * @Date: 2019-10-17 11:25:10
 * @Description: Log 模块定义
 */
namespace FastEngine.Common
{
    public enum LogModule
    {
        Unity,                // Unity
        Network,              // 网络
    }

    public class LogModuleWrap
    {
        public static string Wrap(LogModule module, string message)
        {
            return string.Format("[{0}] {1}", module.ToString(), message);
        }
    }
}