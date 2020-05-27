/*
 * @Author: fasthro
 * @Date: 2020-04-02 15:07:50
 * @Description: 游戏音频系统
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    [MonoSingletonPath("FastEngine/AudioSystem")]
    public class AudioSystem : MonoSingleton<AudioSystem>
    {
        // 2D音频播放器
        public Audio2DPlayer audio2DPlayer { get; private set; }
        // 3D音频播放器
        public Audio3DPlayer audio3DPlayer { get; private set; }

        // 全局总音量
        private float _volume = 1;
        public float volume
        {
            get { return _volume; }
            set
            {
                _volume = Mathf.Clamp01(value);
                UpdateMusicVolume();
                UpdateSoundVolume();
            }
        }

        // 背景音乐音量
        private float _musicVolume = 1;
        public float musicVolume
        {
            get { return _musicVolume; }
            set
            {
                _musicVolume = Mathf.Clamp01(value);
                UpdateMusicVolume();
            }
        }

        // 音效总音量
        private float _soundVolume = 1;
        public float soundVolume
        {
            get { return _soundVolume; }
            set
            {
                _soundVolume = Mathf.Clamp01(value);
                UpdateSoundVolume();
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void InitializeSingleton()
        {
            audio2DPlayer = new Audio2DPlayer(gameObject);
            audio3DPlayer = new Audio3DPlayer(gameObject);
        }

        /// <summary>
        /// 更新音乐音量
        /// </summary>
        private void UpdateMusicVolume()
        {
            audio2DPlayer.SetMusicVolume(volume * musicVolume);
            audio3DPlayer.SetMusicVolume(volume * musicVolume);
        }

        /// <summary>
        /// 更新音效音量
        /// </summary>
        private void UpdateSoundVolume()
        {
            audio2DPlayer.SetSoundVolume(volume * soundVolume);
            audio3DPlayer.SetSoundVolume(volume * soundVolume);
        }

        void Update()
        {
            audio2DPlayer.Update();
            audio3DPlayer.Update();
        }

        #region API

        /// <summary>
        /// 播放2D音乐
        /// </summary>
        /// <param name="id">音频id</param>
        /// <param name="asset">音频资源</param>
        /// <param name="volumeScale">音量缩放</param>
        /// <param name="loop">是否循环</param>
        /// <param name="fadeTime">过度时间</param>
        /// <param name="delay">延时</param>
        public static AudioAsset PlayMusic2D(int id, string asset, float volumeScale = 1, bool loop = true, float fadeTime = 0.5f, float delay = 0)
        {
            return Instance.audio2DPlayer.PlayMusic(id, asset, loop, volumeScale, delay, fadeTime);
        }

        /// <summary>
        /// 暂停2D音乐
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isPause"></param>
        /// <param name="fadeTime"></param>
        public static void PauseMusic2D(int id, bool isPause, float fadeTime = 0.5f)
        {
            Instance.audio2DPlayer.PauseMusic(id, isPause, fadeTime);
        }

        /// <summary>
        /// 停止2D音乐
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fadeTime"></param>
        public static void StopMusic2D(int id, float fadeTime = 0.5f)
        {
            Instance.audio2DPlayer.StopMusic(id, fadeTime);
        }

        /// <summary>
        /// 停止所有2D音乐
        /// </summary>
        /// <param name="fadeTime"></param>
        public static void StopMusicAll2D(float fadeTime = 0.5f)
        {
            Instance.audio2DPlayer.StopMusicAll(fadeTime);
        }

        /// <summary>
        /// 播放2D音效
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="volumeScale"></param>
        /// <param name="delay"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public static AudioAsset PlaySound2D(string asset, float volumeScale = 1, float delay = 1, float pitch = 1)
        {
            return Instance.audio2DPlayer.PlaySound(asset, volumeScale, delay, pitch);
        }

        /// <summary>
        /// 暂停所有2D音效
        /// </summary>
        /// <param name="isPause"></param>
        public static void PauseSoundAll2D(bool isPause)
        {
            Instance.audio2DPlayer.PauseSoundAll(isPause);
        }
        #endregion
    }
}
