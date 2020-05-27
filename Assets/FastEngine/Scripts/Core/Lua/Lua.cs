/*
 * @Author: fasthro
 * @Date: 2019-10-25 12:02:35
 * @Description: Lua
 */
using System.Collections;
using System.Collections.Generic;
using FastEngine.Debuger;
using LuaInterface;
using UnityEngine;

namespace FastEngine.Core
{
    [MonoSingletonPath("FastEngine/Lua")]
    public class Lua : MonoSingleton<Lua>
    {
        public LuaState luaState { get; private set; }
        public LuaLooper luaLoop { get; private set; }
        public LuaLoader luaLoader { get; private set; }

        private void InternalInitialize()
        {
            luaState = new LuaState();
            // open lib
            luaState.OpenLibs(LuaDLL.luaopen_socket_core); 
            luaState.OpenLibs(LuaDLL.luaopen_pb);
            // lua set top
            luaState.LuaSetTop(0);
            
            // wrap
            LuaBinder.Bind(luaState);
            // delegate
            DelegateFactory.Init();
            // coroutine
            LuaCoroutine.Register(luaState, this);
            // lua loader
            luaLoader = new LuaLoader();
            luaLoader.Initialize();
           
            // start
            luaState.Start();

            #region add lua file
            luaState.DoFile("fastengine");
            luaState.DoFile("luascript");
            #endregion

            // lua loop
            luaLoop = gameObject.AddComponent<LuaLooper>();
            luaLoop.luaState = luaState;

            

            // 启动 lua 脚本
            Lua.Call("LuaStart");
        }

        private void InternalGC() { luaState.LuaGC(LuaGCOptions.LUA_GCCOLLECT); }

        private void InternalClose()
        {
            Lua.Call("LuaQuit");

            luaLoop.Destroy();
            luaLoop = null;

            luaState.Dispose();
            luaState = null;
        }

        #region API
        public static void Initialize() { Instance.InternalInitialize(); }
        public static void GC() { Instance.InternalGC(); }
        public static void Close() { Instance.InternalClose(); }

        public static int GetTableIntValue(string tableName, string fieldName)
        {
            var table = Instance.luaState.GetTable(tableName);
            try
            {
                return int.Parse(table.GetStringField(fieldName));
            }
            catch
            {
                Debug.LogError("lua table value to int error! tableName: " + tableName + " fieldName: " + fieldName);
            }
            return 0;
        }

        public static string GetTableStringValue(string tableName, string fieldName)
        {
            var table = Instance.luaState.GetTable(tableName);
            try
            {
                return table.GetStringField(fieldName);
            }
            catch
            {
                Debug.LogError("lua table value to string error! tableName: " + tableName + " fieldName: " + fieldName);
            }
            return "";
        }

        public static void Call(string funcName)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) func.Call();
        }

        public static void Call<T1>(string funcName, T1 arg1)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) func.Call(arg1);
        }

        public static void Call<T1, T2>(string funcName, T1 arg1, T2 arg2)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2);
        }

        public static void Call<T1, T2, T3>(string funcName, T1 arg1, T2 arg2, T3 arg3)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3);
        }

        public static void Call<T1, T2, T3, T4>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4);
        }

        public static void Call<T1, T2, T3, T4, T5>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5);
        }

        public static void Call<T1, T2, T3, T4, T5, T6>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static void Call<T1, T2, T3, T4, T5, T6, T7>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static void Call<T1, T2, T3, T4, T5, T6, T7, T8>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) func.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static R1 Invoke<R1>(string funcName)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) return func.Invoke<R1>();
            return default(R1);
        }

        public static R1 Invoke<T1, R1>(string funcName, T1 arg1)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, R1>(arg1);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, R1>(string funcName, T1 arg1, T2 arg2)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, R1>(arg1, arg2);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, R1>(arg1, arg2, arg3);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, R1>(arg1, arg2, arg3, arg4);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, R1>(arg1, arg2, arg3, arg4, arg5);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, R1>(arg1, arg2, arg3, arg4, arg5, arg6);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, T7, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, T7, R1>(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, T7, T8, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, T7, T8, R1>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return default(R1);
        }

        public static R1 Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, R1>(string funcName, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            LuaFunction func = Instance.luaState.GetFunction(funcName);
            if (func != null) return func.Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, R1>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return default(R1);
        }
        #endregion
    }
}

