/*
 * @Author: fasthro
 * @Date: 2019-11-11 13:40:19
 * @Description: 页数据
 */
using System.Text;
using FastEngine;
using FastEngine.Core;
using UnityEngine;

namespace FastEngine.Editor.Localization
{
    public class Sheet
    {
        public string name;
        public Column[] columns;

        public string ToLuaKeyString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i < columns.Length; i++)
            {
                builder.AppendLine(string.Format("\t{0}_{1} = {2},", name, columns[i].key, i));
            }
            return builder.ToString().Trim('\r', '\n');
        }

        public string ToCSharpKeyString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i < columns.Length; i++)
            {
                builder.AppendLine(string.Format("\tpublic static int {0}_{1} = {2};", name, columns[i].key, i));
            }
            return builder.ToString().Trim('\r', '\n');
        }

        public string ToValueString(int index)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i < columns.Length; i++)
            {
                builder.Append(columns[i].values[index] + "\n");
            }
            return builder.ToString().Trim('\r', '\n');
        }

        public override string ToString()
        {
            return "sheet name: " + name + " column count: " + columns.Length;
        }
    }
}