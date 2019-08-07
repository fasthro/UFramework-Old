/*
 * @Author: fasthro
 * @Date: 2019-06-27 15:06:44
 * @Description: 资源引用计数器接口
 */
namespace FastEngine.Core
{
    public interface IResRefCounter
    {
        int refCount { get; }
        void Retain();
        void Release();
    }
}