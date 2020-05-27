/*
 * @Author: fasthro
 * @Date: 2020-05-08 15:41:23
 * @Description: 登录渠道基类
 */

using UnityEngine;

namespace FastEngine.Core
{
    public interface ILoginChannel
    {
        void Login();
        void Logout();
    }
}