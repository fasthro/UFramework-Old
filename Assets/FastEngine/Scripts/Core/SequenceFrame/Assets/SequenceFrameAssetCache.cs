/*
 * @Author: fasthro
 * @Date: 2020-04-07 15:20:28
 * @Description: 帧动画资源缓存池
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FastEngine.Core
{
    [MonoSingletonPath("FastEngine/SequenceFrameAssetCache")]
    public class SequenceFrameAssetCache : MonoSingleton<SequenceFrameAssetCache>
    {
        private Dictionary<string, Texture> mTextureDict = new Dictionary<string, Texture>();
        private Dictionary<string, SequenceFrameMaterialAsset> mMaterialDict = new Dictionary<string, SequenceFrameMaterialAsset>();
        private Dictionary<int, SequenceFrameMeshAsset> mMeshDict = new Dictionary<int, SequenceFrameMeshAsset>();

        #region mesh
        Vector3[] _meshVertices;
        Vector2[] _meshUVs;

        /// <summary>
        /// 获取 meshAsset
        /// </summary>
        /// <param name="scaleX"></param>
        /// <param name="scaleY"></param>
        /// <returns></returns>
        public SequenceFrameMeshAsset GetMeshAsset(int scaleX, int scaleY)
        {
            int key = scaleX * 10000 + scaleY;
            SequenceFrameMeshAsset meshAsset = null;
            if (!mMeshDict.TryGetValue(key, out meshAsset))
            {
                // uv
                _meshUVs = new Vector2[4];
                _meshUVs[0] = new Vector2(0, 0);
                _meshUVs[1] = new Vector2(0, scaleX);
                _meshUVs[2] = new Vector2(scaleX, scaleY);
                _meshUVs[3] = new Vector2(scaleY, 0);
                // 顶点
                _meshVertices = new Vector3[4];
                _meshVertices[0] = new Vector3(0, 0, 0);
                _meshVertices[1] = new Vector3(0, scaleY, 0);
                _meshVertices[2] = new Vector3(scaleX, scaleY, 0);
                _meshVertices[3] = new Vector3(scaleX, 0, 0);
                // tris
                int[] tris = { 0, 1, 2, 0, 2, 3 };
                // mesh
                meshAsset = new SequenceFrameMeshAsset();
                meshAsset.mesh.vertices = _meshVertices;
                meshAsset.mesh.uv = _meshUVs;
                meshAsset.mesh.triangles = tris;

                mMeshDict.Add(key, meshAsset);
            }
            meshAsset.Retain();
            return meshAsset;
        }
        #endregion

        #region material

        /// <summary>
        /// 获取材质
        /// </summary>
        /// <param name="resName"></param>
        /// <returns></returns>
        public SequenceFrameMaterialAsset GetMaterialAsset(string resName)
        {
            SequenceFrameMaterialAsset materialAsset = null;
            if (!mMaterialDict.TryGetValue(resName, out materialAsset))
            {
                materialAsset = new SequenceFrameMaterialAsset();
                mMaterialDict.Add(resName, materialAsset);
            }
            return materialAsset;
        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="asset"></param>
        public void ReleaseAsset(SequenceFrameAsset asset)
        {
            asset.Release();
        }
    }
}
