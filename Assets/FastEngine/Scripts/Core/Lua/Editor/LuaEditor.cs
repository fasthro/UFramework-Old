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

        [MenuItem("FastEngine/Lua/Generate Wrap")]
        static void GenerateWrap()
        {
            
        }

        [MenuItem("FastEngine/Lua/Clear Wrap")]
        static void ClearWrap()
        {
            
        }

        /// <summary>
        /// 生成LuaBinder文件
        /// </summary>
        static void CreateLuaBinder()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using LuaInterface;");
            sb.AppendLine();
            sb.AppendLine("public static class LuaBinder");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static void Bind(LuaState L)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tthrow new LuaException(\"Please generate LuaBinder files first!\");");
            sb.AppendLine("\t}");
            sb.AppendLine("}");

            var fp = LuaConfig.luaGenerateDirectory + "/LuaBinder.cs";
            var data = Encoding.GetEncoding("UTF-8").GetBytes(sb.ToString());
            try
            {
                if (File.Exists(fp))
                    File.Delete(fp);

                File.WriteAllBytes(fp, data);
            }
            catch (Exception e)
            {
                Debug.LogError("create LuaBinder.cs error! \n" + e.Message);
            }
        }
    }
}
