/*
 * @Author: fasthro
 * @Date: 2020-01-04 18:08:33
 * @Description: 国际化配置
 */
using System.Collections.Generic;
using FastEngine.Core;
using UnityEngine;

namespace FastEngine.Core
{
    public class i18nConfig : ConfigObject
    {
        public List<SystemLanguage> languages { get; set; }

        protected override void OnInitialize()
        {
            if (languages == null)
            {
                languages = new List<SystemLanguage>();
            }
        }
    }
}