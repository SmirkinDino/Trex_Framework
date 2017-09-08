using Dino_Core.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestContainer), true)]
public class TestContainerBaseEditor : Editor {

    protected TestContainer _container;

    private void OnEnable()
    {
        _container = target as TestContainer;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(4);

        if (_container._model == null)
        {
            EditorGUILayout.HelpBox("You should generate a default config for this entity!",MessageType.Warning);
        }

        if (_container._key.Equals(string.Empty))
        {
            EditorGUILayout.HelpBox("You should enter a default key!", MessageType.Warning);
        }

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Generate Model File", EditorStyles.toolbarButton))
            {
                GenerateConfigurations();
            }

            GUILayout.Space(4);

            if (GUILayout.Button("Remove All", EditorStyles.toolbarButton))
            {
                RemoveAllAsset();
            }
        }
        GUILayout.EndHorizontal();

        EditorUtility.SetDirty(target);
        Repaint();
    }

    private void RemoveAllAsset()
    {
        if (EditorUtility.DisplayDialog("Destroy Assets", "Are you sure you want to destroy those objects?", "Yes", "No"))
        {
            Object[] _objs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(_container));
            for (int i = 0; i < _objs.Length; i++)
            {
                if (AssetDatabase.IsMainAsset(_objs[i]) || _objs[i] is GameObject || _objs[i] is Component)
                {
                    continue;
                }
                else
                {
                    DestroyImmediate(_objs[i], true);
                }
            }
        }

        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(_container));
        AssetDatabase.Refresh();
    }
    private void GenerateConfigurations()
    {
        _container._model = ScriptableObject.CreateInstance<TestModel>();

        _container._model.name = _container._key;

        AssetDatabase.AddObjectToAsset(_container._model, _container);

        AssetDatabase.SaveAssets();
    }
}


