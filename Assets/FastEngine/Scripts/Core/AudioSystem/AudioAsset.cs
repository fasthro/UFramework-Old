/*
 * @Author: fasthro
 * @Date: 2020-04-02 16:08:23
 * @Description: audioAsset
 */

using UnityEngine;

namespace FastEngine.Core
{
    /// <summary>
    /// 音频类型
    /// </summary>
    public enum AudioAssetType
    {
        Music,
        Sound
    }

    /// <summary>
    /// 音频播放状态
    /// </summary>
    public enum AudioPlayStatus
    {
        Playing,
        Pause,
        Stoping,
        Stop
    }

    /// <summary>
    /// 音频资源
    /// </summary>
    public class AudioAsset
    {
        public int id;
        public AudioAssetType assetType;
        public AudioPlayStatus playStatus;
        public AudioSource audioSource;
        public string asset;
        public AssetBundleLoader assetLoader;

        // 总音量
        private float _totalVolume;
        public float totalVolume
        {
            get { return _totalVolume; }
            set
            {
                _totalVolume = value;
                volume = _totalVolume * volumeScale;
            }
        }
        // 实际最大音量
        public float realMaxVolume { get { return totalVolume * volumeScale; } }
        // 实际音量
        public float volume
        {
            get { return audioSource.volume; }
            set { audioSource.volume = value; }
        }
        // 音量缩放
        private float _volumeScale = 1;
        public float volumeScale
        {
            get { return _volumeScale; }
            set
            {
                _volumeScale = value;
                ResetMaxVolume();
            }
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        public void Reset()
        {
            asset = string.Empty;
            audioSource.pitch = 1;
        }

        /// <summary>
        /// 重置到最大音量
        /// </summary>
        public void ResetMaxVolume() { volume = totalVolume * volumeScale; }

        /// <summary>
        /// 播放
        /// </summary>
        public void Play() { Play(0); }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="delay">延时</param>
        public void Play(float delay)
        {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.PlayDelayed(delay);
                playStatus = AudioPlayStatus.Playing;
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (audioSource != null && audioSource.clip != null && audioSource.isPlaying)
            {
                audioSource.Pause();
                playStatus = AudioPlayStatus.Pause;
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (audioSource != null)
                audioSource.Stop();
            playStatus = AudioPlayStatus.Stop;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public void Update()
        {
            if (playStatus != AudioPlayStatus.Stop)
            {
                if (audioSource == null || (!audioSource.isPlaying && playStatus != AudioPlayStatus.Pause))
                    Stop();
            }
        }
    }
}
