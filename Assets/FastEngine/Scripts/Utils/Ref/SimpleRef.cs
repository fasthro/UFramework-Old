/*
 * @Author: fasthro
 * @Date: 2019-10-28 16:36:24
 * @Description: 简单的引用计数基类
 */
namespace FastEngine.Ref
{
    public class SimpleRef : IRef
    {
        public int refCount { get; private set; }
        public bool isRefZero { get { return refCount == 0; } }

        public void Release()
        {
            refCount--;
            if (refCount == 0) OnZeroRef();
        }

        public void Retain()
        {
            refCount++;
        }

        protected virtual void OnZeroRef()
        {

        }
    }
}