using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor
{
    public class SetupEngine
    {
        [MenuItem("FastEngine/Setup Engine")]
        static void Setup()
        {
            // 构建目录
            // res
            FilePathUtils.DirectoryCreate(FilePathUtils.Combine(Application.dataPath, "Art"));
            // ui
            FilePathUtils.DirectoryCreate(FilePathUtils.Combine(Application.dataPath, "Art", "UI"));

            // script
            FilePathUtils.DirectoryCreate(FilePathUtils.Combine(Application.dataPath, "Scripts"));
            FilePathUtils.DirectoryCreate(FilePathUtils.Combine(Application.dataPath, "Scripts", "LuaGenerate"));
            FilePathUtils.DirectoryCreate(FilePathUtils.Combine(Application.dataPath, "Scripts", "Proto"));
            // lua
            FilePathUtils.DirectoryCreate(FilePathUtils.Combine(Application.dataPath, "LuaScripts"));
            FilePathUtils.DirectoryCreate(FilePathUtils.Combine(Application.dataPath, "LuaScripts", "proto"));

            Debug.Log("Welcome to fastengine");
        }
    }
}
