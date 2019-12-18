/*
 * @Author: fasthro
 * @Date: 2019-11-27 15:03:43
 * @Description: 打包配置
 */
namespace FastEngine.Core
{
    public class BuildPackConfig
    {
        public bool cleanBuild { get; set; }           // 是否清理打包

        public int bundleVersionCode { get; set; }     // bundleVersionCode(每次打包自增1)
        public string keystoreName { get; set; }       // 签名文件
        public string keystorePass { get; set; }       // 密钥
        public string keyaliasName { get; set; }       // 别名
        public string keyaliasPass { get; set; }       // 密钥
        public bool andoridIL2CPP { get; set; }        //  IL2CPP
        public bool iOSIL2CPP { get; set; }            //  IL2CPP
        public bool WindowsIL2CPP { get; set; }        //  IL2CPP
    }
}