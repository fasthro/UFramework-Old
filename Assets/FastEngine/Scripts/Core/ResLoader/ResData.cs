/*
 * @Author: fasthro
 * @Date: 2019-06-26 14:30:37
 * @Description: 资源数据
 */
namespace FastEngine.Core
{
    public class ResData : IPoolObject
    {
        // 资源名称
        private string m_assetName;
        public string assetName { get { return m_assetName; } }

        // bundle 名称
        private string m_bundleName;
        public string bundleName { get { return m_bundleName; } }

        // 资源类型
        private ResType m_type;
        public ResType type { get { return m_type; } }

        // ResPoolSystem 中的 key
        public string poolKey
        {
            get
            {
                switch (m_type)
                {
                    case ResType.Resource:
                        return m_assetName.ToLower();
                    case ResType.Bundle:
                        return m_bundleName.ToLower();
                    case ResType.Asset:
                        return (m_bundleName + m_assetName).ToLower();
                    default:
                        return "";
                }
            }
        }
        
        /// <summary>
        /// ResourceRes
        /// </summary>
        /// <param name="assetName"></param>
        public static ResData AllocateResource(string assetName)
        {
            return Allocate(assetName, "", ResType.Resource);
        }

        /// <summary>
        /// BundleRes
        /// </summary>
        /// <param name="bundleName"></param>
        public static ResData AllocateBundle(string bundleName)
        {
            return Allocate("", bundleName, ResType.Bundle);
        }

        /// <summary>
        /// Asset
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="bundleName"></param>
        public static ResData AllocateAsset(string assetName, string bundleName)
        {
            return Allocate(assetName, bundleName, ResType.Asset);
        }

        private static ResData Allocate(string assetName, string bundleName, ResType type)
        {
            var data = ObjectPool<ResData>.Instance.Allocate();
            data.Init(assetName, bundleName, type);
            return data;
        }

        #region  IPoolable
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<ResData>.Instance.Recycle(this);
        }
        #endregion

        public void Init(string assetName, string bundleName, ResType type)
        {
            m_bundleName = bundleName;
            m_assetName = assetName;
            m_type = type;
        }
    }
}