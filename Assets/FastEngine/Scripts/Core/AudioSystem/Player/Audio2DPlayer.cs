/*
 * @Author: fasthro
 * @Date: 2020-04-02 14:16:42
 * @Description: 2D音频播放器
 */
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    public class Audio2DPlayer : AudioPlayer
    {
        // music dict
        private Dictionary<int, AudioAsset> mMusicDict = new Dictionary<int, AudioAsset>();
        // sound list
        private List<AudioAsset> mSounds = new List<AudioAsset>();
        // clear sound list
        private List<AudioAsset> mClearSounds = new List<AudioAsset>();

        public Audio2DPlayer(GameObject gameObject) : base(gameObject) { }

        /// <summary>
        /// 设置音乐音量
        /// </summary>
        /// <param name="volume"></param>
        public override void SetMusicVolume(float volume)
        {
            base.SetMusicVolume(volume);
            foreach (var item in mMusicDict.Values)
                item.totalVolume = volume;
        }

        /// <summary>
        /// 设置音效音量
        /// </summary>
        /// <param name="volume"></param>
        public override void SetSoundVolume(float volume)
        {
            base.SetSoundVolume(volume);
            for (int i = 0; i < mSounds.Count; i++)
                mSounds[i].totalVolume = volume;
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="asset">资源路径</param>
        /// <param name="loop">是否循环播放</param>
        /// <param name="volumeScale">音量缩放</param>
        /// <param name="delay">延时播放</param>
        /// <param name="fadeTime">过度时间</param>
        /// <returns></returns>
        public AudioAsset PlayMusic(int id, string asset, bool loop = true, float volumeScale = 1f, float delay = 0, float fadeTime = 0.5f)
        {
            AudioAsset audioAsset = null;
            if (!mMusicDict.TryGetValue(id, out audioAsset))
            {
                audioAsset = GetCacheAudioAsset(gameObject, false, AudioAssetType.Music);
                mMusicDict.Add(id, audioAsset);
            }
            audioAsset.id = id;
            PlayMusicWithAudioAsset(audioAsset, asset, loop, volumeScale, delay, fadeTime);
            return audioAsset;
        }

        /// <summary>
        /// 暂停音乐
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isPause"></param>
        /// <param name="fadeTime"></param>
        public void PauseMusic(int id, bool isPause, float fadeTime = 0.5f)
        {
            AudioAsset audioAsset = null;
            if (mMusicDict.TryGetValue(id, out audioAsset))
                PauseMusicWithAudioAsset(audioAsset, isPause, fadeTime);
        }

        /// <summary>
        /// 暂停全部音乐
        /// </summary>
        /// <param name="isPause"></param>
        /// <param name="fadeTime"></param>
        public void PauseMusicAll(bool isPause, float fadeTime = 0.5f)
        {
            foreach (int id in mMusicDict.Keys)
                PauseMusic(id, isPause, fadeTime);
        }

        /// <summary>
        /// 停止音乐
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fadeTime"></param>
        public void StopMusic(int id, float fadeTime = 0.5f)
        {
            AudioAsset audioAsset = null;
            if (mMusicDict.TryGetValue(id, out audioAsset))
                StopMusicWithAudioAsset(audioAsset, fadeTime);
        }

        /// <summary>
        /// 停止全部音乐
        /// </summary>
        /// <param name="fadeTime"></param>
        public void StopMusicAll(float fadeTime = 0.5f)
        {
            foreach (int id in mMusicDict.Keys)
                StopMusic(id);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="volumeScale"></param>
        /// <param name="delay"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public AudioAsset PlaySound(string asset, float volumeScale = 1, float delay = 0, float pitch = 1)
        {
            AudioAsset audioAsset = GetCacheAudioAsset(gameObject, false, AudioAssetType.Sound);
            mSounds.Add(audioAsset);

            PlayWithAudioClip(audioAsset, asset, false, volumeScale, delay, pitch);
            return audioAsset;
        }

        /// <summary>
        /// 暂停音效
        /// </summary>
        /// <param name="isPause"></param>
        public void PauseSoundAll(bool isPause)
        {
            for (int i = 0; i < mSounds.Count; i++)
            {
                if (isPause) mSounds[i].Pause();
                else mSounds[i].Play();
            }
        }

        protected override void OnUpdate()
        {
            // music

            // sound
            for (int i = 0; i < mSounds.Count; i++)
            {
                mSounds[i].Update();
                if (mSounds[i].playStatus == AudioPlayStatus.Stop)
                    mClearSounds.Add(mSounds[i]);
            }

            for (int i = 0; i < mClearSounds.Count; i++)
            {
                RecycleAudioAsset(mClearSounds[i]);
                mSounds.Remove(mClearSounds[i]);
            }
            mClearSounds.Clear();
        }
    }
}