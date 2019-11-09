/*
 * @Author: fasthro
 * @Date: 2019-11-09 15:17:31
 * @Description: AssetBundle 配置编辑器
 */
using System.Collections.Generic;
using System.IO;
using FastEngine.Core;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.AssetBundle
{
    public class ConfigEditorWindow : EditorWindow
    {
        static ConfigEditorWindow inst;
        static Vector3 scrollPosition;
        static List<Pack> packs;

        public static void OpenWindow()
        {
            inst = GetWindow<ConfigEditorWindow>(false, "");
            inst.titleContent.text = "AssetBundle 配置编辑器";
            packs = LoadConfig();
        }

        void OnEnable() { if (inst == null) OpenWindow(); }
        void OnDestroy() { inst = null; }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Save"))
            {
                Save();
            }

            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus")))
            {
                packs.Add(new Pack());
            }

            EditorGUILayout.EndHorizontal();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < packs.Count; i++)
            {
                var pack = packs[i];

                EditorGUILayout.BeginVertical("box");

                pack.editorShow = GUILayout.Toggle(pack.editorShow, EditorGUIUtility.IconContent("ViewToolOrbit"), "button");

                if (pack.editorShow)
                {
                    // 
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("目标资源路径");
                    if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus")))
                    {
                        packs.RemoveAt(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    pack.target = EditorGUILayout.TextField(pack.target);

                    //
                    EditorGUILayout.LabelField("Bundle输出路径");
                    pack.bundlePath = EditorGUILayout.TextField(pack.bundlePath);

                    if (pack.model != BuildModel.Standard && pack.model != BuildModel.File)
                    {
                        EditorGUILayout.LabelField("资源匹配模式");
                        pack.pattern = EditorGUILayout.TextField(pack.pattern);
                    }
                    else
                    {
                        pack.pattern = "*.*";
                    }

                    // 
                    EditorGUILayout.LabelField("打包模式");
                    pack.model = (BuildModel)EditorGUILayout.EnumPopup("", pack.model);

                    //
                    EditorGUILayout.LabelField("导出映射配置");
                    pack.genMapping = (GenerateMapping)EditorGUILayout.EnumPopup("", pack.genMapping);
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(pack.target);
                    if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus")))
                    {
                        packs.RemoveAt(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();
            }

            EditorGUILayout.EndScrollView();
        }

        static void Save()
        {
            var mp = AssetBundlePath.EditorConfigFilePath();
            if (File.Exists(mp)) File.Delete(mp);
            File.WriteAllText(mp, JsonMapper.ToJson(packs));
        }

        public static List<Pack> LoadConfig()
        {
            var cp = AssetBundlePath.EditorConfigFilePath();
            if (!File.Exists(cp))
                return new List<Pack>();
            return JsonMapper.ToObject<List<Pack>>(File.ReadAllText(cp));
        }
    }
}