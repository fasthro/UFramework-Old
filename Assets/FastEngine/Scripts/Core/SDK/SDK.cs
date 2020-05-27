/*
 * @Author: fasthro
 * @Date: 2020-05-08 15:12:23
 * @Description: SDK
 */

using System.Reflection;
using UnityEngine;

namespace FastEngine.Core
{
    [MonoSingletonPath("FastEngine/SDK")]
    public class SDK : MonoSingleton<SDK>
    {
        #region service
        /// <summary>
        /// 登录服务
        /// </summary>
        /// <value></value>
        public LoginService loginService { get; private set; }

        /// <summary>
        /// 支付服务
        /// </summary>
        /// <value></value>
        public PayService payService { get; private set; }

        /// <summary>
        /// 数据分析服务
        /// </summary>
        /// <value></value>
        public DNAService dnaService { get; private set; }

        #endregion

        #region config info
        // 平台配置信息
        public SDKPlatformInfo platformInfo { get; private set; }
        #endregion

        public override void InitializeSingleton()
        {
            // service
            loginService = new LoginService();
            payService = new PayService();
            dnaService = new DNAService();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            var config = Config.ReadResourceDirectory<SDKConfig>();
#if UNITY_EDITOR
            platformInfo = config.develop;
#elif UNITY_ANDROID
            platformInfo = config.android;
#elif UNITY_IOS
            platformInfo = config.iOS;
#endif
            if (platformInfo == null) return;
            // initialize service
            loginService.Initialize(platformInfo.login);
            payService.Initialize(platformInfo.pay);
            dnaService.Initialize(platformInfo.dna);
        }

        void Update()
        {
            loginService.Update();
            payService.Update();
            dnaService.Update();
        }
    }
}