/*
 * @Author: fasthro
 * @Date: 2019-12-09 15:06:09
 * @Description: AssetBundle Config
 */
using System.Collections.Generic;
using FastEngine.Core;

namespace FastEngine.Editor.AssetBundle
{
    public class AssetBundleConfig : ConfigObject
    {
        public List<Pack> packs;           // assetbundle pack
        public List<Source> sources;       // copu source

        protected override void OnInitialize()
        {
            if (packs == null)
            {
                packs = new List<Pack>();
            }
            if (sources == null)
            {
                sources = new List<Source>();
            }
        }
    }
}