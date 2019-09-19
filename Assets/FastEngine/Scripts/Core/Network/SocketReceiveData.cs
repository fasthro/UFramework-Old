/*
 * @Author: fasthro
 * @Date: 2019-08-08 17:14:25
 * @Description: 网络协议数据结构
 */
namespace FastEngine.Core
{
    [System.Serializable]
    public class SocketReceiveProtocal
    {
        // 协议号
        public int protocal;
        // 协议 Session
        public int session;
        // 协议数据
        public byte[] data;
        // 协议数据长度
        public int dataSize;
        // 协议总长
        public int bufferSize;

    }
}