using UnityEngine;
using System.Collections;

/// <summary>
/// this is the 2D Point Light Component
/// SmirkinDino 16.9.30
/// </summary>
public class PointLight2D : Base2DLight {

    void Awake()
    {
        // init light
        initLight();
    }

    void Update()
    {
        // if the light did not on , return
        if (!on)
        {
            mMeshFilter.gameObject.SetActive(false);
            return;
        }
        else if(!mMeshFilter.gameObject.activeSelf)
        {
            mMeshFilter.gameObject.SetActive(true);
        }

        // get the vertexBuufer if dynamic draw shadow
        GetVertexsBuffer();

        // light world poition
        Vector3 lightPoint = transform.position;

        // light world position2D
        Vector2 lightPoint2D = new Vector2(lightPoint.x, lightPoint.y);

        // light world position3D ,this was cut the z axis
        Vector3 lightPoint3D = new Vector3(lightPoint2D.x, lightPoint2D.y);

        // the vertexArray wait to be sorted
        ArrayList entityList = new ArrayList();

        for (int i = 0; i < mVertexSet.Length; i++)
        {
            // get current vertex data
            Vector2 vertex = new Vector2(mVertexSet[i].x, mVertexSet[i].y);

            // check whether the vertex is the edge or not
            // and cast ray and add sort Array
            // ..
            // ..
            // 
            RayEntity entity = DetectRaybyOffset(lightPoint3D, new Vector3(vertex.x, vertex.y), 0);
            RayEntity entityLeft = DetectRaybyOffset(lightPoint3D, new Vector3(vertex.x, vertex.y), -OFFSET_ANGLE);
            RayEntity entityRight = DetectRaybyOffset(lightPoint3D, new Vector3(vertex.x, vertex.y), OFFSET_ANGLE);

            entityList.Add(entity);
            entityList.Add(entityLeft);
            entityList.Add(entityRight);
        }

        // add corner
        AddCorner(entityList);

        // sort the vertexs
        entityList.Sort(mComaparer);

        // update mesh
        UpdateMesh(entityList,lightPoint3D,true);
    }
}
