/*
 * @Author: fasthro
 * @Date: 2019-08-06 19:47:27
 * @Description: 池对象接口
 * -- 实现对象的对象池必须继承此接口
 */
namespace FastEngine.Core
{
    public interface IPoolObject
    {
        // 回收标识
        bool isRecycled { get; set; }

        // 回收
        void Recycle();
    }
}