/*
 * @Author: fasthro
 * @Date: 2019-08-08 19:28:16
 * @Description: Socket 常量
 */
namespace FastEngine.Core
{
    public static class SocketConst
    {
        // 协议头 - 协议总长度(字节)
        public const int HEAD_BODY_SIZE = 4;
        // 协议头 - 特殊标识长度(字节)
        public const int HEAD_FLAG_SIZE = 4;
        // 协议头 - 协议号长度(字节)
        public const int HEAD_COMMAND_SIZE = 4;
        // 协议头 - Session 长度(字节)
        public const int HEAD_SESSION_SIZE = 4;
    }
}