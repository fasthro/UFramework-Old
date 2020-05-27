/*
 * @Author: fasthro
 * @Date: 2020-04-02 14:16:42
 * @Description: 音频播放器基类
 */
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    public abstract class AudioPlayer
    {
        // 音乐音量
        public float musicVolume { get; private set; }
        // 音效音量
        public float soundVolume { get; private set; }

        // 音频资源池
        protected Stack<AudioAsset> mAudioAssetCache = new Stack<AudioAsset>();

        // 过度
        protected Dictionary<AudioAsset, AudioFade> mFadeDict = new Dictionary<AudioAsset, AudioFade>();
        protected Stack<AudioFade> mFadeCache = new Stack<AudioFade>();
        protected List<AudioAsset> mFadeDeletes = new List<AudioAsset>();

        // gameObject
        protected GameObject gameObject;

        public AudioPlayer(GameObject gameObject)
        {
            this.gameObject = gameObject;
            musicVolume = 1;
            soundVolume = 1;
        }

        /// <summary>
        /// 设置音乐音量
        /// </summary>
        /// <param name="volume"></param>
        public virtual void SetMusicVolume(float volume)
        {
            musicVolume = volume;
        }

        /// <summary>
        /// 设置音效音量
        /// </summary>
        /// <param name="volume"></param>
        public virtual void SetSoundVolume(float volume)
        {
            soundVolume = volume;
        }

        #region clip asset
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        protected AssetBundleLoader CreateAudioAssetLoader(string asset)
        {
            AssetBundleLoader loader = AssetBundleLoader.Allocate(asset, null);
            if (loader.LoadSync()) return loader;
            loader.Unload();
            loader.Recycle();
            return null;
        }

        /// <summary>
        /// 加载 AudioClip
        /// </summary>
        /// <param name="loader"></param>
        /// <returns></returns>
        protected AudioClip LoadAudioClip(AssetBundleLoader loader)
        {
            if (loader == null) return null;
            AudioClip clip = loader.assetRes.GetAsset<AudioClip>();
            if (clip == null) Debug.LogError("无法加载音频文件: " + loader.assetRes.assetName);
            return clip;
        }

        /// <summary>
        /// 在缓存池中获取 AudioAsset
        /// </summary>
        /// <param name="go">目标物体</param>
        /// <param name="is3D">是否为3d音乐</param>
        /// <param name="assetType">资源类型</param>
        /// <returns></returns>
        protected AudioAsset GetCacheAudioAsset(GameObject go, bool is3D, AudioAssetType assetType)
        {
            AudioAsset audioAsset = null;
            if (mAudioAssetCache.Count > 0)
            {
                audioAsset = mAudioAssetCache.Pop();
                audioAsset.Reset();
            }
            else
            {
                audioAsset = new AudioAsset();
                audioAsset.audioSource = go.AddComponent<AudioSource>();
            }

            audioAsset.audioSource.spatialBlend = is3D ? 1 : 0;
            audioAsset.assetType = assetType;

            if (assetType == AudioAssetType.Music) audioAsset.totalVolume = musicVolume;
            else audioAsset.totalVolume = soundVolume;

            return audioAsset;
        }

        /// <summary>
        /// 回收 AudioAsset
        /// </summary>
        /// <param name="audioAsset"></param>
        protected void RecycleAudioAsset(AudioAsset audioAsset)
        {
            if (audioAsset.audioSource.clip != null)
            {
                audioAsset.audioSource.clip = null;
                audioAsset.assetLoader.Unload();
                audioAsset.assetLoader.Recycle();
            }
            mAudioAssetCache.Push(audioAsset);
        }
        #endregion

        #region opt
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="audioAsset"></param>
        /// <param name="asset"></param>
        /// <param name="loop"></param>
        /// <param name="volumeScale"></param>
        /// <param name="delay"></param>
        /// <param name="pitch"></param>
        protected void PlayWithAudioClip(AudioAsset audioAsset, string asset, bool loop = true, float volumeScale = 1, float delay = 1, float pitch = 1)
        {
            AssetBundleLoader loader = CreateAudioAssetLoader(asset);
            AudioClip clip = LoadAudioClip(loader);
            if (clip == null) Debug.LogError("音频文件无法播放, 无法加载音频文件");
            else
            {
                audioAsset.asset = asset;
                audioAsset.assetLoader = loader;
                audioAsset.audioSource.clip = clip;
                audioAsset.audioSource.loop = loop;
                audioAsset.audioSource.pitch = pitch;
                audioAsset.volumeScale = volumeScale;
                audioAsset.Play(delay);
            }
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="audioAsset"></param>
        /// <param name="asset"></param>
        /// <param name="loop"></param>
        /// <param name="volumeScale"></param>
        /// <param name="delay"></param>
        /// <param name="fadeTime"></param>
        protected void PlayMusicWithAudioAsset(AudioAsset audioAsset, string asset, bool loop = true, float volumeScale = 1, float delay = 0, float fadeTime = 0.5f)
        {
            if (audioAsset.asset.Equals(asset))
            {
                if (audioAsset.playStatus != AudioPlayStatus.Playing)
                {
                    AddFade(audioAsset, AudioFadeType.FadeIn, fadeTime, delay, null, null);
                    audioAsset.Play();
                }
            }
            else
            {
                if (audioAsset.playStatus != AudioPlayStatus.Playing) fadeTime = fadeTime / 2f;
                AddFade(audioAsset, AudioFadeType.FadeIn, fadeTime, delay, null, (value) =>
                {
                    PlayWithAudioClip(value, asset, loop, volumeScale, delay);
                });
            }
        }

        /// <summary>
        /// 暂停音乐
        /// </summary>
        /// <param name="audioAsset"></param>
        /// <param name="isPause"></param>
        /// <param name="fadeTime"></param>
        protected void PauseMusicWithAudioAsset(AudioAsset audioAsset, bool isPause, float fadeTime = 0.5f)
        {
            if (isPause)
            {
                if (audioAsset.playStatus == AudioPlayStatus.Playing)
                {
                    audioAsset.playStatus = AudioPlayStatus.Pause;
                    AddFade(audioAsset, AudioFadeType.FadeOut, fadeTime, 0, (value) =>
                    {
                        value.Pause();
                    }, null);
                }
            }
            else
            {
                if (audioAsset.playStatus == AudioPlayStatus.Pause)
                {
                    audioAsset.Play();
                    AddFade(audioAsset, AudioFadeType.FadeIn, fadeTime, 0, null, null);
                }
            }
        }

        /// <summary>
        /// 停止播放音乐
        /// </summary>
        /// <param name="audioAsset"></param>
        /// <param name="fadeTime"></param>
        protected void StopMusicWithAudioAsset(AudioAsset audioAsset, float fadeTime = 0.5f)
        {
            if (audioAsset.playStatus != AudioPlayStatus.Stop)
            {
                audioAsset.playStatus = AudioPlayStatus.Stop;
                AddFade(audioAsset, AudioFadeType.FadeOut, fadeTime, 0, (value) =>
                {
                    audioAsset.Stop();
                }, null);
            }
        }
        #endregion

        #region fade
        /// <summary>
        /// 音频播放过度
        /// </summary>
        /// <param name="audioAsset"></param>
        /// <param name="fadeType"></param>
        /// <param name="fadeTime"></param>
        /// <param name="delay"></param>
        /// <param name="fadeCompleteCallback"></param>
        /// <param name="fadeOutCompleteCallback"></param>
        public void AddFade(AudioAsset audioAsset, AudioFadeType fadeType, float fadeTime, float delay, AudioFadeCallback fadeCompleteCallback, AudioFadeCallback fadeOutCompleteCallback)
        {
            AudioFade fade = null;
            if (!mFadeDict.TryGetValue(audioAsset, out fade))
            {
                if (mFadeCache.Count > 0) fade = mFadeCache.Pop();
                else fade = new AudioFade();

                mFadeDict.Add(audioAsset, fade);
            }

            fade.fadeOutCompleteCallback.InvokeGracefully(fade.audioAsset);
            fade.fadeOutCompleteCallback = fadeOutCompleteCallback;

            fade.fadeCompleteCallback.InvokeGracefully(fade.audioAsset);
            fade.fadeCompleteCallback = fadeCompleteCallback;

            fade.audioAsset = audioAsset;
            fade.fadeType = fadeType;
            fade.delayTime = delay;
            if (fadeTime <= 0) fade.fadeTime = 0.000001f;
            else fade.fadeTime = fadeTime;
            fade.initialize();
        }

        /// <summary>
        /// fade in
        /// </summary>
        /// <param name="fade"></param>
        /// <returns></returns>
        private bool FadeIn(AudioFade fade)
        {
            float oldVolume = fade.tempVolume;
            float speed = fade.audioAsset.realMaxVolume / fade.fadeTime * 2f;
            oldVolume = oldVolume + speed * Time.unscaledDeltaTime;
            fade.audioAsset.volume = oldVolume;
            fade.tempVolume = oldVolume;

            if (oldVolume < fade.audioAsset.realMaxVolume) return false;
            else
            {
                fade.audioAsset.ResetMaxVolume();
                return true;
            }
        }

        /// <summary>
        /// fade out
        /// </summary>
        /// <param name="fade"></param>
        /// <returns></returns>
        private bool FadeOut(AudioFade fade)
        {
            float oldVolume = fade.tempVolume;
            float speed = fade.audioAsset.realMaxVolume / fade.fadeTime;
            oldVolume = oldVolume - speed * Time.unscaledDeltaTime;
            fade.audioAsset.volume = oldVolume;
            fade.tempVolume = oldVolume;

            if (oldVolume > 0) return false;
            else
            {
                fade.audioAsset.volume = 0;
                return true;
            }
        }

        /// <summary>
        /// fade out -> in
        /// </summary>
        /// <param name="fade"></param>
        /// <returns></returns>
        private bool FadeOut2In(AudioFade fade)
        {
            if (fade.fadeStatus == AudioFadeStatus.FadeOut)
            {
                if (FadeOut(fade))
                {
                    fade.fadeStatus = AudioFadeStatus.Delay;
                    return false;
                }
            }
            else if (fade.fadeStatus == AudioFadeStatus.Delay)
            {
                fade.delayTime -= Time.unscaledDeltaTime;
                if (fade.delayTime <= 0)
                {
                    fade.fadeStatus = AudioFadeStatus.FadeIn;
                    return false;
                }
            }
            else if (fade.fadeStatus == AudioFadeStatus.FadeIn)
            {
                if (FadeIn(fade))
                {
                    fade.fadeStatus = AudioFadeStatus.Complete;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// update fade
        /// </summary>
        private void UpdateFade()
        {
            if (mFadeDict.Count > 0)
            {
                foreach (var item in mFadeDict.Values)
                {
                    bool isComplete = false;
                    switch (item.fadeType)
                    {
                        case AudioFadeType.FadeIn:
                            isComplete = FadeIn(item);
                            break;
                        case AudioFadeType.FadeOut:
                            isComplete = FadeOut(item);
                            break;
                        case AudioFadeType.FadeOut2In:
                            isComplete = FadeOut2In(item);
                            break;
                    }
                    if (isComplete) mFadeDeletes.Add(item.audioAsset);
                }

                if (mFadeDeletes.Count > 0)
                {
                    for (int i = 0; i < mFadeDeletes.Count; i++)
                    {
                        AudioAsset audioAsset = mFadeDeletes[i];
                        AudioFade fade = mFadeDict[audioAsset];

                        fade.fadeCompleteCallback.InvokeGracefully(audioAsset);
                        fade.fadeCompleteCallback = null;
                        fade.fadeOutCompleteCallback = null;

                        mFadeCache.Push(fade);
                        mFadeDict.Remove(audioAsset);
                    }
                    mFadeDeletes.Clear();
                }
            }
        }
        #endregion

        public void Update()
        {
            UpdateFade();
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            
        }
    }
}
