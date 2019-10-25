using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEditor;

using BindType = ToLuaMenu.BindType;
using UnityEngine.UI;
using System.Reflection;
using FastEngine.Core;

public static class CustomSettings
{
    public static string saveDir = LuaConfig.luaGenerateDirectory;
    public static string toluaBaseType = LuaConfig.luaGenerateBaseDirectory;
    public static string injectionFilesPath = Application.dataPath + "/FastEngine/3rd/Lua/ToLua/Injection/";

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    public static List<Type> staticClassTypes = new List<Type>
    {
        // typeof(UnityEngine.Application),
        // typeof(UnityEngine.Time),
        // typeof(UnityEngine.Screen),
        // typeof(UnityEngine.SleepTimeout),
        // typeof(UnityEngine.Input),
        // typeof(UnityEngine.Resources),
        // typeof(UnityEngine.Physics),
        // typeof(UnityEngine.RenderSettings),
        // typeof(UnityEngine.QualitySettings),
        // typeof(UnityEngine.GL),
        // typeof(UnityEngine.Graphics),
    };

    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static DelegateType[] customDelegateList =
    {        
        // _DT(typeof(Action)),                
        // _DT(typeof(UnityEngine.Events.UnityAction)),
        // _DT(typeof(System.Predicate<int>)),
        // _DT(typeof(System.Action<int>)),
        // _DT(typeof(System.Comparison<int>)),
        // _DT(typeof(System.Func<int, int>)),
    };

    // 在这里添加你要导出注册到lua的类型列表
    public static BindType[] customTypeList =
    {
        _GT(typeof(Debugger)).SetNameSpace(null),          

        #region base
               
       _GT(typeof(UnityEngine.Object)),
        _GT(typeof(GameObject)),
        _GT(typeof(Transform)),
        _GT(typeof(Component)),
        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),
        _GT(typeof(Time)),
        _GT(typeof(AssetBundle)),

        _GT(typeof(Vector3)),
        _GT(typeof(Vector4)),
        _GT(typeof(Vector2)),
        _GT(typeof(Color)),

        #endregion
    };

    public static List<Type> dynamicList = new List<Type>() { };

    //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
    //使用方法参见例子14
    public static List<Type> outList = new List<Type>() { };

    //ngui优化，下面的类没有派生类，可以作为sealed class
    public static List<Type> sealedList = new List<Type>() { };
    public static BindType _GT(Type t) { return new BindType(t); }
    public static DelegateType _DT(Type t) { return new DelegateType(t); }
}
