/*
 * @Author: fasthro
 * @Date: 2020-05-08 20:05:32
 * @Description: 渠道基类
 */

using UnityEngine;

namespace FastEngine.Core
{
    public abstract class SDKChannel : ILoginChannel, IDNAChannel, IPayChannel
    {
        public SDKChannelInfo channelInfo { get; private set; }
        public bool initializing { get; protected set; }
        public bool initialized { get; protected set; }
        public bool inOperation { get; private set; }

        // 标志-登录
        private bool loginFlag;

        protected bool logEnabled;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public SDKChannel(SDKChannelInfo info)
        {
            initialized = false;
            logEnabled = Logger.logEnabled;
            channelInfo = info;
        }

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            if (!initializing && !initialized)
            {
                initializing = true;
                OnInitialize();
            }
        }

        /// <summary>
        /// 初始化-子类实现
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// 初始化-完成
        /// </summary>
        protected void OnInitializeComplete()
        {
            initialized = true;
            inOperation = false;

            if (loginFlag)
            {
                loginFlag = false;
                Login();
            }
        }

        #endregion

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            if (!inOperation)
            {
                inOperation = true;
                if (initialized)
                {
                    log("login.");
                    OnLogin();
                }
                else
                {
                    log("login initialize.");
                    loginFlag = true;
                    Initialize();
                }
            }
            else log("in operation login");
        }

        /// <summary>
        /// 登录-子类实现
        /// </summary>
        protected virtual void OnLogin() { }

        /// <summary>
        /// 登出
        /// </summary>
        public void Logout()
        {
            OnLogout();
        }

        /// <summary>
        /// 登出-子类实现
        /// </summary>
        protected virtual void OnLogout() { }

        /// <summary>
        /// 登录-成功
        /// </summary>
        /// <param name="info"></param>
        protected void OnLoginSucceed(LoginCallbackInfo info)
        {
            inOperation = false;
            info.isSucceed = true;
            SDKCallback.Broadcast(SDKCallbackEvent.LOGIN, info);
        }

        /// <summary>
        /// 登录-失败
        /// </summary>
        /// <param name="message"></param>
        protected void OnLoginError(string message)
        {
            inOperation = false;

            LoginCallbackInfo info = new LoginCallbackInfo(channelInfo.channelName);
            info.isError = true;
            info.message = message;

            SDKCallback.Broadcast(SDKCallbackEvent.LOGIN, info);
        }

        /// <summary>
        /// 登录-取消
        /// </summary>
        protected void OnLoginCancel()
        {
            inOperation = false;

            LoginCallbackInfo info = new LoginCallbackInfo(channelInfo.channelName);
            info.isCancel = true;

            SDKCallback.Broadcast(SDKCallbackEvent.LOGIN, info);
        }
        #endregion

        #region 数据采集

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventName"></param>
        public void SendEvent(string eventName)
        {
            OnSendEvent(eventName);
            if (Logger.logEnabled)
            {
                log("SendEvent:" + eventName);
            }
        }

        /// <summary>
        /// 发送事件-子类实现
        /// </summary>
        /// <param name="eventName"></param>
        protected virtual void OnSendEvent(string eventName) { }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="param"></param>
        public void SendEvent(string eventName, ParamDictionary param)
        {
            OnSendEvent(eventName, param);
            if (Logger.logEnabled)
            {
                log("SendEvent:" + eventName + " -> " + param.AsJson());
            }
        }

        /// <summary>
        /// 发送事件-子类实现
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="param"></param>
        protected virtual void OnSendEvent(string eventName, ParamDictionary param) { }

        #endregion

        public void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        /// <summary>
        /// log
        /// </summary>
        /// <param name="message"></param>
        protected void log(object message)
        {
            if (Logger.logEnabled)
            {
                Debug.Log("[SDK] " + channelInfo.channelName + " > " + message.ToString());
            }
        }
    }
}