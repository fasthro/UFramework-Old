/*
 * @Author: fasthro
 * @Date: 2019-12-19 15:01:37
 * @Description: 字段类型
 */
using System;
using UnityEngine;

namespace FastEngine.Core.Excel2Table
{
    public static class TypeUtils
    {
        /// <summary>
        /// type string to field type
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static FieldType TypeContentToFieldType(string ts)
        {
            if (FieldType.Byte.ToString().Equals(ts)) return FieldType.Byte;
            else if (FieldType.Int.ToString().Equals(ts)) return FieldType.Int;
            else if (FieldType.Long.ToString().Equals(ts)) return FieldType.Long;
            else if (FieldType.Float.ToString().Equals(ts)) return FieldType.Float;
            else if (FieldType.Double.ToString().Equals(ts)) return FieldType.Double;
            else if (FieldType.String.ToString().Equals(ts)) return FieldType.String;
            else if (FieldType.Boolean.ToString().Equals(ts)) return FieldType.Boolean;
            else if (FieldType.Vector2.ToString().Equals(ts)) return FieldType.Vector2;
            else if (FieldType.Vector3.ToString().Equals(ts)) return FieldType.Vector3;
            else if (FieldType.i18n.ToString().Equals(ts)) return FieldType.i18n;
            else if (FieldType.ArrayByte.ToString().Equals(ts)) return FieldType.ArrayByte;
            else if (FieldType.ArrayInt.ToString().Equals(ts)) return FieldType.ArrayInt;
            else if (FieldType.ArrayLong.ToString().Equals(ts)) return FieldType.ArrayLong;
            else if (FieldType.ArrayFloat.ToString().Equals(ts)) return FieldType.ArrayFloat;
            else if (FieldType.ArrayDouble.ToString().Equals(ts)) return FieldType.ArrayDouble;
            else if (FieldType.ArrayString.ToString().Equals(ts)) return FieldType.ArrayString;
            else if (FieldType.ArrayBoolean.ToString().Equals(ts)) return FieldType.ArrayBoolean;
            else if (FieldType.ArrayVector2.ToString().Equals(ts)) return FieldType.ArrayVector2;
            else if (FieldType.ArrayVector3.ToString().Equals(ts)) return FieldType.ArrayVector3;
            else if (FieldType.Arrayi18n.ToString().Equals(ts)) return FieldType.Arrayi18n;
            else if (FieldType.Ignore.ToString().Equals(ts)) return FieldType.Ignore;
            else return FieldType.Unknow;
        }

        /// <summary>
        /// file type to c# variable type string
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static string FieldTypeToTypeContent(FieldType fileType)
        {
            switch (fileType)
            {
                case FieldType.Byte:
                    return "byte";
                case FieldType.Int:
                    return "int";
                case FieldType.Long:
                    return "long";
                case FieldType.Float:
                    return "float";
                case FieldType.Double:
                    return "double";
                case FieldType.String:
                    return "string";
                case FieldType.i18n:
                    return "i18nObject";
                case FieldType.Boolean:
                    return "bool";
                case FieldType.Vector2:
                    return "Vector2";
                case FieldType.Vector3:
                    return "Vector3";
                case FieldType.ArrayByte:
                    return "byte[]";
                case FieldType.ArrayInt:
                    return "int[]";
                case FieldType.ArrayLong:
                    return "long[]";
                case FieldType.ArrayFloat:
                    return "float[]";
                case FieldType.ArrayDouble:
                    return "double[]";
                case FieldType.ArrayString:
                    return "string[]";
                case FieldType.ArrayBoolean:
                    return "bool[]";
                case FieldType.ArrayVector2:
                    return "Vector2[]";
                case FieldType.ArrayVector3:
                    return "Vector3[]";
                case FieldType.Arrayi18n:
                    return "i18nObject[]";
                default:
                    return "";
            }
        }

        #region  content to c# value
        /// <summary>
        /// content to bool
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool ContentToBooleanValue(string content)
        {
            return content.Equals("0") ? false : true;
        }

        /// <summary>
        /// content to i18nObject
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static i18nObject ContentToI18nObjectValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ':' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return new i18nObject(int.Parse(datas[0]), int.Parse(datas[1]));
        }

        /// <summary>
        /// content to vector2
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Vector2 ContentToVector2Value(string content)
        {
            if (string.IsNullOrEmpty(content)) return Vector2.zero;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return new Vector2(float.Parse(datas[0]), float.Parse(datas[1]));
        }

        /// <summary>
        /// content to vector3
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Vector3 ContentToVector3Value(string content)
        {
            if (string.IsNullOrEmpty(content)) return Vector3.zero;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            return new Vector3(float.Parse(datas[0]), float.Parse(datas[1]), float.Parse(datas[2]));
        }

        /// <summary>
        /// content to byte array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] ContentToArrayByteValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            byte[] data = new byte[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = byte.Parse(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to int array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int[] ContentToArrayIntValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            int[] data = new int[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = int.Parse(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to long array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static long[] ContentToArrayLongValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            long[] data = new long[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = long.Parse(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to float array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static float[] ContentToArrayFloatValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            float[] data = new float[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = float.Parse(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to double array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static double[] ContentToArrayDoubleValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            double[] data = new double[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = double.Parse(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to bool array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool[] ContentToArrayBooleanValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            bool[] data = new bool[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = datas[i].Equals("0") ? false : true;
            }
            return data;
        }

        /// <summary>
        /// content to bool array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string[] ContentToArrayStringValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { ',' };
            return content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// content to vector2 array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Vector2[] ContentToArrayVector2Value(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { '|' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            Vector2[] data = new Vector2[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = ContentToVector2Value(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to vector3 array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Vector3[] ContentToArrayVector3Value(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separator = new char[] { '|' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            Vector3[] data = new Vector3[datas.Length];
            for (int i = 0; i < datas.Length; i++)
            {
                data[i] = ContentToVector3Value(datas[i]);
            }
            return data;
        }

        /// <summary>
        /// content to i18nObject array
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static i18nObject[] ContentToArrayI18nObjectValue(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            char[] separatorArray = new char[] { ',' };
            string[] datasArray = content.Split(separatorArray, StringSplitOptions.RemoveEmptyEntries);
            i18nObject[] result = new i18nObject[datasArray.Length];
            for (int i = 0; i < datasArray.Length; i++)
            {
                string dataContent = datasArray[i];
                if (string.IsNullOrEmpty(dataContent)) continue;
                char[] separator = new char[] { ':' };
                string[] datas = dataContent.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                result[i] = new i18nObject(int.Parse(datas[0]), int.Parse(datas[1]));
            }
            return result;
        }
        #endregion
    }
}