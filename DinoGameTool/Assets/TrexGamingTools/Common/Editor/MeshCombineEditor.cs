using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshCombiner))]
[CanEditMultipleObjects]
public class MeshCombineEditor : Editor {

    protected MeshCombiner m_Combiner;

    private void OnEnable()
    {
        m_Combiner = target as MeshCombiner;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginVertical();
        {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);
                m_Combiner.AutoRemove = EditorGUILayout.Toggle( "Remove After Combine", m_Combiner.AutoRemove, GUILayout.Width(350));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);
                m_Combiner.CombineCollider = EditorGUILayout.Toggle( "Combine MeshCollider", m_Combiner.CombineCollider, GUILayout.Width(350));
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(100);
                if (GUILayout.Button("Combine", GUILayout.Width(80)))
                {
                    m_Combiner.Combine();
                }
            }
            GUILayout.EndHorizontal();

        }
        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
