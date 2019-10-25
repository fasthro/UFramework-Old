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

namespace FastEngine.Editor
{
    public class LuaEditor
    {
        [MenuItem("FastEngine/tolua 生成 Wrap")]
        static void RegenerateWrap()
        {
            ToLuaMenu.ClearLuaWraps();
            ToLuaMenu.GenLuaAll();
        }
    }
}
