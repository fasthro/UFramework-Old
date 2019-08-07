/*
 * @Author: fasthro
 * @Date: 2019-06-22 20:36:28
 * @Description: 异步接口，对象需要实现此接口才能被 RunAsync 执行
 */
using System;
using System.Collections;

namespace FastEngine.Core
{
    public interface IRunAsyncObject
    {
        IEnumerator AsyncRun(IRunAsync async);
    }
}