/*
 * @Author: fasthro
 * @Date: 2020-04-03 14:56:22
 * @Description: 音频数据
 */
namespace FastEngine.Core
{
    public abstract class AudioUnit
    {
        public string asset;
        public bool loop = true;
        public bool is3D = true;
        public float volume = 1;
        public float delay = 0;
        public float fadeTime = 0.5f;
    }
}