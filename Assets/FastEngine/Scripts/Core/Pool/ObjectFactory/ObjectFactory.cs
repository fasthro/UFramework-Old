/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:23
 * @Description: 标准对象工厂
 */

namespace FastEngine.Core
{
    public class ObjectFactory<T> : IObjectFactory<T> where T : new()
    {
        public T Create()
        {
            return new T();
        }
    }
}