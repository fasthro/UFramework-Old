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
    public class AssetBundleEditorWindow : FastEditorWindow
    {
        static AssetBundleConfig assetBundleCconfig;

        private Vector3 _scrollPosition;
        private int _bar = 0;
        private string[] _barStrs = new string[] { "AssetBundle", "Copy Source" };

        protected override void OnInitialize()
        {
            titleContent.text = "AssetBundle 配置编辑器";
            assetBundleCconfig = Config.ReadEditorDirectory<AssetBundleConfig>();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Save"))
            {
                assetBundleCconfig.Save<AssetBundleConfig>();
                AssetDatabase.Refresh();
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus")))
            {
                if (_bar == 0)
                {
                    var pack = new Pack();
                    pack.editorShow = true;
                    assetBundleCconfig.packs.Add(pack);
                }
                else if (_bar == 1)
                {
                    var source = new Source();
                    source.editorShow = true;
                    assetBundleCconfig.sources.Add(source);
                }
            }
            EditorGUILayout.EndHorizontal();

            _bar = GUILayout.Toolbar(_bar, _barStrs);

            EditorGUILayout.BeginVertical("box");
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            if (_bar == 0)
            {
                for (int i = 0; i < assetBundleCconfig.packs.Count; i++)
                {
                    var pack = assetBundleCconfig.packs[i];

                    EditorGUILayout.BeginVertical("box");

                    if (pack.editorShow)
                    {
                        // 
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("目标资源路径");
                        if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash")))
                        {
                            assetBundleCconfig.packs.RemoveAt(i);
                            break;
                        }
                        if (pack.editorShow && GUILayout.Button(EditorGUIUtility.IconContent("Profiler.NextFrame")))
                        {
                            pack.editorShow = !pack.editorShow;
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginHorizontal();
                        pack.target = EditorGUILayout.TextField(pack.target);

                        if (GUILayout.Button(EditorGUIUtility.IconContent("ViewToolZoom On")))
                        {
                            var tp = EditorUtility.OpenFolderPanel("选择目标路径", Application.dataPath, "");
                            if (!string.IsNullOrEmpty(tp))
                            {
                                if (tp.Length > Application.dataPath.Length)
                                {
                                    pack.target = "Assets" + tp.Substring(Application.dataPath.Length, tp.Length - Application.dataPath.Length);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        //
                        EditorGUILayout.LabelField("AssetBundle 输出路径");
                        pack.bundlePath = EditorGUILayout.TextField(pack.bundlePath);

                        if (pack.model == BuildModel.Folder)
                        {
                            EditorGUILayout.LabelField("AssetBundle 名称");
                            pack.bundleName = EditorGUILayout.TextField(pack.bundleName);
                        }

                        if (pack.model != BuildModel.Standard && pack.model != BuildModel.File)
                        {
                            EditorGUILayout.LabelField("资源匹配模式");
                            pack.pattern = EditorGUILayout.TextField(pack.pattern);
                        }
                        else pack.pattern = "*.*";

                        // 
                        EditorGUILayout.LabelField("打包模式");
                        pack.model = (BuildModel)EditorGUILayout.EnumPopup("", pack.model);

                        //
                        EditorGUILayout.LabelField("导出映射配置");
                        pack.genMapping = (GenerateMapping)EditorGUILayout.EnumPopup("", pack.genMapping);
                    }
                    else
                    {
                        // 
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(pack.target);
                        if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash")))
                        {
                            assetBundleCconfig.packs.RemoveAt(i);
                            break;
                        }
                        if (!pack.editorShow && GUILayout.Button(EditorGUIUtility.IconContent("editicon.sml")))
                        {
                            pack.editorShow = !pack.editorShow;
                        }
                        EditorGUILayout.EndHorizontal();

                    }

                    EditorGUILayout.EndVertical();

                    EditorGUILayout.Space();
                }
            }
            else if (_bar == 1)
            {
                for (int i = 0; i < assetBundleCconfig.sources.Count; i++)
                {
                    var source = assetBundleCconfig.sources[i];
                    EditorGUILayout.BeginVertical("box");
                    if (source.editorShow)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Source");
                        if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash")))
                        {
                            assetBundleCconfig.sources.RemoveAt(i);
                            break;
                        }
                        if (source.editorShow && GUILayout.Button(EditorGUIUtility.IconContent("Profiler.NextFrame")))
                        {
                            source.editorShow = !source.editorShow;
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();

                        EditorGUILayout.BeginHorizontal();
                        source.source = EditorGUILayout.TextField(source.source);

                        if (GUILayout.Button(EditorGUIUtility.IconContent("ViewToolZoom On")))
                        {
                            var tp = EditorUtility.OpenFolderPanel("Copy Source Path", Application.dataPath, "");
                            if (!string.IsNullOrEmpty(tp))
                            {
                                if (tp.Length > Application.dataPath.Length)
                                {
                                    source.source = "Assets" + tp.Substring(Application.dataPath.Length, tp.Length - Application.dataPath.Length);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        //
                        EditorGUILayout.LabelField("Dest");
                        source.dest = EditorGUILayout.TextField(source.dest);
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(source.source);
                        if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash")))
                        {
                            assetBundleCconfig.sources.RemoveAt(i);
                            break;
                        }
                        if (!source.editorShow && GUILayout.Button(EditorGUIUtility.IconContent("editicon.sml")))
                        {
                            source.editorShow = !source.editorShow;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}