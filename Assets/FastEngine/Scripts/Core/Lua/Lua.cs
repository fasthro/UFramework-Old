/*
 * @Author: fasthro
 * @Date: 2019-10-25 12:02:35
 * @Description: Lua
 */
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

namespace FastEngine.Core
{
    [MonoSingletonPath("FastEngine/Lua")]
    public class Lua : MonoSingleton<Lua>
    {
        public LuaState luaState { get; private set; }
        public LuaLooper luaLoop { get; private set; }

        private void InternalInitialize()
        {
            luaState = new LuaState();

            #region libs

            luaState.OpenLibs(LuaDLL.luaopen_pb);

            #endregion

            luaState.LuaSetTop(0);

            #region other

            // wrap
            LuaBinder.Bind(luaState);
            // delegate
            DelegateFactory.Init();
            // coroutine
            LuaCoroutine.Register(luaState, this);

            #endregion

            luaState.Start();

            

            // lua loop
            luaLoop = gameObject.AddComponent<LuaLooper>();
            luaLoop.luaState = luaState;
        }

        public object[] CallFunction(string funcName, params object[] args)
        {
            LuaFunction func = luaState.GetFunction(funcName);
            if (func != null)
            {
                return func.Invoke<object[], object[]>(args);
            }
            return null;
        }

        private void InternalGC() { luaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT); }

        private void InternalClose()
        {
            luaLoop.Destroy();
            luaLoop = null;

            luaState.Dispose();
            luaState = null;
        }

        #region API
        public static void Initialize() { Instance.InternalInitialize(); }
        public static void GC() { Instance.InternalGC(); }
        public static void Close() { Instance.InternalClose(); }
        #endregion
    }
}

