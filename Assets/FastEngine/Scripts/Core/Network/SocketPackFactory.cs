/*
 * @Author: fasthro
 * @Date: 2019-10-17 17:45:32
 * @Description: 协议包工厂
 */

using Google.Protobuf;

namespace FastEngine.Core
{
    public static class SocketPackFactory
    {
        /// <summary>
        /// 创建写入包
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static SocketPack CreateWriter(int cmd)
        {
            return new SocketPack(cmd);
        }

        /// <summary>
        /// 创建C# Protobuf写入包
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static SocketPack CreateWriter<T>(int cmd) where T : class, IMessage, new()
        {
            return new SocketPack(cmd, new T());
        }

        /// <summary>
        /// 创建 lua protobuf 写入包
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="serialize"></param>
        /// <returns></returns>
        public static SocketPack CreateWriter(int cmd, string serialize)
        {
            return new SocketPack(cmd, serialize);
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
