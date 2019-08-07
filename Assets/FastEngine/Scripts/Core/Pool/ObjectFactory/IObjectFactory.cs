/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:23
 * @Description: 对象工厂接口
 */

namespace FastEngine.Core
{
    public interface IObjectFactory<T>
    {
        T Create();
    }
}