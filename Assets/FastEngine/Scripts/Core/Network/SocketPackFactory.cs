/*
 * @Author: fasthro
 * @Date: 2019-10-17 17:45:32
 * @Description: 协议包工厂
 */

namespace FastEngine.Core
{
    public static class SocketPackFactory
    {
        /// <summary>
        /// 创建写入包
        /// </summary>
        public static SocketPack CreateWriter(int cmd)
        {
            return new SocketPack(cmd);
        }

        /// <summary>
        /// 创建读取包
        /// </summary>
        /// <param name="data"></param>
        public static SocketPack CreateReader(int cmd, byte[] data)
        {
            return new SocketPack(cmd, data);
        }
    }
}