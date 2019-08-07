/*
 * @Author: fasthro
 * @Date: 2019-06-24 14:13:54
 * @Description: 异步接口
 */
namespace FastEngine.Core
{
    public interface IRunAsync
    {
        // 异步执行完成回调
        void OnRunAsync();
    }
}