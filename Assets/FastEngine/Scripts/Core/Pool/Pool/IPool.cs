/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:23
 * @Description: 池接口
 */

namespace FastEngine.Core
{
    public interface IPool<T>
    {
        int count { get; }

        T Allocate();

        bool Recycle(T obj);
    }
}