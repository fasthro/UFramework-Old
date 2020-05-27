/*
 * @Author: fasthro
 * @Date: 2019-11-26 19:39:06
 * @Description: 版本配置编辑
 */
using System.Collections.Generic;
using FastEngine.Core;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Version
{
    public class VersionEditorWindow : FastEditorWindow
    {
        static AppConfig appConfig;
        static AppRunModel lastAppRunModel;

        static BuildConfig buildConfig;
        static int buildTarget;
        static string[] targetStrs = new string[] { "Android", "iOS", "Windows" };

        protected override void OnInitialize()
        {
            titleContent.text = "应用配置编辑器";

            appConfig = Config.ReadEditorDirectory<AppConfig>();
            lastAppRunModel = appConfig.runModel;

            buildConfig = Config.ReadEditorDirectory<BuildConfig>();
            buildTarget = 0;
        }

        void OnGUI()
        {
            if (GUILayout.Button("Save"))
            {
                #region app config
                if (appConfig.resolutionWidth <= 0)
                {
                    appConfig.resolutionWidth = 1280;
                }
                if (appConfig.resolutionHeight <= 0)
                {
                    appConfig.resolutionHeight = 720;
                }
                // 支持语言列表去重(此处不考虑效率问题啦)
                List<SystemLanguage> sames = new List<SystemLanguage>();
                for (int i = 0; i < appConfig.supportedLanuages.Count; i++)
                {
                    if (!sames.Contains(appConfig.supportedLanuages[i]))
                    {
                        sames.Add(appConfig.supportedLanuages[i]);
                    }
                }
                appConfig.supportedLanuages = sames;
                // 
                Config.Write<AppConfig>(appConfig);
                #endregion

                #region builk config

                Config.Write<BuildConfig>(buildConfig);

                #endregion

                AssetDatabase.Refresh();
            }

            #region app config

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("App Config");

            EditorGUILayout.BeginVertical("box");
            // runModel
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("App Run Model");
            appConfig.runModel = (AppRunModel)EditorGUILayout.EnumPopup("", appConfig.runModel);
            EditorGUILayout.EndVertical();
            // change run model
            if (lastAppRunModel != appConfig.runModel)
            {
                lastAppRunModel = appConfig.runModel;
                appConfig.enableLog = appConfig.runModel != AppRunModel.Release;
                if (appConfig.runModel == AppRunModel.Release)
                {
                    appConfig.useSystemLanguage = true;
                    appConfig.defaultLanguage = SystemLanguage.English;
                }
                else
                {
                    appConfig.useSystemLanguage = false;
                    appConfig.language = SystemLanguage.English;
                    appConfig.defaultLanguage = SystemLanguage.English;
                }
            }
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            if (appConfig.runModel == AppRunModel.Test)
            {
                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Screen Resolution");
                EditorGUILayout.BeginVertical("box");
                appConfig.resolutionWidth = EditorGUILayout.IntField("Width", appConfig.resolutionWidth);
                if (appConfig.resolutionWidth <= 0)
                {
                    appConfig.resolutionWidth = 1280;
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical("box");
                appConfig.resolutionHeight = EditorGUILayout.IntField("Height", appConfig.resolutionHeight);
                if (appConfig.resolutionHeight <= 0)
                {
                    appConfig.resolutionHeight = 720;
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();
            }
#endif
            // log
            EditorGUILayout.BeginVertical("box");
            appConfig.enableLog = EditorGUILayout.ToggleLeft("Log Enable", appConfig.enableLog);
            EditorGUILayout.EndVertical();
            // language
            EditorGUILayout.BeginVertical("box");
            appConfig.useSystemLanguage = EditorGUILayout.ToggleLeft("Use System Language", appConfig.useSystemLanguage);
            EditorGUILayout.EndVertical();
            if (!appConfig.useSystemLanguage)
            {
                EditorGUILayout.BeginVertical("box");
                appConfig.language = (SystemLanguage)EditorGUILayout.EnumPopup("", appConfig.language);
                EditorGUILayout.EndVertical();
            }
            // default language
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Default Language");
            appConfig.defaultLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("", appConfig.defaultLanguage);
            EditorGUILayout.EndVertical();
            // app support language
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Supported Languages");
            for (int i = 0; i < appConfig.supportedLanuages.Count; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                appConfig.supportedLanuages[i] = (SystemLanguage)EditorGUILayout.EnumPopup("", appConfig.supportedLanuages[i]);
                if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.Height(18)))
                {
                    appConfig.supportedLanuages.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
            if (appConfig.supportedLanuages.Count == 0)
            {
                GUILayout.Label("The application does not support any language types");
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus")))
            {
                appConfig.supportedLanuages.Add(Application.systemLanguage);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
            #endregion

            #region build
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Build Config");

            EditorGUILayout.BeginVertical("box");
            buildConfig.cleanBuild = EditorGUILayout.ToggleLeft("Clean Build", buildConfig.cleanBuild);
            EditorGUILayout.EndVertical();

            buildTarget = GUILayout.Toolbar(buildTarget, targetStrs);

            if (buildTarget == 0)
            {
                DrawVersion(buildConfig.android.version);

                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Keystore Name");
                buildConfig.android.keystoreName = EditorGUILayout.TextField(buildConfig.android.keystoreName);
                GUILayout.Label("Keystore Password");
                buildConfig.android.keystorePass = EditorGUILayout.PasswordField(buildConfig.android.keystorePass);
                GUILayout.Label("keyalias Name");
                buildConfig.android.keyaliasName = EditorGUILayout.TextField(buildConfig.android.keyaliasName);
                GUILayout.Label("Keystore Password");
                buildConfig.android.keyaliasPass = EditorGUILayout.PasswordField(buildConfig.android.keyaliasPass);
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");
                buildConfig.android.il2cpp = EditorGUILayout.ToggleLeft("il2cpp", buildConfig.android.il2cpp);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
            }
            else if (buildTarget == 1)
            {
                DrawVersion(buildConfig.ios.version);

                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginVertical("box");
                buildConfig.ios.il2cpp = EditorGUILayout.ToggleLeft("il2cpp", buildConfig.ios.il2cpp);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
            }
            else if (buildTarget == 2)
            {
                DrawVersion(buildConfig.windows.version);

                EditorGUILayout.BeginVertical("box");
                buildConfig.windows.il2cpp = EditorGUILayout.ToggleLeft("il2cpp", buildConfig.windows.il2cpp);
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
            #endregion

        }

        void DrawVersion(VersionConfig version)
        {
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Version Config");

            EditorGUILayout.BeginVertical("box");

            GUILayout.Label("Version");
            EditorGUILayout.BeginHorizontal("box");
            version.large = Mathf.Abs(EditorGUILayout.IntField(version.large));
            version.middle = Mathf.Abs(EditorGUILayout.IntField(version.middle));
            version.small = Mathf.Abs(EditorGUILayout.IntField(version.small));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Resource Version");
            version.resource = EditorGUILayout.IntField(version.resource);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }
    }
}