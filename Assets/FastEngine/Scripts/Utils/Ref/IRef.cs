/*
 * @Author: fasthro
 * @Date: 2019-10-28 16:35:14
 * @Description: 引用计数接口
 */
namespace FastEngine.Utils
{
    public interface IRef
    {
        int refCount { get; }
        void Retain();
        void Release();
    }
}