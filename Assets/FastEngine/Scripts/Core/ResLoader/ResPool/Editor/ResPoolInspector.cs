using System.Collections.Generic;
using UnityEditor;

namespace FastEngine.Core
{
    [CustomEditor(typeof(ResPool))]
    public class ResPoolInspector : UnityEditor.Editor
    {
        private ResPool Target;

        private List<Res> poolList;
        void OnEnable()
        {
            Target = target as ResPool;
            poolList = Target.GetPoolList();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < poolList.Count; i++)
            {
                var res = poolList[i];

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("assetName: " + res.assetName);
                EditorGUILayout.LabelField("bundleName: " + res.bundleName);
                EditorGUILayout.LabelField("refCount: " + res.refCount.ToString());
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();
        }
    }
}