/*
 * @Author: fasthro
 * @Date: 2019-11-26 19:39:06
 * @Description: 热更新配置编辑
 */
using FastEngine.Core;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Version
{
    public class HotfixEditorWindow : FastEditorWindow
    {
        static HotfixConfig hotfixConfig;

        protected override void OnInitialize()
        {
            titleContent.text = "热更新配置编辑器";
            // hotfixConfig = AppUtils.ReadEditorConfig<HotfixConfig>();
        }

        // void OnGUI()
        // {
        //     if (GUILayout.Button("Save"))
        //     {
        //         AppUtils.WriteEditorConfig<HotfixConfig>(JsonMapper.ToJson(hotfixConfig));
        //         AssetDatabase.Refresh();
        //     }
        //     EditorGUILayout.BeginVertical("box");

        //     EditorGUILayout.BeginVertical("box");
        //     hotfixConfig.cleanBuild = EditorGUILayout.ToggleLeft("Clean Build", hotfixConfig.cleanBuild);
        //     EditorGUILayout.EndHorizontal();

        //     EditorGUILayout.BeginVertical("box");
        //     GUILayout.Label("Resource Version");
        //     EditorGUILayout.BeginHorizontal("box");
        //     hotfixConfig.version.large = Mathf.Abs(EditorGUILayout.IntField(hotfixConfig.version.large));
        //     hotfixConfig.version.middle = Mathf.Abs(EditorGUILayout.IntField(hotfixConfig.version.middle));
        //     hotfixConfig.version.small = Mathf.Abs(EditorGUILayout.IntField(hotfixConfig.version.small));
        //     hotfixConfig.version.resource = Mathf.Abs(EditorGUILayout.IntField(hotfixConfig.version.resource));
        //     EditorGUILayout.EndHorizontal();
        //     EditorGUILayout.EndVertical();

        //     EditorGUILayout.BeginVertical("box");
        //     GUILayout.Label("Fix Resource Version");
        //     EditorGUILayout.BeginHorizontal("box");
        //     hotfixConfig.fixVersion.large = Mathf.Abs(EditorGUILayout.IntField(hotfixConfig.fixVersion.large));
        //     hotfixConfig.fixVersion.middle = Mathf.Abs(EditorGUILayout.IntField(hotfixConfig.fixVersion.middle));
        //     hotfixConfig.fixVersion.small = Mathf.Abs(EditorGUILayout.IntField(hotfixConfig.fixVersion.small));
        //     hotfixConfig.fixVersion.resource = Mathf.Abs(EditorGUILayout.IntField(hotfixConfig.fixVersion.resource));
        //     EditorGUILayout.EndHorizontal();
        //     EditorGUILayout.EndVertical();

        //     EditorGUILayout.EndVertical();
        // }
    }
}