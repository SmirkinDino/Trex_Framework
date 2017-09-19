using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Dino_Core.DinoUGUI
{
    [CustomEditor(typeof(TweenPosition))]
    public class TweenerPositionEditor : Editor
    {
        TweenPosition mTweener ;
        private Vector3 showPosition;
        private Vector3 hidePosition;
        private float during;

        private void OnEnable()
        {
            mTweener = (TweenPosition)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginVertical();
            GUILayout.Space(10);

            EditorGUILayout.Vector3Field("CurrentWorldPosition", mTweener.transform.position);
            mTweener.ShowPosition = EditorGUILayout.Vector3Field("ShowPosition", mTweener.ShowPosition);
            mTweener.HidePosition = EditorGUILayout.Vector3Field("HidePosition", mTweener.transform.position);
            mTweener.During = EditorGUILayout.FloatField("during",mTweener.During);
            GUILayout.EndVertical();

            // Apply the modify
            serializedObject.ApplyModifiedProperties();
        }

    }

}
