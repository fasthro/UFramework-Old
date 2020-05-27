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
    public class SequenceFrameMeshAsset : SequenceFrameAsset
    {
        public Mesh mesh { get; private set; }

        public SequenceFrameMeshAsset(){
            mesh = new Mesh();
            mesh.name = "sequence-frame-mesh";
        }
    }
}