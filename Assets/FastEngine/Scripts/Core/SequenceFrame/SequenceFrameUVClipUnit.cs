/*
 * @Author: fasthro
 * @Date: 2020-04-01 19:35:49
 * @Description: 序列帧UV动画单元
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    [System.Serializable]
    public class SequenceFrameUVClipUnit
    {
        // 缩放
        public int scaleX = 1;
        public int scaleY = 1;
        // 翻转
        public int filpX = 1;
        public int filpY = 1;
        // 序列图横纵
        public int horizontal = 8;
        public int vertical = 4;
        // 动画速度
        public int speed = 500;
        public float speedScale = 1;
        // 延时播放
        public float delay;
        // 空间位置(local)
        public SequenceFrameUVSpace space;
        // 贴图资源
        public string textureRes;
    }
}