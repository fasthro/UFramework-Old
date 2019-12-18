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
        static VersionConfig versionConfig;
        static BuildPackConfig buildConfig;
        static BuildHotfixConfig hotfixConfig;

        static int buildTarget;
        static string[] targetStrs = new string[] { "Android", "iOS", "Windows" };

        protected override void OnInitialize()
        {
            titleContent.text = "Version 配置编辑器";
            appConfig = AppUtils.ReadConfig<AppConfig>(FilePathUtils.Combine(Application.dataPath, "Resources", "AppConfig.json"));
            versionConfig = AppUtils.ReadEditorConfig<VersionConfig>();
            buildConfig = AppUtils.ReadEditorConfig<BuildPackConfig>();
            hotfixConfig = AppUtils.ReadEditorConfig<BuildHotfixConfig>();
            buildTarget = 0;
        }

        void OnGUI()
        {
            if (GUILayout.Button("Save"))
            {
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

                AppUtils.WriteEditorConfig<AppConfig>(JsonMapper.ToJson(appConfig));
                AppUtils.WriteEditorConfig<VersionConfig>(JsonMapper.ToJson(versionConfig));
                AppUtils.WriteEditorConfig<BuildPackConfig>(JsonMapper.ToJson(buildConfig));
                AppUtils.WriteEditorConfig<BuildHotfixConfig>(JsonMapper.ToJson(hotfixConfig));

                AssetDatabase.Refresh();
            }
            #region app
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("App Config");

            EditorGUILayout.BeginVertical("box");
            // runModel
            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("App Run Model");
            appConfig.runModel = (AppRunModel)EditorGUILayout.EnumPopup("", appConfig.runModel);
            EditorGUILayout.EndVertical();
            // release Version
            EditorGUILayout.BeginVertical("box");
            appConfig.releaseVersion = EditorGUILayout.ToggleLeft("Release Version", appConfig.releaseVersion);
            EditorGUILayout.EndVertical();
            // QATest
            EditorGUILayout.BeginVertical("box");
            appConfig.QATest = EditorGUILayout.ToggleLeft("QA Test", appConfig.QATest);
            if (appConfig.releaseVersion)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("Please confirm whether it is the released version");
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
            // log
            if (!appConfig.releaseVersion)
            {
                EditorGUILayout.BeginVertical("box");
                appConfig.enableLog = EditorGUILayout.ToggleLeft("Enable Log", appConfig.enableLog);
                EditorGUILayout.EndVertical();
            }
            else appConfig.enableLog = false;
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
            EditorGUILayout.BeginHorizontal("box");
            GUILayout.Label("Supported Languages");
            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus")))
            {
                appConfig.supportedLanuages.Add(Application.systemLanguage);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical("box");
            for (int i = 0; i < appConfig.supportedLanuages.Count; i++)
            {
                EditorGUILayout.BeginHorizontal("box");
                appConfig.supportedLanuages[i] = (SystemLanguage)EditorGUILayout.EnumPopup("", appConfig.supportedLanuages[i]);
                if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash")))
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
            versionConfig.large = Mathf.Abs(EditorGUILayout.IntField(versionConfig.large));
            versionConfig.middle = Mathf.Abs(EditorGUILayout.IntField(versionConfig.middle));
            versionConfig.small = Mathf.Abs(EditorGUILayout.IntField(versionConfig.small));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Resource Version");
            versionConfig.resource = EditorGUILayout.IntField(versionConfig.resource);
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
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Keystore Name");
                buildConfig.keystoreName = EditorGUILayout.TextField(buildConfig.keystoreName);
                GUILayout.Label("Keystore Password");
                buildConfig.keystorePass = EditorGUILayout.PasswordField(buildConfig.keystorePass);
                GUILayout.Label("keyalias Name");
                buildConfig.keyaliasName = EditorGUILayout.TextField(buildConfig.keyaliasName);
                GUILayout.Label("Keystore Password");
                buildConfig.keyaliasPass = EditorGUILayout.PasswordField(buildConfig.keyaliasPass);
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");
                buildConfig.andoridIL2CPP = EditorGUILayout.ToggleLeft("IL2CPP", buildConfig.andoridIL2CPP);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
            }
            else if (buildTarget == 1)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.BeginVertical("box");
                buildConfig.iOSIL2CPP = EditorGUILayout.ToggleLeft("IL2CPP", buildConfig.iOSIL2CPP);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndVertical();
            }
            else if (buildTarget == 2)
            {
                EditorGUILayout.BeginVertical("box");
                buildConfig.WindowsIL2CPP = EditorGUILayout.ToggleLeft("IL2CPP", buildConfig.WindowsIL2CPP);
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
            #endregion

        }
    }
}