/*
 * @Author: fasthro
 * @Date: 2019-11-26 19:39:06
 * @Description: 版本配置编辑
 */
using System.Collections.Generic;
using FastEngine.Core;
using FastEngine.Utils;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Version
{
    public class VersionCEW : CEWBase
    {
        private AppConfig m_app;
        private string m_appPath;
        private VersionConfig m_version;
        private string m_versionPath;
        private BuildPackConfig m_build;
        private string m_buildPath;
        private BuildHotfixConfig m_hotfix;
        private string m_hotfixPath;

        private int m_buildTarget;
        private string[] m_targetStrs = new string[] { "Android", "iOS", "Windows" };

        protected override void OnInitialize()
        {
            titleContent.text = "Version 配置编辑器";

            path = FilePathUtils.Combine(Application.dataPath, "Resources", "AppConfig.json");
            m_appPath = path;
            m_app = LoadConfig<AppConfig>();
            if(m_app.supportedLanuages == null) m_app.supportedLanuages = new List<SystemLanguage>();

            path = AppUtils.EditorConfigPath("VersionConfig");
            m_versionPath = path;
            m_version = LoadConfig<VersionConfig>();

            path = AppUtils.EditorConfigPath("BuildPackConfig");
            m_buildPath = path;
            m_build = LoadConfig<BuildPackConfig>();

            path = AppUtils.EditorConfigPath("BuildHotfixConfig");
            m_hotfixPath = path;
            m_hotfix = LoadConfig<BuildHotfixConfig>();

            m_buildTarget = 0;
        }

        void OnGUI()
        {
            if (GUILayout.Button("Save"))
            {
                // 支持语言列表去重(此处不考虑效率问题啦)
                List<SystemLanguage> sames = new List<SystemLanguage>();
                for (int i = 0; i < m_app.supportedLanuages.Count; i++)
                {
                    if (!sames.Contains(m_app.supportedLanuages[i]))
                    {
                        sames.Add(m_app.supportedLanuages[i]);
                    }
                }
                m_app.supportedLanuages = sames;

                FilePathUtils.FileWriteAllText(m_appPath, JsonMapper.ToJson(m_app));
                FilePathUtils.FileWriteAllText(m_versionPath, JsonMapper.ToJson(m_version));
                FilePathUtils.FileWriteAllText(m_buildPath, JsonMapper.ToJson(m_build));
                FilePathUtils.FileWriteAllText(m_hotfixPath, JsonMapper.ToJson(m_hotfix));
            }
            #region app
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("App Config");

            EditorGUILayout.BeginVertical("box");
            // runModel
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("App Run Model");
            m_app.runModel = (AppRunModel)EditorGUILayout.EnumPopup("", m_app.runModel);
            EditorGUILayout.EndVertical();
            // QATest
            EditorGUILayout.BeginVertical("box");
            m_app.QATest = EditorGUILayout.ToggleLeft("QA Test", m_app.QATest);
            EditorGUILayout.EndVertical();
            // log
            EditorGUILayout.BeginVertical("box");
            m_app.enableLog = EditorGUILayout.ToggleLeft("Enable Log", m_app.enableLog);
            EditorGUILayout.EndVertical();
            // language
            EditorGUILayout.BeginVertical("box");
            m_app.useSystemLanguage = EditorGUILayout.ToggleLeft("Use System Language", m_app.useSystemLanguage);
            EditorGUILayout.EndVertical();
            if (!m_app.useSystemLanguage)
            {
                EditorGUILayout.BeginVertical("box");
                m_app.language = (SystemLanguage)EditorGUILayout.EnumPopup("", m_app.language);
                EditorGUILayout.EndVertical();
            }
            // default language
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Default Language");
            m_app.defaultLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("", m_app.defaultLanguage);
            EditorGUILayout.EndVertical();
            // app support language
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal("box");
            GUILayout.Label("Supported Languages");
            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus")))
            {
                m_app.supportedLanuages.Add(Application.systemLanguage);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical("box");
            for (int i = 0; i < m_app.supportedLanuages.Count; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                m_app.supportedLanuages[i] = (SystemLanguage)EditorGUILayout.EnumPopup("", m_app.supportedLanuages[i]);
                if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash")))
                {
                    m_app.supportedLanuages.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
            if (m_app.supportedLanuages.Count == 0)
            {
                GUILayout.Label("The application does not support any language types");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            #endregion

            #region version
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Version Config");

            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("Version");
            EditorGUILayout.BeginHorizontal("box");
            m_version.large = Mathf.Abs(EditorGUILayout.IntField(m_version.large));
            m_version.middle = Mathf.Abs(EditorGUILayout.IntField(m_version.middle));
            m_version.small = Mathf.Abs(EditorGUILayout.IntField(m_version.small));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Resource Version");
            m_version.resource = EditorGUILayout.IntField(m_version.resource);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            #endregion

            #region build
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Build Config");

            EditorGUILayout.BeginVertical("box");
            m_build.cleanBuild = EditorGUILayout.ToggleLeft("Clean Build", m_build.cleanBuild);
            EditorGUILayout.EndVertical();

            m_buildTarget = GUILayout.Toolbar(m_buildTarget, m_targetStrs);
            if (m_buildTarget == 0)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Keystore Name");
                m_build.keystoreName = EditorGUILayout.TextField(m_build.keystoreName);
                GUILayout.Label("Keystore Password");
                m_build.keystorePass = EditorGUILayout.PasswordField(m_build.keystorePass);
                GUILayout.Label("keyalias Name");
                m_build.keyaliasName = EditorGUILayout.TextField(m_build.keyaliasName);
                GUILayout.Label("Keystore Password");
                m_build.keyaliasPass = EditorGUILayout.PasswordField(m_build.keyaliasPass);
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");
                m_build.andoridIL2CPP = EditorGUILayout.ToggleLeft("IL2CPP", m_build.andoridIL2CPP);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
            }
            else if (m_buildTarget == 1)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginVertical("box");
                m_build.iOSIL2CPP = EditorGUILayout.ToggleLeft("IL2CPP", m_build.iOSIL2CPP);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
            }
            else if (m_buildTarget == 2)
            {
                EditorGUILayout.BeginVertical("box");
                m_build.WindowsIL2CPP = EditorGUILayout.ToggleLeft("IL2CPP", m_build.WindowsIL2CPP);
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
            #endregion

        }
    }
}