/*
 * @Author: fasthro
 * @Date: 2019-11-11 19:20:23
 * @Description: 国际化配置编辑器
 */
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using LitJson;

namespace FastEngine.Editor.I18n
{
    public class EditorWindow : FastEditorWindow
    {
        static i18nConfig config;

        Vector3 _scrollPosition;

        protected override void OnInitialize()
        {
            titleContent.text = "i18n 配置编辑器";
            config = AppUtils.ReadEditorConfig<i18nConfig>();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Save"))
            {
                AppUtils.WriteEditorConfig<i18nConfig>(JsonMapper.ToJson(config));
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Generate"))
            {
                AppUtils.WriteEditorConfig<i18nConfig>(JsonMapper.ToJson(config));
                AssetDatabase.Refresh();

                LocalizationEditor.Generate();
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus")))
            {
                config.languages.Add(SystemLanguage.Unknown);
            }

            EditorGUILayout.EndHorizontal();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            for (int i = 0; i < config.languages.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                config.languages[i] = (SystemLanguage)EditorGUILayout.EnumPopup("", config.languages[i]);
                if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus")))
                {
                    config.languages.RemoveAt(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
