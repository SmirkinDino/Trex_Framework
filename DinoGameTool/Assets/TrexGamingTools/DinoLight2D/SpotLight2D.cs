using UnityEngine;
using System.Collections;

public class SpotLight2D : Base2DLight {

    /// <summary>
    /// the angle of the spotArea
    /// </summary>
    public float mSpotArea = 30.0f;

    /// <summary>
    /// sort relative
    /// </summary>
    private Vector3 mRelative;

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
        else if (!mMeshFilter.gameObject.activeSelf)
        {
            mMeshFilter.gameObject.SetActive(true);
        }

        // get the vertexBuufer if dynamic draw shadow
        GetVertexsBuffer();

        // calculate the relative
        mRelative = Quaternion.AngleAxis(-mSpotArea,RayEntity.TOP_TOWORADS) * transform.up;

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

            // if the light out of the area , the cut the light
            if (Vector3.Angle(new Vector3( mVertexSet[i].x, mVertexSet[i].y) - lightPoint3D, transform.up) >= mSpotArea)
                continue;

            // get current vertex data
            Vector2 vertex = new Vector2(mVertexSet[i].x, mVertexSet[i].y);

            // check whether the vertex is the edge or not
            // and cast ray and add sort Array
            // ..
            // ..
            // 
            RayEntity entity = DetectRaybyOffset(lightPoint3D, new Vector3(vertex.x, vertex.y), 0 , mRelative);
            RayEntity entityLeft = DetectRaybyOffset(lightPoint3D, new Vector3(vertex.x, vertex.y), -OFFSET_ANGLE, mRelative);
            RayEntity entityRight = DetectRaybyOffset(lightPoint3D, new Vector3(vertex.x, vertex.y), OFFSET_ANGLE, mRelative);

            entityList.Add(entity);
            entityList.Add(entityLeft);
            entityList.Add(entityRight);
        }

        // add corner
        this.AddCorner(entityList);

        // sort the vertexs
        entityList.Sort(mComaparer);

        // update mesh
        UpdateMesh(entityList, lightPoint3D,false);

        // Update Shader
        UpdateShader(lightPoint, mRange);
    }

    /// <summary>
    /// override the border info
    /// </summary>
    /// <param name="_vertexBuffer"></param>
    protected override void AddCorner(ArrayList _vertexBuffer)
    {
        // target position
        Vector3 tartget = transform.position + transform.up * mRange;

        if (mSuperFit)
        {
            // calculate the fineness of each step
            float fineness = (float)mFineness / 10.0f;

            // current step and angle
            float currentDegree = -mSpotArea + fineness;

            // cast ray
            while (currentDegree < mSpotArea)
            {
                RayEntity ray = DetectRaybyOffset(new Vector3(transform.position.x, transform.position.y), new Vector3(tartget.x, tartget.y), currentDegree, mRelative);
                _vertexBuffer.Add(ray);
                currentDegree += fineness;
            }
        }


        // Add the border of the light , this will low the mistake
        RayEntity downBorder = DetectRaybyOffset(new Vector3(transform.position.x, transform.position.y), new Vector3(tartget.x, tartget.y), -mSpotArea, mRelative);
        RayEntity upBorder = DetectRaybyOffset(new Vector3(transform.position.x, transform.position.y), new Vector3(tartget.x, tartget.y), mSpotArea, mRelative);
        _vertexBuffer.Add(downBorder);
        _vertexBuffer.Add(upBorder);
    }

    /// <summary>
    /// Update Sahder
    /// </summary>
    protected override void UpdateShader(Vector3 _point, float _intensity)
    {
        base.UpdateShader(_point, _intensity);
        mRender.material.SetVector("_SpotTowards", transform.up);
        mRender.material.SetFloat("_LightRangeT", Mathf.Sin(mSpotArea * Mathf.PI / 180.0f));
    }
}
