/*
 * @Author: fasthro
 * @Date: 2019-10-25 17:48:41
 * @Description: Lua Editor
 */
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using FastEngine.Core;
using System.IO;
using System;
using UnityEditor;

namespace FastEngine.Editor.Lua
{
    public class LuaEditor
    {
        [MenuItem("FastEngine/Lua -> 生成 Wrap", false, 1)]
        static void WrapGenerate()
        {
            ToLuaMenu.ClearLuaWraps();
        }

        [MenuItem("FastEngine/Lua -> 生成 API", false, 2)]
        static void LuaApiGenerate()
        {
            LuaApi.Generate();
        }

        [MenuItem("FastEngine/Lua -> 打包", false, 3)]
        static void BuildBundle()
        {
            LuaBuildBundle.Build();
        }
    }
}
