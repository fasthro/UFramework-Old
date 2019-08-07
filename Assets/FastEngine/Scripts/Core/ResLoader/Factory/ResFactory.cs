/*
 * @Author: fasthro
 * @Date: 2019-06-28 11:10:01
 * @Description: Res 工厂
 */
namespace FastEngine.Core
{
    public static class ResFactory
    {
        /// <summary>
        /// 创建资源
        /// </summary>
        /// <param name="data"></param>
        public static Res Create(ResData data)
        {
            switch (data.type)
            {
                case ResType.Bundle:
                    return BundleRes.Allocate(data);
                case ResType.Asset:
                    return AssetRes.Allocate(data);
                case ResType.Resource:
                    return ResourceRes.Allocate(data);
                default:
                    return null;
            }
        }
    }
}