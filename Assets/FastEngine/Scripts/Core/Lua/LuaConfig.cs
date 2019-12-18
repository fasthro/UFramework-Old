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
        // lua 字节码模式
        public static bool luaByteMode = false;

        /// <summary>
        /// lua 文件目录(编辑器模式)
        /// </summary>
        /// <value></value>
        public static string[] luaDirectorys = new string[]
        {
            // FIXME 未来优化到引擎库中
            Application.dataPath + "/FastEngine/3rd/Lua/ToLua/Lua",      // tolua
            Application.dataPath + "/FastEngine/Scripts/Core/Lua/Libs",  // engine lua libs
            Application.dataPath + "/LuaScripts",                        // project lua script
        };

        /// <summary>
        /// lua wrap 导出目录
        /// </summary>
        public static string luaGenerateDirectory = Application.dataPath + "/Scripts/LuaGenerate/";

        /// <summary>
        /// lua warp base type 导出目录
        /// </summary>
        public static string luaGenerateBaseDirectory = Application.dataPath + "/FastEngine/3rd/Lua/ToLua/BaseType/";

        /// <summary>
        /// lua api 导出目录
        /// </summary>
        public static string luaApiGenerateDirectory = Application.dataPath + "/LuaScripts/";
        
    }
}