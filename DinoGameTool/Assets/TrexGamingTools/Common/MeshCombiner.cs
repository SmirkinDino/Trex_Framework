using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// 请将这个脚本放在一个空物体上，会自动和并子物体网格
/// 
/// SmirkinDino 2017.05.24
/// </summary>
public class MeshCombiner : MonoBehaviour {

    public bool CombineCollider { get; set; }
    public bool AutoRemove { get; set; }

    private static MeshFilter[] SourceMeshFilters;
    private static CombineInstance[] SourceInstances;
    private static MeshRenderer[] SourceRenderers;
    private static List<Material> SourceMaterials = new List<Material>();

    private static MeshFilter ThisFilter;
    private static MeshRenderer ThisRenderer;

    public void Combine()
    {

        SourceMeshFilters = GetComponentsInChildren<MeshFilter>(); 
        SourceInstances = new CombineInstance[SourceMeshFilters.Length];

        SourceRenderers = GetComponentsInChildren<MeshRenderer>();
        SourceMaterials.Clear();

        for (int i = 0; i < SourceMeshFilters.Length; i++)
        {
            SourceMaterials.Add(SourceRenderers[i].sharedMaterial);

            SourceInstances[i].mesh = SourceMeshFilters[i].sharedMesh;
            SourceInstances[i].transform = transform.worldToLocalMatrix * SourceMeshFilters[i].transform.localToWorldMatrix;
            if (SourceMeshFilters[i].gameObject.name != gameObject.name)
            {
                //DestroyImmediate(SourceMeshFilters[i].gameObject);
                SourceMeshFilters[i].gameObject.SetActive(false);
            }
        }

        ThisFilter = gameObject.AddComponent<MeshFilter>();
        ThisRenderer = gameObject.AddComponent<MeshRenderer>();

        ThisFilter.sharedMesh = new Mesh();
        ThisFilter.mesh.CombineMeshes(SourceInstances, false);

        ThisRenderer.sharedMaterials = SourceMaterials.ToArray();

        if(CombineCollider) gameObject.AddComponent<MeshCollider>();

        transform.gameObject.SetActive(true);

        if(AutoRemove) DestroyImmediate(GetComponent<MeshCombiner>());
    }





}
