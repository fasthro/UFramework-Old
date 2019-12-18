/*
 * @Author: fasthro
 * @Date: 2019-11-09 15:17:31
 * @Description: Table 配置编辑器
 */
using System.Collections.Generic;
using System.IO;
using FastEngine.Core;
using FastEngine.Core.Excel2Table;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Excel2Table
{
    public class TableEditorWindow : FastEditorWindow
    {
        private TableConfig m_table;
        private Vector3 m_scrollPosition;

        protected override void OnInitialize()
        {
            titleContent.text = "Table 配置编辑器";
            m_table = AppUtils.ReadEditorConfig<TableConfig>();

            if (m_table.tableDictionary == null)
                m_table.tableDictionary = new Dictionary<string, TableItem>();

            if (Directory.Exists(AppUtils.TableExcelDirectory()))
            {
                var files = Directory.GetFiles(AppUtils.TableExcelDirectory(), "*.xlsx", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = FilePathUtils.GetFileName(files[i], false);
                    if (!m_table.tableDictionary.ContainsKey(fileName))
                    {
                        var item = new TableItem();
                        item.tableName = fileName;
                        item.dataFormatOptions = DataFormatOptions.Array;
                        m_table.tableDictionary.Add(fileName, item);
                    }
                }
            }
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Save"))
            {
                AppUtils.WriteEditorConfig<TableConfig>(JsonMapper.ToJson(m_table));
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Generate"))
            {
                AppUtils.WriteEditorConfig<TableConfig>(JsonMapper.ToJson(m_table));
                AssetDatabase.Refresh();

                TableEditor.Generate();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Out Format");
            m_table.outFormatOptions = (FormatOptions)EditorGUILayout.EnumPopup("", m_table.outFormatOptions);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            GUILayout.Label("Table Object Namespace");
            m_table.tableModelNamespace = EditorGUILayout.TextField(m_table.tableModelNamespace);
            if (string.IsNullOrEmpty(m_table.tableModelNamespace))
            {
                m_table.tableModelNamespace = "Table";
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();


            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
            EditorGUILayout.BeginVertical("box");

            m_table.tableDictionary.ForEach((item) =>
            {
                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Table: " + item.Value.tableName);

                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Access To Data");
                item.Value.dataFormatOptions = (DataFormatOptions)EditorGUILayout.EnumPopup("", item.Value.dataFormatOptions);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();
            });

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}