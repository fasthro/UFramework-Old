/*
 * @Author: fasthro
 * @Date: 2019-11-11 19:20:23
 * @Description: 多语言配置编辑器
 */
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using LitJson;
using FastEngine.Utils;
using FastEngine.Core;
using System.IO;

namespace FastEngine.Editor.Localization
{
    public class LocalizationCEW : CEWBase
    {
        private Vector3 m_scrollPosition;
        private List<SystemLanguage> m_languages;

        protected override void OnInitialize()
        {
            titleContent.text = "Localization 配置编辑器";
            path = AppUtils.EditorConfigPath("LocalizationConfig");
            m_languages = LoadConfig<List<SystemLanguage>>();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Save"))
            {
                FilePathUtils.FileWriteAllText(path, JsonMapper.ToJson(m_languages));

                if (GUILayout.Button("Generate"))
                {
                    LocalizationEditor.Generate();
                }
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus")))
            {
                m_languages.Add(SystemLanguage.Unknown);
            }

            EditorGUILayout.EndHorizontal();

            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);

            for (int i = 0; i < m_languages.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                m_languages[i] = (SystemLanguage)EditorGUILayout.EnumPopup("", m_languages[i]);
                if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus")))
                {
                    m_languages.RemoveAt(i);
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
