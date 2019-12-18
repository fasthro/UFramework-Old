/*
 * @Author: fasthro
 * @Date: 2019-11-28 15:41:00
 * @Description: 热更新
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FastEngine.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace FastEngine.Core
{
    // -初始化版本
    // --- 加载版本配置(Data && Content)目录
    // 检查基础资源解压
    // --- 对比版本配置和检查数据目录决定是否进行基本资源解压
    // --- 如果资源目录版本小于Content目录版本则进行解压，反之直接进行更新检查
    // 解压基础资源解压
    // --- 读取Content目录zip文件，解压到数据目录
    // 检查更新
    // --- 读取远程Hotfix配置，
    //     hotfix:true 进行热更新比对，本地文件md5值与远程配置中文件md5值不相等，或者本地不存在远程的文件列表中的文件，添加到更新下载列表
    //     hotfix:false 热更新流程完成
    // 更新
    // 更新完毕

    [MonoSingletonPath("FastEngine/HotfixUpdate")]
    public class HotfixUpdate : MonoSingleton<HotfixUpdate>
    {
        // 远程根地址(使用者外部配置)
        public static string RemoteRoot;
        // 远程最新版本跟地址
        public static string RemoteLaseVersionRoot;

        // hotfix event callback
        private HotfixEventCallback m_eventCallback;
        // unpack callback
        private UnpackCallback m_unpackBaseCallback;

        // 数据目录版本配置
        private VersionConfig m_version;
        // 原始版本配置(包体版本配置)
        private VersionConfig m_rawVersion;

        // hotfixConfig
        private HotfixConfig m_hotfix;

        // 下载列表
        private HashSet<string> m_downloadHashSet = new HashSet<string>();
        private List<string> m_downloadFiles = new List<string>();
        private List<DownloadHandlerPro> m_downloadHandlers = new List<DownloadHandlerPro>();
        private List<UnpackCallback> m_unpackUpdateCallbacks = new List<UnpackCallback>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="callback"></param>
        public void Initialize(HotfixEventCallback eventCallback)
        {
            // callback
            this.m_eventCallback = eventCallback;
            // 初始化版本
            InitVersion();
        }

        void Update()
        {
            if (m_eventCallback == null) return;

            // 解压基础资源进度
            if (m_eventCallback.hotfixEvent == HotfixEvent.UnpackBase)
            {
                if (m_unpackBaseCallback != null)
                {
                    m_eventCallback.Progress(m_unpackBaseCallback.progress);
                    if (m_unpackBaseCallback.done)
                    {
                        InitUpdate();
                    }
                }
            }

            // 下载热更新资源
            if (m_eventCallback.hotfixEvent == HotfixEvent.DownloadUpdate)
            {
                bool isDone = true;
                float progress = 0;
                for (int i = 0; i < m_downloadHandlers.Count; i++)
                {
                    if (!m_downloadHandlers[i].done) isDone = false;
                    progress += m_downloadHandlers[i].progress;
                }
                m_eventCallback.Progress(progress / (float)m_downloadHandlers.Count);
                if (isDone) UnpackUpdate();
            }

            // 解压热更新资源进度
            if (m_eventCallback.hotfixEvent == HotfixEvent.UnpackUpdate)
            {
                bool isDone = true;
                float progress = 0;
                for (int i = 0; i < m_unpackUpdateCallbacks.Count; i++)
                {
                    if (!m_unpackUpdateCallbacks[i].done) isDone = false;
                    progress += m_unpackUpdateCallbacks[i].progress;
                }
                m_eventCallback.Progress(progress / (float)m_unpackUpdateCallbacks.Count);
                if (isDone) HotfixFinished();
            }

            // 下载进度
            if (m_eventCallback.hotfixEvent == HotfixEvent.DownloadUpdate)
            {

            }
        }

        #region version config
        /// <summary>
        /// 初始化版本配置
        /// </summary>
        private void InitVersion()
        {
            m_eventCallback.Callback(HotfixEvent.InitVersion);

            // 加载数据目录版本配置
            var dp = FilePathUtils.Combine(AppUtils.DataRootDirectory(), "VersionConfig.json");
            if (FilePathUtils.FileExists(dp))
                m_version = AppUtils.LoadConfig<VersionConfig>(dp);

            // 加载原始版本配置
            var rp = AppUtils.AppRawPath() + "VersionConfig.json";
            if (Application.platform == RuntimePlatform.Android)
            {
                CoroutineFactory.CreateAndStart(AndroidReadRawVersion(rp), (result) =>
                {
                    CheckUnpackBase();
                });
            }
            else
            {
                m_rawVersion = AppUtils.LoadConfig<VersionConfig>(rp);
                CheckUnpackBase();
            }
        }

        /// <summary>
        /// Android 读取Raw版本配置
        /// </summary>
        /// <returns></returns>
        IEnumerator AndroidReadRawVersion(string rp)
        {
            var request = new UnityWebRequest(rp);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.isDone)
            {
                m_rawVersion = AppUtils.LoadConfig2<VersionConfig>(request.downloadHandler.text);
            }
        }

        /// <summary>
        /// 检查是否需要解压基础资源
        /// </summary>
        private void CheckUnpackBase()
        {
            m_eventCallback.Callback(HotfixEvent.CheckBase);

            // 检查bundle目录
            var unpack = !Directory.Exists(AppUtils.BundleRootDirectory());
            // 版本配置检查
            if (m_version != null && m_rawVersion != null)
            {
                var result = VersionConfig.CompareVersion(m_version, m_rawVersion);
                if (result == -1)
                {
                    unpack = true;
                }
            }
            if (m_rawVersion != null && m_version == null) m_version = m_rawVersion;
            if (m_rawVersion == null)
            {
                // 非法客户端
                Debug.Log("非法客户端");
            }
            else
            {
                // 解压/检查更新
                if (unpack) UnpackBase();
                else InitUpdate();
            }
        }
        #endregion

        #region 基础资源
        /// <summary>
        /// 解压基础资源到持久化数据目录
        /// </summary>
        private void UnpackBase()
        {
            m_eventCallback.Callback(HotfixEvent.UnpackBase);
            m_unpackBaseCallback = new UnpackCallback(m_version.compressFileTotalCount);
            var source = AppUtils.AppRawPath() + PlatformUtils.PlatformId() + ".zip";
            if (Application.platform == RuntimePlatform.Android)
            {
                CoroutineFactory.CreateAndStart(AndroidReadBase(source));
            }
            else
            {
                var task = new UnzipTreadTask(source, AppUtils.DataRootDirectory(), m_unpackBaseCallback);
                ThreadTaskService.Instance.AddTask(task);
            }
        }

        /// <summary>
        /// android 解压特殊处理
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IEnumerator AndroidReadBase(string source)
        {
            var request = new UnityWebRequest(source);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.isDone)
            {
                var stream = new MemoryStream(request.downloadHandler.data);
                var task = new UnzipTreadTask(stream, AppUtils.DataRootDirectory(), m_unpackBaseCallback);
                ThreadTaskService.Instance.AddTask(task);
            }
        }
        #endregion

        #region update
        /// <summary>
        /// 初始化更新
        /// </summary>
        private void InitUpdate()
        {
            m_eventCallback.Callback(HotfixEvent.InitUpdate);
            CoroutineFactory.CreateAndStart(ReadRemoteHotfixConfig());
        }

        /// <summary>
        /// 读取远程热更新配置
        /// </summary>
        /// <returns></returns>
        IEnumerator ReadRemoteHotfixConfig()
        {
            var request = new UnityWebRequest(RemoteLaseVersionRoot + "HotfixConfig.json");
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.isDone)
            {
                m_hotfix = AppUtils.LoadConfig2<HotfixConfig>(request.downloadHandler.text);
                if (m_hotfix.hotfix) CheckUpdate();
                else HotfixFinished();
            }
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        private void CheckUpdate()
        {
            m_eventCallback.Callback(HotfixEvent.CheckUpdate);
            m_downloadHashSet.Clear();
            m_downloadFiles.Clear();
            HotfixConfig.FileInfo remoteFileInfo = null;
            string localFilePath = "";
            string hash = "";
            for (int i = 0; i < m_hotfix.fileInfos.Length; i++)
            {
                remoteFileInfo = m_hotfix.fileInfos[i];
                localFilePath = FilePathUtils.Combine(AppUtils.BundleRootDirectory(), remoteFileInfo.path);
                if (!FilePathUtils.FileExists(localFilePath))
                {
                    if (!m_downloadHashSet.Contains(remoteFileInfo.version))
                    {
                        m_downloadHashSet.Add(remoteFileInfo.version);
                        m_downloadFiles.Add(remoteFileInfo.version);
                    }
                }
                else
                {
                    hash = AppUtils.MD5File(localFilePath);
                    if (!hash.Equals(remoteFileInfo.hash))
                    {
                        if (!m_downloadHashSet.Contains(remoteFileInfo.version))
                        {
                            m_downloadHashSet.Add(remoteFileInfo.version);
                            m_downloadFiles.Add(remoteFileInfo.version);
                        }
                        FilePathUtils.DeleteFile(localFilePath);
                    }
                }
            }

            if (m_downloadFiles.Count > 0) DownloadUpdate();
            else HotfixFinished();
        }

        /// <summary>
        /// 下载更新
        /// </summary>
        private void DownloadUpdate()
        {
            m_eventCallback.Callback(HotfixEvent.DownloadUpdate);
            m_downloadHandlers.Clear();
            for (int i = 0; i < m_downloadFiles.Count; i++)
            {
                var localPath = FilePathUtils.Combine(AppUtils.DataRootDirectory(), m_downloadFiles[i] + ".zip");
                // 如果已经下载完成没有被解压那么就不需要下载此文件
                if (!FilePathUtils.FileExists(localPath))
                {
                    var url = RemoteRoot + m_downloadFiles[i] + "/HotfixRes.zip";
                    m_downloadHandlers.Add(DownloadPro.Instance.Download(url, localPath));
                }
            }
        }

        /// <summary>
        /// 解压热更新资源
        /// </summary>
        private void UnpackUpdate()
        {
            m_eventCallback.Callback(HotfixEvent.UnpackUpdate);
            m_unpackUpdateCallbacks.Clear();
            for (int i = 0; i < m_downloadFiles.Count; i++)
            {
                var zipPath = FilePathUtils.Combine(AppUtils.DataRootDirectory(), m_downloadFiles[i] + ".zip");
                var unpackCallback = new UnpackCallback(100, zipPath);
                m_unpackUpdateCallbacks.Add(unpackCallback);
                var task = new UnzipTreadTask(zipPath, AppUtils.BundleRootDirectory(), unpackCallback);
                ThreadTaskService.Instance.AddTask(task);
            }
        }
        #endregion

        /// <summary>
        /// 热更新完成
        /// </summary>
        private void HotfixFinished()
        {
            m_eventCallback.Callback(HotfixEvent.HotfixFinished);
        }
    }
}