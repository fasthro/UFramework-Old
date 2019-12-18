/*
 * @Author: fasthro
 * @Date: 2020-01-04 18:08:33
 * @Description: 国际化配置
 */
using System.Collections.Generic;
using FastEngine.Core;
using UnityEngine;

namespace FastEngine.Editor.I18n
{
    public class i18nConfig : IConfig
    {
        public List<SystemLanguage> languages { get; set; }

        public void Initialize()
        {
            if (languages == null)
            {
                languages = new List<SystemLanguage>();
            }
        }
    }
}