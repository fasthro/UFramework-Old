/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:36:12
 * @Description: 资源状态
 */
namespace FastEngine.Core
{
    public enum ResState
    {
        Waiting,          // 未加载
        Loading,          // 正在加载
        Failed,           // 加载失败
        Ready,            // 已加载
    }
}