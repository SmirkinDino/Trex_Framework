using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dino_Core.Core
{
    [CustomEditor(typeof(SpawnPoolConfig))]
    public class SpawnPoolConfigEditor : Editor
    {
        protected SpawnPoolConfig m_Pool;
        protected GUIStyle _style;
        protected List<bool> m_IsDrawPools;

        private void OnEnable()
        {
            m_Pool = target as SpawnPoolConfig;

            if(m_Pool.Pools == null)
            {
                m_Pool.Pools = new List<SpawnPool>();
            }

            if (m_IsDrawPools == null)
            {
                m_IsDrawPools = new List<bool>();

                for (int i = 0; i < m_Pool.Pools.Count; i++)
                {
                    m_IsDrawPools.Add(false);
                }
            }
        }

        // Update is called once per frame
        public override void OnInspectorGUI()
        {
            _style = new GUIStyle(EditorStyles.toolbarButton);

            _style.margin.top = 2;

            GUI.enabled = true;

            serializedObject.Update();

            // Title
            EditorGUILayout.PrefixLabel("DinoCore SpawnPool", EditorStyles.toolbarButton);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                // This is the plus button
                if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(25)))
                {
                    m_IsDrawPools.Add(false);
                    m_Pool.Pools.Add(new SpawnPool());
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10.0f);

            for (int i = 0; i < m_Pool.Pools.Count; i++)
            {
                if (m_Pool.Pools[i] == null)
                {
                    m_IsDrawPools.RemoveAt(i);
                    m_Pool.Pools.RemoveAt(i);
                    continue;
                }

                // skip first
                if (i > 0)
                {
                    GUILayout.Space(15.0f);
                }

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width(25)))
                    {
                        if (m_Pool.Pools.Count > 0)
                        {
                            m_IsDrawPools.RemoveAt(i);
                            m_Pool.Pools.RemoveAt(i);
                            return;
                        }
                    }

                    GUILayout.Space(12.0f);

                    // draw label
                    m_IsDrawPools[i] = EditorGUILayout.Foldout(m_IsDrawPools[i], string.Format("Pool Key : {0}", m_Pool.Pools[i].PoolName),true, EditorStyles.toolbarButton);
                }
                EditorGUILayout.EndHorizontal();

                if (m_IsDrawPools[i])
                {
                    // draw pool
                    DrawPool(m_Pool.Pools[i]);
                }
            }

            this.Repaint();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }

            // Apply the modify
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawPool(SpawnPool _pool)
        { 
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space();

                // Pool Name
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(20);
                    EditorGUILayout.LabelField("PoolKey :", GUILayout.Width(80));
                    _pool.PoolName = EditorGUILayout.TextField(_pool.PoolName, GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                // This is the Prefab List
                DrawPrefab(_pool);

                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(20);

                    // This is the plus button
                    if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(25)))
                    {
                        _pool.Add(new SpawnPrefab());
                    }
                }
                EditorGUILayout.EndHorizontal();

            }
            EditorGUILayout.EndVertical();

        }

        protected void DrawPrefab(SpawnPool _pool)
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                // button
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(20);
                    if (GUILayout.Button("-", _style, GUILayout.Width(25)))
                    {
                        if (i >= 0)
                        {
                            _pool.RemoveAt(i);
                            return;
                        }
                    }

                    EditorGUILayout.LabelField(string.Format("Element: {0}", i + 1), EditorStyles.toolbarButton, GUILayout.MaxWidth(80));

                    if (_pool[i].Resouces) EditorGUILayout.LabelField(string.Format("Key: {0}", _pool[i].Resouces.name), EditorStyles.toolbarButton, GUILayout.MaxWidth(300));
                    else EditorGUILayout.LabelField("Drag Prefab To Get The Key", EditorStyles.toolbarButton, GUILayout.MaxWidth(202));
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);

                // res
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(49);
                    EditorGUILayout.LabelField("Prefab", EditorStyles.toolbarButton,GUILayout.Width(80));
                    _pool[i].Resouces = EditorGUILayout.ObjectField(_pool[i].Resouces, typeof(Transform), false, GUILayout.MaxWidth(221)) as Transform;
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);

                // preload
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(49);
                    EditorGUILayout.LabelField("Preload", EditorStyles.toolbarButton, GUILayout.Width(80));
                    _pool[i].Preload = EditorGUILayout.IntField(_pool[i].Preload, GUILayout.MaxWidth(203));
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);

                // limit
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(49);
                    EditorGUILayout.LabelField("Limit", EditorStyles.toolbarButton, GUILayout.Width(80));
                    _pool[i].Limit = EditorGUILayout.IntField(_pool[i].Limit, GUILayout.MaxWidth(203));
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);

                // message
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(49);
                    EditorGUILayout.LabelField("Message", EditorStyles.toolbarButton, GUILayout.Width(80));
                    _pool[i].LogMessage = EditorGUILayout.TextField(_pool[i].LogMessage, GUILayout.MaxWidth(203));
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(15);
            }
        }
    }
}