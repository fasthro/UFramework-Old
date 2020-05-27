/*
 * @Author: fasthro
 * @Date: 2020-03-31 15:05:51
 * @Description: 序列帧UV动画
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FastEngine.Core
{
    public class SequenceFrameUV : MonoBehaviour
    {
        public SequenceFrameUVClipUnit clipUnit;
        public bool asyncLoad = true;

        // texture
        private Texture mTexture;
        // 播放延时
        private float mDealy;
        private bool isWaitDealy;

        // 
        private GameObject mUnitGo;
        private MeshRenderer mMeshRender;
        private MeshFilter mMeshFilter;
        private AssetBundleLoader mTextureLoader;

        private SequenceFrameMeshAsset mMeshAsset;
        private SequenceFrameMaterialAsset mMaterialAsset;

        private MaterialPropertyBlock mMaterialPropertyBlock;

        private Vector3 m_scale;

        // 是否开启优化
        private bool optimize = true;

        // 动画是否正在播放
        public bool isPlaying { get; private set; }
        // 动画是否正在暂停
        public bool isPause { get; private set; }

        void Update()
        {
            if (isWaitDealy)
            {
                mDealy -= Time.deltaTime;
                if (mDealy <= 0)
                {
                    isWaitDealy = false;
                    Play(0);
                }
            }
        }

        /// <summary>
        /// 是否显示激活序列对象
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active) { gameObject.SetActive(active); }

        /// <summary>
        /// 播放动画
        /// </summary>
        public void Play(SequenceFrameUVClipUnit clipUnit)
        {
            if (this.clipUnit == null || mTexture == null || !this.clipUnit.textureRes.Equals(clipUnit.textureRes))
            {
                this.clipUnit = clipUnit;
                loadTexture();
                return;
            }

            if (clipUnit.scaleX != this.clipUnit.scaleX || clipUnit.scaleY != this.clipUnit.scaleY)
                UpdateMesh();

            this.clipUnit = clipUnit;
            Play(clipUnit.delay);
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        public void Play()
        {
            Play(0);
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="dealy">延时</param>
        public void Play(float dealy)
        {
            Pause();
            mDealy = dealy;
            isWaitDealy = true;
            if (mDealy <= 0)
            {
                isWaitDealy = false;
                isPlaying = true;
                isPause = false;
                UpdateMaterial();
            }
            transform.localPosition = clipUnit.space.position;
            transform.localEulerAngles = clipUnit.space.angle;
            m_scale.x = clipUnit.filpX;
            m_scale.y = clipUnit.filpY;
            mUnitGo.transform.localScale = m_scale;
        }

        /// <summary>
        /// 暂停播放
        /// </summary>
        public void Pause()
        {
            isPause = true;
            isPlaying = false;
            UpdateMaterial();
        }

        /// <summary>
        /// 设置贴图
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        public void SetTexture(Texture texture)
        {
            this.mTexture = texture;
            if (mMeshRender == null) CreateMesh();
            if (mTexture != null) UpdateMaterial();
        }

        /// <summary>
        /// 加载贴图
        /// </summary>
        private void loadTexture()
        {
#if UNITY_EDITOR
            mTexture = AssetDatabase.LoadAssetAtPath(clipUnit.textureRes, typeof(Texture)) as Texture;
            SetTexture(mTexture);
            if (!isPause) Play(clipUnit.delay);
#else
            if (mTextureLoader != null) { mTextureLoader.Recycle(); }
            mTextureLoader = AssetBundleLoader.Allocate(clipUnit.textureRes, (ready, res) =>
            {
                if (ready) onTextureLoaded(res);
            });
            if (asyncLoad) mTextureLoader.LoadAsync();
            else
            {
                if (mTextureLoader.LoadSync()) onTextureLoaded(mTextureLoader.assetRes);
            }
#endif
        }

        /// <summary>
        /// 贴图加载完成
        /// </summary>
        /// <param name="res"></param>
        private void onTextureLoaded(Res res)
        {
            mTexture = res.GetAsset<Texture>();
            SetTexture(mTexture);
            if (!isPause) Play(clipUnit.delay);
        }

        #region Mesh
        /// <summary>
        /// 创建 mesh
        /// </summary>
        private void CreateMesh()
        {
            mMeshAsset = SequenceFrameAssetCache.Instance.GetMeshAsset(clipUnit.scaleX, clipUnit.scaleY);
            // gameObject
            mUnitGo = new GameObject("Unit-UV");
            mUnitGo.transform.SetParent(transform, false);
            m_scale = new Vector3(clipUnit.filpX, clipUnit.filpY, 1);
            mUnitGo.transform.localScale = m_scale;
            mMeshFilter = mUnitGo.AddComponent<MeshFilter>();
            mMeshFilter.mesh = mMeshAsset.mesh;

            mMeshRender = mUnitGo.AddComponent<MeshRenderer>();
            mMaterialPropertyBlock = new MaterialPropertyBlock();
        }

        /// <summary>
        /// 更新材质
        /// </summary>
        private void UpdateMaterial()
        {
            mMaterialAsset = SequenceFrameAssetCache.Instance.GetMaterialAsset(clipUnit.textureRes);
            mMeshRender.material = mMaterialAsset.material;
            if (!optimize)
            {
                mMeshRender.material.SetTextureScale("_MainTex", new Vector2(1.0f / clipUnit.scaleX, 1.0f / clipUnit.scaleY));
                mMeshRender.material.SetFloat("_Speed", clipUnit.speed);
                mMeshRender.material.SetFloat("_SpeedScale", isPause ? 0 : clipUnit.speedScale);
                mMeshRender.material.SetFloat("_Column", clipUnit.vertical);
                mMeshRender.material.SetFloat("_Row", clipUnit.horizontal);
                mMeshRender.material.SetTexture("_MainTex", mTexture);
            }
            else
            {
                mMaterialPropertyBlock.SetVector("_MainTex_ST", new Vector4(1.0f / clipUnit.scaleX, 1.0f / clipUnit.scaleY, 1f, 1f));
                mMaterialPropertyBlock.SetFloat("_Speed", clipUnit.speed);
                mMaterialPropertyBlock.SetFloat("_SpeedScale", isPause ? 0 : clipUnit.speedScale);
                mMaterialPropertyBlock.SetFloat("_Column", clipUnit.vertical);
                mMaterialPropertyBlock.SetFloat("_Row", clipUnit.horizontal);
                if (mTexture != null) mMaterialPropertyBlock.SetTexture("_MainTex", mTexture);
                else Debug.LogError("SequenceFrameUV Texture null. res: " + clipUnit.textureRes);
                mMeshRender.SetPropertyBlock(mMaterialPropertyBlock);
            }
        }

        /// <summary>
        /// 更新mesh
        /// </summary>
        private void UpdateMesh()
        {
            SequenceFrameAssetCache.Instance.ReleaseAsset(mMeshAsset);
            mMeshAsset = SequenceFrameAssetCache.Instance.GetMeshAsset(clipUnit.scaleX, clipUnit.scaleY);
            mMeshFilter.mesh = mMeshAsset.mesh;
            if (!optimize) mMeshRender.material.SetTextureScale("_MainTex", new Vector2(1.0f / clipUnit.scaleX, 1.0f / clipUnit.scaleY));
            else
            {
                mMaterialPropertyBlock.SetVector("_MainTex_ST", new Vector4(1.0f / clipUnit.scaleX, 1.0f / clipUnit.scaleY, 1f, 1f));
                mMeshRender.SetPropertyBlock(mMaterialPropertyBlock);
            }
        }
        #endregion
    }

}
