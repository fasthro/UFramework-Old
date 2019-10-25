/*
 * @Author: fasthro
 * @Date: 2019-10-25 16:26:54
 * @Description: Lua 配置
 */
using UnityEngine;

namespace FastEngine.Core
{
    public static class LuaConfig
    {
        /// <summary>
        /// lua 文件目录(编辑器模式)
        /// </summary>
        /// <value></value>
        public static string[] luaDirectorys = new string[]
        {
            Application.dataPath + "/FastEngine/3rd/Lua/ToLua/Lua",      // tolua
            Application.dataPath + "/FastEngine/Scripts/Core/Lua/Libs",  // engine lua libs
            Application.dataPath + "/LuaScripts",                        // project lua script
        };

        /// <summary>
        /// lua wap 导出目录(包含tolua base type)
        /// </summary>
        public static string luaGenerateDirectory = Application.dataPath + "/Scripts/LuaGenerate/";
    }
}