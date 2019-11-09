
/*
生成bind to lua 的类的 api提示文件 
注释按照babelua的格式来做的
其他ide格式可以添加
tom
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using FastEngine.Core;

namespace FastEngine.Editor.Lua
{
    public class LuaApi
    {
        static StringBuilder sb = null;
        static BindingFlags binding = BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase;
        static Dictionary<string, XmlNode> xmlNodeCache = null;
        static List<string> loadedModule = null;

        public static void Generate()
        {
            Start();
            foreach (var luaB in CustomSettings.customTypeList)
            {
                GenType(luaB.type);
            }
            StopAndSave();
        }
        //总开始
        public static void Start()
        {
            sb = new StringBuilder();
            xmlNodeCache = new Dictionary<string, XmlNode>();
            loadedModule = new List<string>();
        }
        //总结束
        public static void StopAndSave()
        {
            System.IO.File.WriteAllText(LuaConfig.luaApiGenerateDirectory + "UnityApi.lua", sb.ToString());
            xmlNodeCache = null;
            sb = null; loadedModule = null;
        }

        private static void GenType(Type type)
        {
            if (type.IsEnum)
            {
                GenEnum(type);
            }
            else
            {
                GenClass(type);
            }
        }

        private static void GenEnum(System.Type type)
        {
            sb.Append(type.Name + " = {} \r\n\r\n");

            FieldInfo[] fields = type.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                object[] comment = field.GetCustomAttributes(true);
                sb.Append(type.Name + "." + field.Name + " = nil;\r\n\r\n");
            }
        }

        private static void RemovePropertyFunction(MethodInfo[] methods, PropertyInfo[] ps, out MethodInfo[] outMethods)
        {
            List<MethodInfo> list = methods.ToList();

            for (int i = 0; i < ps.Length; i++)
            {
                int index = list.FindIndex((m) => { return m.Name == "get_" + ps[i].Name; });

                if (index >= 0 && list[index].Name != "get_Item")
                {
                    list.RemoveAt(index);
                }

                index = list.FindIndex((m) => { return m.Name == "set_" + ps[i].Name; });

                if (index >= 0 && list[index].Name != "set_Item")
                {
                    list.RemoveAt(index);
                }
            }
            outMethods = list.ToArray();
        }

        private static void GenClass(Type type)
        {
            sb.Append(type.Name + " = {} \r\n--*\r\n");
            Dictionary<string, ArrayList> nameToMethods = new Dictionary<string, ArrayList>();

            //构造函数
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Length > 0)
            {
                sb.Append("--[Comment]\r\n--consturctor for " + type.Name + " overrides:\r\n--*\r\n");
                for (int i = 0; i < constructors.Length; i++)
                {
                    ConstructorInfo con = constructors[i];
                    if (IsObsolete(con))
                        continue;
                    sb.Append("--" + type.Name + ".New(");
                    ParameterInfo[] args = con.GetParameters();
                    for (int j = 0; j < args.Length; j++)
                    {
                        sb.Append(args[j].ParameterType.Name + " " + args[j].Name);
                        if (j < args.Length - 1)
                            sb.Append(",");
                    }
                    sb.Append(")\r\n");
                    string methodComment = GetMethodComment(con);
                    if (methodComment != null && methodComment.Length > 0)
                        sb.Append("--" + methodComment + "\r\n");
                    sb.Append("--*\r\n\r\n");

                }
                sb.Append("function " + type.Name + ".New() end\r\n--*\r\n");
            }

            //propertys
            PropertyInfo[] ps = type.GetProperties();
            for (int i = 0; i < ps.Length; i++)
            {
                PropertyInfo p = ps[i];
                if (IsObsolete(p))
                    continue;
                sb.Append("--[Comment]\r\n-- property: " + p.PropertyType.Name + " " + type.Name + "." + p.Name + "\t");
                if (p.CanRead)
                    sb.Append("get\t");
                if (p.CanWrite)
                    sb.Append("set\t");
                sb.Append("\r\n");

                string propertyComment = GetpropertyComment(p);
                if (propertyComment != null && propertyComment.Length > 0)
                    sb.Append("--" + propertyComment + "\r\n");

                sb.Append(type.Name + "." + p.Name + " = nil \r\n--*\r\n");
            }

            //静态函数
            binding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly;
            MethodInfo[] methods = type.GetMethods(binding);
            RemovePropertyFunction(methods, ps, out methods);
            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo method = methods[i];
                if (IsObsolete(method))
                    continue;
                if (method.IsGenericMethod)
                    continue;
                ArrayList thisNameMethods = null;
                if (nameToMethods.TryGetValue(method.Name, out thisNameMethods))
                {//这个函数有了
                    thisNameMethods.Add(method);
                }
                else
                {
                    ArrayList newArray = new ArrayList();
                    newArray.Add(method);
                    nameToMethods.Add(method.Name, newArray);
                }
            }

            foreach (var item in nameToMethods)
            {
                sb.Append("--[Comment]\r\n--overrides:\r\n--*\r\n");

                ArrayList thisNameMethods = item.Value;
                bool isStatic = false;

                foreach (MethodInfo method in thisNameMethods)
                {
                    if (method.IsStatic)
                        isStatic = true;

                    string returnType = method.ReturnType.Name;
                    ParameterInfo[] args = method.GetParameters();
                    sb.Append("--" + returnType + " " + item.Key + "(");

                    for (int i = 0; i < args.Length; i++)
                    {
                        ParameterInfo arg = args[i];
                        sb.Append(arg.ParameterType.Name + " " + arg.Name);
                        if (i < args.Length - 1)
                            sb.Append(",");
                    }

                    sb.Append(")\r\n");
                    string methodComment = GetMethodComment(method);
                    if (methodComment != null && methodComment.Length > 0)
                        sb.Append("--" + methodComment + "\r\n");

                    sb.Append("--*\r\n");
                }
                if (isStatic)
                {
                    sb.Append("--static method,use '.'\r\nfunction " + type.Name + "." + item.Key + "() end \r\n\r\n");
                }
                else
                {
                    sb.Append("--no static method,use ':'\r\nfunction " + type.Name + ":" + item.Key + "() end \r\n\r\n");
                }
            }
        }

        private static string GetpropertyComment(PropertyInfo p)
        {
            string fullMethodNameInXml = "P:";
            fullMethodNameInXml += p.ReflectedType.FullName;
            fullMethodNameInXml += "." + p.Name;
            return GetCommentFromXml(p.Module, fullMethodNameInXml);
        }

        private static string GetMethodComment(MethodBase method)
        {
            string fullMethodNameInXml = "M:";
            fullMethodNameInXml += method.ReflectedType.FullName;
            if (method.IsConstructor)
                fullMethodNameInXml += ".#ctor";
            else
                fullMethodNameInXml += "." + method.Name;

            ParameterInfo[] args = method.GetParameters();
            if (args.Length > 0)
            {
                fullMethodNameInXml += "(";
                for (int i = 0; i < args.Length; i++)
                {
                    ParameterInfo arg = args[i];
                    fullMethodNameInXml += arg.ParameterType.FullName;
                    if (i < args.Length - 1)
                    {
                        fullMethodNameInXml += ",";
                    }
                }
                fullMethodNameInXml += ")";
            }
            return GetCommentFromXml(method.Module, fullMethodNameInXml);
        }

        private static string GetCommentFromXml(Module module, string fullMethodNameInXml)
        {
            string moduleName = module.Name;
            XmlNode xmlNode = null;

            bool moduleLoaded = false;
            foreach (var str in loadedModule)
            {
                if (str == moduleName)
                {
                    moduleLoaded = true;
                    break;
                }
            }

            if (!moduleLoaded)
            {
                loadedModule.Add(moduleName);
                string dllPath = module.FullyQualifiedName;
                string xmlPath = System.IO.Path.ChangeExtension(dllPath, "xml");
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(xmlPath);
                    XmlNode root = doc.SelectSingleNode("doc");
                    root = root["members"];

                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        XmlNode node = root.ChildNodes[i];
                        if (node.Name == "member" && node.Attributes["name"] != null)
                        {
                            XmlNode tmp = null;
                            if (!xmlNodeCache.TryGetValue(node.Attributes["name"].Value, out tmp))
                                xmlNodeCache.Add(node.Attributes["name"].Value, node);
                        }
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            if (!xmlNodeCache.TryGetValue(fullMethodNameInXml, out xmlNode))
            {
                return null;
            }

            if (xmlNode == null)
                return null;

            //开始找咯

            //ok 找到这个node了 就要拿子节点了
            string ret = "";
            if (xmlNode["summary"] != null)
                if (xmlNode["summary"]["para"] != null)
                {
                    ret = xmlNode["summary"]["para"].InnerText;
                    ret = ret.Replace('\r', ' ');
                    ret = ret.Replace('\n', ' ');
                }

            //看看有没有参数信息 有就也列出来
            bool paramInfoPrinted = false;

            for (int j = 0; j < xmlNode.ChildNodes.Count; j++)
            {
                if (xmlNode.ChildNodes.Item(j).Name == "param")
                {
                    if (xmlNode.ChildNodes.Item(j).Attributes["name"] != null)
                    {
                        string tmp = xmlNode.ChildNodes.Item(j).InnerText;
                        if (tmp.Length > 0)
                        {
                            if (!paramInfoPrinted)
                            {
                                ret += "\r\n--params:\r\n";
                                paramInfoPrinted = true;
                            }
                            tmp = tmp.Replace('\r', ' ');
                            tmp = tmp.Replace('\n', ' ');
                            ret += "--" + xmlNode.ChildNodes.Item(j).Attributes["name"].Value + ":    " + tmp + "\r\n";
                        }
                    }
                }
            }

            ret = ret.Replace("[[", "{{");
            ret = ret.Replace("]]", "}}");

            return ret;
        }

        private static bool IsObsolete(MemberInfo mb)
        {
            object[] attrs = mb.GetCustomAttributes(true);

            for (int j = 0; j < attrs.Length; j++)
            {
                Type t = attrs[j].GetType();

                if (t == typeof(System.ObsoleteAttribute))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

