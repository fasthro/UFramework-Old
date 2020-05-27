/*
 * @Author: fasthro
 * @Date: 2020-05-22 19:47:31
 * @Description: Config Edotir Window
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using FastEngine.Core;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.FastConfig
{
    public class ConfigEditorWindow : FastEditorWindow
    {
        static string CONFIG_NAME = "BaseConfig";
        private BaseConfig baseConfig;
        private List<string> configNames;
        private string[] options = new string[] { "Editor", "Resource", "Data" };

        protected override void OnInitialize()
        {
            titleContent.text = "配置文件设置";

            // 配置名称列表
            configNames = new List<string>();
            Type[] types = Assembly.Load("Assembly-CSharp").GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsSubclassOf(typeof(ConfigObject)))
                {
                    if (!types[i].Name.Equals(CONFIG_NAME))
                        configNames.Add(types[i].Name);
                }
            }

            baseConfig = Config.ReadEditorDirectory<BaseConfig>();
            if (!baseConfig.map.ContainsKey(CONFIG_NAME))
                baseConfig.map.Add(CONFIG_NAME, ConfigAddress.Editor);
        }

        void OnGUI()
        {
            if (GUILayout.Button("Save"))
            {
                Config.Write<BaseConfig>(baseConfig);
            }

            for (int i = 0; i < configNames.Count; i++)
            {
                var cname = configNames[i];
                EditorGUILayout.BeginVertical("box");
                if (!baseConfig.map.ContainsKey(cname))
                    baseConfig.map.Add(cname, ConfigAddress.Editor);
                baseConfig.map[cname] = (ConfigAddress)EditorGUILayout.Popup(cname, (int)baseConfig.map[cname], options);
                EditorGUILayout.EndVertical();
            }
        }
    }
}