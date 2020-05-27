/*
 * @Author: fasthro
 * @Date: 2020-04-07 15:20:28
 * @Description: 帧动画材质资源
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    public class SequenceFrameMaterialAsset : SequenceFrameAsset
    {
        public Material material { get; private set; }

        public SequenceFrameMaterialAsset()
        {
            material = new Material(Shader.Find("FastEngine/SequenceFrameUV"));
            material.name = "sequence-frame-material";
        }
    }
}
