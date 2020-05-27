/*
 * @Author: fasthro
 * @Date: 2020-05-08 15:42:41
 * @Description: 数据分析渠道基类
 */

namespace FastEngine.Core
{
    public interface IDNAChannel
    {
        void SendEvent(string eventName);
        void SendEvent(string eventName, ParamDictionary param);
    }
}
