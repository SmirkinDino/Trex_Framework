using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestModel), true)]
public class TestModelBaseEditor : Editor
{
    protected TestModel _object;

    private void OnEnable()
    {
        _object = target as TestModel;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Rename", EditorStyles.toolbarButton))
            {
                RennameAsset();
            }

            GUILayout.Space(4);

            if (GUILayout.Button("Remove", EditorStyles.toolbarButton))
            {
                DeleteAsset();
            }
        }
        GUILayout.EndHorizontal();
    }

    private void RennameAsset()
    {
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(_object), _object.Name);

        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(_object));

        AssetDatabase.Refresh();
    }

    private void DeleteAsset()
    {
        DestroyImmediate(_object, true);

        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(_object));

        AssetDatabase.Refresh();
    }
}