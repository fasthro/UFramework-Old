/*
 * @Author: fasthro
 * @Date: 2020-03-31 15:05:51
 * @Description: 序列帧UV动画片段
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    [System.Serializable]
    public class SequenceFrameUVClip
    {
        public string clipName;

        public string attachGoRes;
        public SequenceFrameUVSpace attachGoSpace;

        public AudioSoundUnit audio;

        public SequenceFrameUVClipUnit[] units;


        [HideInInspector]
        public bool isPlaying { get; set; }
    }
}