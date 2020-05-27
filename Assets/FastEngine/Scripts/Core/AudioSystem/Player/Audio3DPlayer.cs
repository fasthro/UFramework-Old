/*
 * @Author: fasthro
 * @Date: 2020-04-02 14:16:42
 * @Description: 3D音频播放器
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    public class Audio3DPlayer : AudioPlayer
    {
        // music dict
        private Dictionary<GameObject, Dictionary<int, AudioAsset>> mMusicDict = new Dictionary<GameObject, Dictionary<int, AudioAsset>>();
        // sound list
        private Dictionary<GameObject, List<AudioAsset>> mSoundDict = new Dictionary<GameObject, List<AudioAsset>>();
        // clear sound list
        private List<GameObject> mClearOwnerSounds = new List<GameObject>();
        private List<AudioAsset> mClearSounds = new List<AudioAsset>();

        public Audio3DPlayer(GameObject gameObject) : base(gameObject) { }

        /// <summary>
        /// 设置音乐音量
        /// </summary>
        /// <param name="volume"></param>
        public override void SetMusicVolume(float volume)
        {
            base.SetMusicVolume(volume);
            foreach (var itemDict in mMusicDict.Values)
                foreach (var item in itemDict.Values)
                    item.totalVolume = volume;
        }

        /// <summary>
        /// 设置音效音量
        /// </summary>
        /// <param name="volume"></param>
        public override void SetSoundVolume(float volume)
        {
            base.SetSoundVolume(volume);
            foreach (var items in mSoundDict.Values)
                for (int i = 0; i < items.Count; i++)
                    items[i].totalVolume = volume;
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="id"></param>
        /// <param name="asset"></param>
        /// <param name="loop"></param>
        /// <param name="volumeScale"></param>
        /// <param name="delay"></param>
        /// <param name="fadeTime"></param>
        /// <returns></returns>
        public AudioAsset PlayMusic(GameObject owner, int id, string asset, bool loop = true, float volumeScale = 1f, float delay = 0, float fadeTime = 0.5f)
        {
            if (owner == null) return null;

            Dictionary<int, AudioAsset> dict = null;
            if (!mMusicDict.TryGetValue(owner, out dict))
            {
                dict = new Dictionary<int, AudioAsset>();
                mMusicDict.Add(owner, dict);
            }

            AudioAsset audioAsset = null;
            if (!dict.TryGetValue(id, out audioAsset))
            {
                audioAsset = GetCacheAudioAsset(owner, true, AudioAssetType.Music);
                dict.Add(id, audioAsset);
            }

            audioAsset.id = id;
            PlayMusicWithAudioAsset(audioAsset, asset, loop, volumeScale, delay, fadeTime);
            return audioAsset;
        }

        /// <summary>
        /// 暂停音乐
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="id"></param>
        /// <param name="isPause"></param>
        /// <param name="fadeTime"></param>
        public void PauseMusic(GameObject owner, int id, bool isPause, float fadeTime = 0.5f)
        {
            if (owner == null) return;

            Dictionary<int, AudioAsset> dict = null;
            if (mMusicDict.TryGetValue(owner, out dict))
            {
                AudioAsset audioAsset = null;
                if (dict.TryGetValue(id, out audioAsset))
                {
                    PauseMusicWithAudioAsset(audioAsset, isPause, fadeTime);
                }
            }
        }

        /// <summary>
        /// 暂停所有音乐
        /// </summary>
        /// <param name="isPause"></param>
        /// <param name="fadeTime"></param>
        public void PuaseMusicAll(bool isPause, float fadeTime)
        {
            foreach (var itemDict in mMusicDict.Values)
                foreach (var item in itemDict.Values)
                    PauseMusicWithAudioAsset(item, isPause, fadeTime);
        }

        /// <summary>
        /// 停止音乐
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="id"></param>
        /// <param name="fadeTime"></param>
        public void StopMusic(GameObject owner, int id, float fadeTime = 0.5f)
        {
            if (owner == null) return;

            Dictionary<int, AudioAsset> dict = null;
            if (mMusicDict.TryGetValue(owner, out dict))
            {
                AudioAsset audioAsset = null;
                if (dict.TryGetValue(id, out audioAsset))
                {
                    StopMusicWithAudioAsset(audioAsset, fadeTime);
                }
            }
        }

        /// <summary>
        /// 停止单个Owner的所有音乐
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="fadeTime"></param>
        public void StopMusicOwnerAll(GameObject owner, float fadeTime = 0.5f)
        {
            if (owner == null) return;

            Dictionary<int, AudioAsset> dict = null;
            if (mMusicDict.TryGetValue(owner, out dict))
            {
                foreach (var item in dict.Values)
                    StopMusicWithAudioAsset(item, fadeTime);
            }
        }

        /// <summary>
        /// 停止所有音乐
        /// </summary>
        /// <param name="isPause"></param>
        /// <param name="fadeTime"></param>
        public void StopMusicAll(float fadeTime = 0.5f)
        {
            foreach (var itemDict in mMusicDict.Values)
                foreach (var item in itemDict.Values)
                    StopMusicWithAudioAsset(item, fadeTime);
        }

        /// <summary>
        /// 释放单个Owner的音乐
        /// </summary>
        /// <param name="owner"></param>
        public void ReleaseOwnerMusic(GameObject owner)
        {
            if (owner == null) return;

            Dictionary<int, AudioAsset> dict = null;
            if (mMusicDict.TryGetValue(owner, out dict))
            {
                StopMusicOwnerAll(owner);

                foreach (var item in dict.Values)
                    if (item.audioSource != null)
                        Object.Destroy(item.audioSource);

                dict.Clear();
                mMusicDict.Remove(owner);
            }
        }

        /// <summary>
        /// 释放所有音乐
        /// </summary>
        public void ReleaseMusicAll()
        {
            foreach (var item in mMusicDict.Keys)
                ReleaseOwnerMusic(item);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="asset"></param>
        /// <param name="volumeScale"></param>
        /// <param name="delay"></param>
        public void PlaySound(GameObject owner, string asset, float volumeScale = 1, float delay = 0)
        {
            if (owner == null) return;

            List<AudioAsset> list = null;
            if (!mSoundDict.TryGetValue(owner, out list))
            {
                list = new List<AudioAsset>();
                mSoundDict.Add(owner, list);
            }

            AudioAsset audioAsset = GetCacheAudioAsset(gameObject, true, AudioAssetType.Sound); ;
            list.Add(audioAsset);

            PlayWithAudioClip(audioAsset, asset, false, volumeScale, delay);

            // 检查当前物体上的无效AuduoSound
            mClearSounds.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Update();
                if (list[i].playStatus == AudioPlayStatus.Stop)
                    mClearSounds.Add(list[i]);
            }
            for (int i = 0; i < mClearSounds.Count; i++)
            {
                AudioAsset tempAudioAsset = mClearSounds[i];

                if (tempAudioAsset.audioSource != null)
                    Object.Destroy(tempAudioAsset.audioSource);

                RecycleAudioAsset(tempAudioAsset);
                list.Remove(tempAudioAsset);
            }
            mClearSounds.Clear();
        }

        /// <summary>
        /// 播放音效(指定空间坐标点)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="asset"></param>
        /// <param name="volumeScale"></param>
        /// <param name="delay"></param>
        public void PlaySoundAtPoint(Vector3 position, string asset, float volume = 1, float delay = 0)
        {
            AssetBundleLoader loader = CreateAudioAssetLoader(asset);
            AudioClip audioClip = LoadAudioClip(loader);
            if (audioClip != null) CoroutineFactory.CreateAndStart(AsyncPlaySoundAtPoint(position, audioClip, volume, delay), (manual) =>
            {
                loader.Unload();
                loader.Recycle();
            });
        }

        IEnumerator AsyncPlaySoundAtPoint(Vector3 position, AudioClip audioClip, float volume, float delay)
        {
            yield return new WaitForSeconds(delay);
            AudioSource.PlayClipAtPoint(audioClip, position, volume);
        }

        /// <summary>
        /// 暂停音效
        /// </summary>
        /// <param name="isPause"></param>
        public void PauseSoundAll(bool isPause)
        {
            foreach (var items in mSoundDict.Values)
                for (int i = 0; i < items.Count; i++)
                    if (isPause) items[i].Pause();
                    else items[i].Play();
        }

        /// <summary>
        /// 释放单个 Owner 的所有音效
        /// </summary>
        /// <param name="owner"></param>
        public void ReleaseOwnerSound(GameObject owner)
        {
            if (owner == null) return;

            List<AudioAsset> list = null;
            if (mSoundDict.TryGetValue(owner, out list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].audioSource != null)
                        Object.Destroy(list[i].audioSource);
                }
                list.Clear();
                mSoundDict.Remove(owner);
            }
        }

        /// <summary>
        /// 释放全部音效
        /// </summary>
        public void ReleaseSoundAll()
        {
            foreach (var item in mSoundDict.Keys)
                ReleaseOwnerSound(item);

            mSoundDict.Clear();
        }

        protected override void OnUpdate()
        {
            // music

            // sound
            mClearOwnerSounds.Clear();
            mClearOwnerSounds.AddRange(mSoundDict.Keys);
            for (int i = 0; i < mClearOwnerSounds.Count; i++)
            {
                if (mClearOwnerSounds[i] == null) mSoundDict.Remove(mClearOwnerSounds[i]);
            }
            foreach (var list in mSoundDict)
            {
                var values = list.Value;
                foreach (var item in values)
                {
                    item.Update();
                    if (item.playStatus == AudioPlayStatus.Stop)
                    {
                        RecycleAudioAsset(item);
                        mSoundDict[list.Key].Remove(item);
                    }
                }
            }
        }
    }
}