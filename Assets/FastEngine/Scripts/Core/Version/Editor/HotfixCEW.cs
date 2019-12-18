/*
 * @Author: fasthro
 * @Date: 2019-11-26 19:39:06
 * @Description: 版本配置编辑
 */
using FastEngine.Core;
using FastEngine.Utils;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Version
{
    public class HotfixCEW : CEWBase
    {
        private BuildHotfixConfig m_hotfix;

        protected override void OnInitialize()
        {
            titleContent.text = "热更新配置编辑器";
            path = AppUtils.EditorConfigPath("BuildHotfixConfig");
            m_hotfix = LoadConfig<BuildHotfixConfig>();
            if (m_hotfix.version == null) m_hotfix.version = new VersionConfig();
            if (m_hotfix.fixVersion == null) m_hotfix.fixVersion = new VersionConfig();
        }

        void OnGUI()
        {
            if (GUILayout.Button("Save"))
            {
                FilePathUtils.FileWriteAllText(path, JsonMapper.ToJson(m_hotfix));
            }
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginVertical("box");
            m_hotfix.cleanBuild = EditorGUILayout.ToggleLeft("Clean Build", m_hotfix.cleanBuild);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Resource Version");
            EditorGUILayout.BeginHorizontal("box");
            m_hotfix.version.large = Mathf.Abs(EditorGUILayout.IntField(m_hotfix.version.large));
            m_hotfix.version.middle = Mathf.Abs(EditorGUILayout.IntField(m_hotfix.version.middle));
            m_hotfix.version.small = Mathf.Abs(EditorGUILayout.IntField(m_hotfix.version.small));
            m_hotfix.version.resource = Mathf.Abs(EditorGUILayout.IntField(m_hotfix.version.resource));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Fix Resource Version");
            EditorGUILayout.BeginHorizontal("box");
            m_hotfix.fixVersion.large = Mathf.Abs(EditorGUILayout.IntField(m_hotfix.fixVersion.large));
            m_hotfix.fixVersion.middle = Mathf.Abs(EditorGUILayout.IntField(m_hotfix.fixVersion.middle));
            m_hotfix.fixVersion.small = Mathf.Abs(EditorGUILayout.IntField(m_hotfix.fixVersion.small));
            m_hotfix.fixVersion.resource = Mathf.Abs(EditorGUILayout.IntField(m_hotfix.fixVersion.resource));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }
    }
}