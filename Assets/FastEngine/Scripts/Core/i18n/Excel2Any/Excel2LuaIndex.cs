/*
 * @Author: fasthro
 * @Date: 2020-01-04 17:52:23
 * @Description: excel 2 lua index
 */
using System.Text;
using UnityEngine;

namespace FastEngine.Core.I18n
{
    public class Excel2LuaIndex
    {
        private StringBuilder m_builder = new StringBuilder();

        public Excel2LuaIndex(ExcelReader reader)
        {
            m_builder.Clear();

            m_builder.AppendLine("--[[ aotu generated]]");
            m_builder.AppendLine("--[[");
            m_builder.AppendLine(" * @Author: fasthro");
            m_builder.AppendLine(" * @Description: i18 model & key");
            m_builder.AppendLine(" ]]");

            // model
            m_builder.AppendLine("language_model = {");
            for (int i = 0; i < reader.sheets.Length; i++)
            {
                m_builder.AppendLine(string.Format("\t{0} = {1},", reader.sheets[i].name, i));
            }
            m_builder.AppendLine("}");

            // key
            m_builder.AppendLine("language_key = {");

            for (int i = 0; i < reader.sheets.Length; i++)
            {
                m_builder.AppendLine(reader.sheets[i].ToLuaKeyString());
            }
            m_builder.AppendLine("}");
            FilePathUtils.FileWriteAllText(FilePathUtils.Combine(Application.dataPath, "LuaScripts/language.lua"), m_builder.ToString());
        }
    }
}