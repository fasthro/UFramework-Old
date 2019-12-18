/*
 * @Author: fasthro
 * @Date: 2019-12-09 15:06:09
 * @Description: AssetBundle Config
 */
using System.Collections.Generic;

namespace FastEngine.Editor.AssetBundle
{
    public class AssetBundleConfig
    {
        public List<Pack> packs;           // assetbundle pack
        public List<Source> sources;       // copu source

        public AssetBundleConfig()
        {
            packs = new List<Pack>();
            sources = new List<Source>();
        }
    }
}