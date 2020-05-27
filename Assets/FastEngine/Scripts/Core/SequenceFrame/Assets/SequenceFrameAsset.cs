/*
 * @Author: fasthro
 * @Date: 2020-04-07 15:20:28
 * @Description: 帧动画资源
 */
using System.Collections;
using System.Collections.Generic;
using FastEngine.Ref;
using UnityEngine;

namespace FastEngine.Core
{
    public abstract class SequenceFrameAsset : IRef
    {
        protected int mRefCount;
        public int refCount { get { return mRefCount; } }

        public void Release()
        {

        }

        public void Retain()
        {

        }
    }
}
