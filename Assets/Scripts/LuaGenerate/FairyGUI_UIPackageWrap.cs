﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_UIPackageWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.UIPackage), typeof(System.Object));
		L.RegFunction("GetVar", GetVar);
		L.RegFunction("SetVar", SetVar);
		L.RegFunction("GetById", GetById);
		L.RegFunction("GetByName", GetByName);
		L.RegFunction("AddPackage", AddPackage);
		L.RegFunction("RemovePackage", RemovePackage);
		L.RegFunction("RemoveAllPackages", RemoveAllPackages);
		L.RegFunction("GetPackages", GetPackages);
		L.RegFunction("CreateObject", CreateObject);
		L.RegFunction("CreateObjectFromURL", CreateObjectFromURL);
		L.RegFunction("CreateObjectAsync", CreateObjectAsync);
		L.RegFunction("GetItemAsset", GetItemAsset);
		L.RegFunction("GetItemAssetByURL", GetItemAssetByURL);
		L.RegFunction("GetItemURL", GetItemURL);
		L.RegFunction("GetItemByURL", GetItemByURL);
		L.RegFunction("NormalizeURL", NormalizeURL);
		L.RegFunction("SetStringsSource", SetStringsSource);
		L.RegFunction("LoadAllAssets", LoadAllAssets);
		L.RegFunction("UnloadAssets", UnloadAssets);
		L.RegFunction("ReloadAssets", ReloadAssets);
		L.RegFunction("GetItems", GetItems);
		L.RegFunction("GetItem", GetItem);
		L.RegFunction("GetItemByName", GetItemByName);
		L.RegFunction("New", _CreateFairyGUI_UIPackage);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("unloadBundleByFGUI", get_unloadBundleByFGUI, set_unloadBundleByFGUI);
		L.RegVar("URL_PREFIX", get_URL_PREFIX, null);
		L.RegVar("id", get_id, null);
		L.RegVar("name", get_name, null);
		L.RegVar("branch", get_branch, set_branch);
		L.RegVar("assetPath", get_assetPath, null);
		L.RegVar("customId", get_customId, set_customId);
		L.RegVar("resBundle", get_resBundle, null);
		L.RegVar("dependencies", get_dependencies, null);
		L.RegFunction("LoadResource", FairyGUI_UIPackage_LoadResource);
		L.RegFunction("CreateObjectCallback", FairyGUI_UIPackage_CreateObjectCallback);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_UIPackage(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				FairyGUI.UIPackage obj = new FairyGUI.UIPackage();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.UIPackage.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetVar(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = FairyGUI.UIPackage.GetVar(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetVar(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			FairyGUI.UIPackage.SetVar(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetById(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			FairyGUI.UIPackage o = FairyGUI.UIPackage.GetById(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetByName(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			FairyGUI.UIPackage o = FairyGUI.UIPackage.GetByName(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddPackage(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1 && TypeChecker.CheckTypes<UnityEngine.AssetBundle>(L, 1))
			{
				UnityEngine.AssetBundle arg0 = (UnityEngine.AssetBundle)ToLua.ToObject(L, 1);
				FairyGUI.UIPackage o = FairyGUI.UIPackage.AddPackage(arg0);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 1 && TypeChecker.CheckTypes<string>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				FairyGUI.UIPackage o = FairyGUI.UIPackage.AddPackage(arg0);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<UnityEngine.AssetBundle, UnityEngine.AssetBundle>(L, 1))
			{
				UnityEngine.AssetBundle arg0 = (UnityEngine.AssetBundle)ToLua.ToObject(L, 1);
				UnityEngine.AssetBundle arg1 = (UnityEngine.AssetBundle)ToLua.ToObject(L, 2);
				FairyGUI.UIPackage o = FairyGUI.UIPackage.AddPackage(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<string, FairyGUI.UIPackage.LoadResource>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				FairyGUI.UIPackage.LoadResource arg1 = (FairyGUI.UIPackage.LoadResource)ToLua.ToObject(L, 2);
				FairyGUI.UIPackage o = FairyGUI.UIPackage.AddPackage(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes<UnityEngine.AssetBundle, UnityEngine.AssetBundle, string>(L, 1))
			{
				UnityEngine.AssetBundle arg0 = (UnityEngine.AssetBundle)ToLua.ToObject(L, 1);
				UnityEngine.AssetBundle arg1 = (UnityEngine.AssetBundle)ToLua.ToObject(L, 2);
				string arg2 = ToLua.ToString(L, 3);
				FairyGUI.UIPackage o = FairyGUI.UIPackage.AddPackage(arg0, arg1, arg2);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes<byte[], string, FairyGUI.UIPackage.LoadResource>(L, 1))
			{
				byte[] arg0 = ToLua.CheckByteBuffer(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				FairyGUI.UIPackage.LoadResource arg2 = (FairyGUI.UIPackage.LoadResource)ToLua.ToObject(L, 3);
				FairyGUI.UIPackage o = FairyGUI.UIPackage.AddPackage(arg0, arg1, arg2);
				ToLua.PushObject(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.UIPackage.AddPackage");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemovePackage(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			FairyGUI.UIPackage.RemovePackage(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveAllPackages(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			FairyGUI.UIPackage.RemoveAllPackages();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPackages(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			System.Collections.Generic.List<FairyGUI.UIPackage> o = FairyGUI.UIPackage.GetPackages();
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreateObject(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<string, string>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				FairyGUI.GObject o = FairyGUI.UIPackage.CreateObject(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<FairyGUI.UIPackage, string>(L, 1))
			{
				FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				FairyGUI.GObject o = obj.CreateObject(arg0);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes<string, string, System.Type>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				System.Type arg2 = (System.Type)ToLua.ToObject(L, 3);
				FairyGUI.GObject o = FairyGUI.UIPackage.CreateObject(arg0, arg1, arg2);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes<FairyGUI.UIPackage, string, System.Type>(L, 1))
			{
				FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				System.Type arg1 = (System.Type)ToLua.ToObject(L, 3);
				FairyGUI.GObject o = obj.CreateObject(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.UIPackage.CreateObject");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreateObjectFromURL(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				string arg0 = ToLua.CheckString(L, 1);
				FairyGUI.GObject o = FairyGUI.UIPackage.CreateObjectFromURL(arg0);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<System.Type>(L, 2))
			{
				string arg0 = ToLua.CheckString(L, 1);
				System.Type arg1 = (System.Type)ToLua.ToObject(L, 2);
				FairyGUI.GObject o = FairyGUI.UIPackage.CreateObjectFromURL(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<FairyGUI.UIPackage.CreateObjectCallback>(L, 2))
			{
				string arg0 = ToLua.CheckString(L, 1);
				FairyGUI.UIPackage.CreateObjectCallback arg1 = (FairyGUI.UIPackage.CreateObjectCallback)ToLua.ToObject(L, 2);
				FairyGUI.UIPackage.CreateObjectFromURL(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.UIPackage.CreateObjectFromURL");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CreateObjectAsync(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 3 && TypeChecker.CheckTypes<string, string, FairyGUI.UIPackage.CreateObjectCallback>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				FairyGUI.UIPackage.CreateObjectCallback arg2 = (FairyGUI.UIPackage.CreateObjectCallback)ToLua.ToObject(L, 3);
				FairyGUI.UIPackage.CreateObjectAsync(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<FairyGUI.UIPackage, string, FairyGUI.UIPackage.CreateObjectCallback>(L, 1))
			{
				FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				FairyGUI.UIPackage.CreateObjectCallback arg1 = (FairyGUI.UIPackage.CreateObjectCallback)ToLua.ToObject(L, 3);
				obj.CreateObjectAsync(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.UIPackage.CreateObjectAsync");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetItemAsset(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes<string, string>(L, 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				object o = FairyGUI.UIPackage.GetItemAsset(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<FairyGUI.UIPackage, string>(L, 1))
			{
				FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				object o = obj.GetItemAsset(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes<FairyGUI.UIPackage, FairyGUI.PackageItem>(L, 1))
			{
				FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.ToObject(L, 1);
				FairyGUI.PackageItem arg0 = (FairyGUI.PackageItem)ToLua.ToObject(L, 2);
				object o = obj.GetItemAsset(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.UIPackage.GetItemAsset");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetItemAssetByURL(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			object o = FairyGUI.UIPackage.GetItemAssetByURL(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetItemURL(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			string o = FairyGUI.UIPackage.GetItemURL(arg0, arg1);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetItemByURL(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			FairyGUI.PackageItem o = FairyGUI.UIPackage.GetItemByURL(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int NormalizeURL(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = FairyGUI.UIPackage.NormalizeURL(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetStringsSource(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.Utils.XML arg0 = (FairyGUI.Utils.XML)ToLua.CheckObject<FairyGUI.Utils.XML>(L, 1);
			FairyGUI.UIPackage.SetStringsSource(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAllAssets(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.CheckObject<FairyGUI.UIPackage>(L, 1);
			obj.LoadAllAssets();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadAssets(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.CheckObject<FairyGUI.UIPackage>(L, 1);
			obj.UnloadAssets();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReloadAssets(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.CheckObject<FairyGUI.UIPackage>(L, 1);
				obj.ReloadAssets();
				return 0;
			}
			else if (count == 2)
			{
				FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.CheckObject<FairyGUI.UIPackage>(L, 1);
				UnityEngine.AssetBundle arg0 = (UnityEngine.AssetBundle)ToLua.CheckObject<UnityEngine.AssetBundle>(L, 2);
				obj.ReloadAssets(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.UIPackage.ReloadAssets");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetItems(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.CheckObject<FairyGUI.UIPackage>(L, 1);
			System.Collections.Generic.List<FairyGUI.PackageItem> o = obj.GetItems();
			ToLua.PushSealed(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetItem(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.CheckObject<FairyGUI.UIPackage>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			FairyGUI.PackageItem o = obj.GetItem(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetItemByName(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)ToLua.CheckObject<FairyGUI.UIPackage>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			FairyGUI.PackageItem o = obj.GetItemByName(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_unloadBundleByFGUI(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushboolean(L, FairyGUI.UIPackage.unloadBundleByFGUI);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_URL_PREFIX(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, FairyGUI.UIPackage.URL_PREFIX);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_id(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)o;
			string ret = obj.id;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index id on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_name(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)o;
			string ret = obj.name;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index name on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_branch(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, FairyGUI.UIPackage.branch);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_assetPath(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)o;
			string ret = obj.assetPath;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index assetPath on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_customId(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)o;
			string ret = obj.customId;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index customId on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_resBundle(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)o;
			UnityEngine.AssetBundle ret = obj.resBundle;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index resBundle on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_dependencies(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)o;
			System.Collections.Generic.Dictionary<string,string>[] ret = obj.dependencies;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index dependencies on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_unloadBundleByFGUI(IntPtr L)
	{
		try
		{
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			FairyGUI.UIPackage.unloadBundleByFGUI = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_branch(IntPtr L)
	{
		try
		{
			string arg0 = ToLua.CheckString(L, 2);
			FairyGUI.UIPackage.branch = arg0;
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_customId(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.UIPackage obj = (FairyGUI.UIPackage)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.customId = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index customId on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FairyGUI_UIPackage_LoadResource(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			LuaFunction func = ToLua.CheckLuaFunction(L, 1);

			if (count == 1)
			{
				Delegate arg1 = DelegateTraits<FairyGUI.UIPackage.LoadResource>.Create(func);
				ToLua.Push(L, arg1);
			}
			else
			{
				LuaTable self = ToLua.CheckLuaTable(L, 2);
				Delegate arg1 = DelegateTraits<FairyGUI.UIPackage.LoadResource>.Create(func, self);
				ToLua.Push(L, arg1);
			}
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FairyGUI_UIPackage_CreateObjectCallback(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			LuaFunction func = ToLua.CheckLuaFunction(L, 1);

			if (count == 1)
			{
				Delegate arg1 = DelegateTraits<FairyGUI.UIPackage.CreateObjectCallback>.Create(func);
				ToLua.Push(L, arg1);
			}
			else
			{
				LuaTable self = ToLua.CheckLuaTable(L, 2);
				Delegate arg1 = DelegateTraits<FairyGUI.UIPackage.CreateObjectCallback>.Create(func, self);
				ToLua.Push(L, arg1);
			}
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}
