/*
 * @Author: fasthro
 * @Date: 2019-11-19 16:00:03
 * @Description: 网络编辑器
 */
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FastEngine.Utils;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Network
{
    public class NetworkEditor
    {
        [MenuItem("FastEngine/Network -> Proto Generate", false, 300)]
        static void ProtoGenerate()
        {
            ProcessHelper.Run(FilePathUtils.Combine(FilePathUtils.GetTopDirectory(Application.dataPath, 1), "Tools/Protobuf/protoc-gen-all.bat"));
            AssetDatabase.Refresh();
        }
    }
}