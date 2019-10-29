using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using FastEngine.Utils;

namespace FastEngine.Editor
{
    public class SetupEngine
    {
        [MenuItem("FastEngine/Setup Engine")]
        static void Setup()
        {
            // 构建目录
            // res
            FilePath.CreateDirectory(FilePath.Combine(Application.dataPath, "Art"));
            // ui
            FilePath.CreateDirectory(FilePath.Combine(Application.dataPath, "Art", "UI"));

            // script
            FilePath.CreateDirectory(FilePath.Combine(Application.dataPath, "Scripts"));
            FilePath.CreateDirectory(FilePath.Combine(Application.dataPath, "Scripts", "LuaGenerate"));
            FilePath.CreateDirectory(FilePath.Combine(Application.dataPath, "Scripts", "Proto"));
            // lua
            FilePath.CreateDirectory(FilePath.Combine(Application.dataPath, "LuaScripts"));
            FilePath.CreateDirectory(FilePath.Combine(Application.dataPath, "LuaScripts", "proto"));

            Debug.Log("Welcome to fastengine");
        }
    }
}
