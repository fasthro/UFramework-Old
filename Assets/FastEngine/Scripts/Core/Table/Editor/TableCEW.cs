/*
 * @Author: fasthro
 * @Date: 2019-11-09 15:17:31
 * @Description: Table 配置编辑器
 */
using System.Collections.Generic;
using System.IO;
using FastEngine.Core;
using FastEngine.Utils;
using LitJson;
using UnityEditor;
using UnityEngine;

namespace FastEngine.Editor.Table
{
    public class TableCEW : CEWBase
    {
        protected override void OnInitialize()
        {
            titleContent.text = "Table 配置编辑器";
        }

        void OnGUI()
        {

        }
    }
}