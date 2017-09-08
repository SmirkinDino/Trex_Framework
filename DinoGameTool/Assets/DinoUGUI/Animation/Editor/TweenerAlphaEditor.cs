using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Dino_Core.DinoUGUI
{
    [CustomEditor(typeof(TweenAlpha))]
    public class TweenerAlphaEditor : Editor
    {
        TweenAlpha mTweener;
        private float from;
        private float to;
        private float during;

        private void OnEnable()
        {
            mTweener = (TweenAlpha)target;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            GUILayout.BeginVertical();
            GUILayout.Space(10);

            mTweener.To = EditorGUILayout.FloatField("To", mTweener.To);
            mTweener.From = EditorGUILayout.FloatField("From", mTweener.From);
            mTweener.During = EditorGUILayout.FloatField("During", mTweener.During);
            GUILayout.EndVertical();

            // Apply the modify
            serializedObject.ApplyModifiedProperties();
        }

    }

}
