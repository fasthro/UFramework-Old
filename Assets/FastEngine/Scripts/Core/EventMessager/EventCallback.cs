/*
 * @Author: fasthro
 * @Date: 2019-08-05 20:33:23
 * @Description: 事件消息回调
 */

namespace FastEngine.Core
{
    public delegate void EventCallback();
    public delegate void EventCallback<T>(T arg);
}