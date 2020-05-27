/*
 * @Author: fasthro
 * @Date: 2020-03-31 15:05:51
 * @Description: 序列帧UV动画控制器
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FastEngine.Core
{
    public class SequenceFrameUVController : MonoBehaviour
    {
        public SequenceFrameUVClip[] clips;
        public bool aotuPlay = true;
        public bool asyncLoad = true;

        // 当前播放的动画
        public SequenceFrameUVClip clip { get; private set; }

        // sequenceFrameUVs
        private SequenceFrameUV[] sequenceFrameUVs;

        // 附加物体对象
        private GameObject attachGameObject;
        private AssetBundleLoader attachLoader;

        void Start() { if (aotuPlay) Play(0); }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="name"></param>
        public void Play(string name)
        {
            if (clip != null && clip.clipName.Equals(name) && clip.isPlaying) return;
            for (int i = 0; i < clips.Length; i++)
            {
                var clipName = clips[i].clipName;
                if (clipName.Equals(name))
                {
                    Play(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="clip"></param>
        private void Play(SequenceFrameUVClip clip) { Play(clip.clipName); }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="index"></param>
        private void Play(int index)
        {
            if (clips.Length == 0) return;
            if (clip == null) clip = clips[index];
            if (clip.clipName != clips[index].clipName || !clip.isPlaying)
            {
                clip = clips[index];
                clip.isPlaying = true;
                int unitCount = clip.units.Length;
                if (sequenceFrameUVs == null)
                {
                    sequenceFrameUVs = new SequenceFrameUV[unitCount];
                    for (int i = 0; i < unitCount; i++)
                        sequenceFrameUVs[i] = CreateSFUV("Unit-" + i.ToString());
                }
                else if (sequenceFrameUVs.Length < unitCount)
                {
                    int oldCount = sequenceFrameUVs.Length;
                    int newCount = unitCount - oldCount;
                    sequenceFrameUVs = new SequenceFrameUV[unitCount];

                    for (int i = 0; i < oldCount; i++)
                        sequenceFrameUVs[i] = transform.Find("Unit-" + i.ToString()).GetComponent<SequenceFrameUV>();

                    int _index = oldCount;
                    for (int i = 0; i < newCount; i++)
                    {
                        _index += i;
                        sequenceFrameUVs[_index] = CreateSFUV("Unit-" + _index.ToString());
                    }
                }
                bool active = false;
                for (int i = 0; i < sequenceFrameUVs.Length; i++)
                {
                    active = i < unitCount;
                    sequenceFrameUVs[i].SetActive(active);
                    if (active) sequenceFrameUVs[i].Play(clip.units[i]);
                }
                if (!string.IsNullOrEmpty(clip.attachGoRes)) loadAttachGameObject();
                else
                {
                    if (attachGameObject != null) GameObject.DestroyImmediate(attachGameObject);
                }
            }
        }

        /// <summary>
        /// 创建序列帧动画单元实例
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private SequenceFrameUV CreateSFUV(string name)
        {
            GameObject unitGo = new GameObject(name);
            unitGo.transform.parent = transform;
            unitGo.transform.localPosition = Vector3.zero;
            var sfuv = unitGo.AddComponent<SequenceFrameUV>();
            sfuv.asyncLoad = asyncLoad;
            return sfuv;
        }

        /// <summary>
        /// 加载附加物体
        /// </summary>
        private void loadAttachGameObject()
        {
            if (attachGameObject != null) GameObject.DestroyImmediate(attachGameObject);
#if UNITY_EDITOR
            var prefab = AssetDatabase.LoadAssetAtPath(clip.attachGoRes, typeof(GameObject)) as GameObject;
            attachGameObject = GameObject.Instantiate(prefab) as GameObject;
            attachGameObject.transform.parent = transform;
            attachGameObject.transform.localPosition = clip.attachGoSpace.position;
            attachGameObject.transform.localEulerAngles = clip.attachGoSpace.angle;
#else
            if (attachLoader != null) { attachLoader.Recycle(); }
            attachLoader = AssetBundleLoader.Allocate(clip.attachGoRes, (ready, res) =>
            {
                if (ready) onAttachLoaded(res);
            });
            if (asyncLoad) attachLoader.LoadAsync();
            else
            {
                if (attachLoader.LoadSync()) onAttachLoaded(attachLoader.assetRes);
            }
#endif
        }

        /// <summary>
        /// 附加物体加载完成
        /// </summary>
        /// <param name="res"></param>
        private void onAttachLoaded(Res res)
        {
            attachGameObject = GameObject.Instantiate(res.GetAsset<GameObject>()) as GameObject;
            attachGameObject.transform.parent = transform;
            attachGameObject.transform.localPosition = clip.attachGoSpace.position;
            attachGameObject.transform.localEulerAngles = clip.attachGoSpace.angle;
        }
    }
}