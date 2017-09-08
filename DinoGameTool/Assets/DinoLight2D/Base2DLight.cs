using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// this is the base Class of SmirkinDino 2DLighting System
/// By SmirkinDino 16.9.30
/// 
/// </summary>
public class Base2DLight : MonoBehaviour {

    /// <summary>
    /// the range of the light
    /// </summary>
    [SerializeField]
    public float mRange = 20.0f;

    /// <summary>
    /// the intensity of the lige
    /// </summary>
    [SerializeField]
    public float mIntensity
    {
        get
        {
            return this.mIntensity;
        }
        set
        {
            this.mIntensity = Mathf.Clamp(value,MIN_INTENSITY,MAX_INTENSITY);
        }
    }

    /// <summary>
    /// The max intensity of the light
    /// </summary>
    [HideInInspector]
    public static readonly float MAX_INTENSITY = 1.0f;

    /// <summary>
    /// The min intensity of the light
    /// </summary>
    [HideInInspector]
    public static readonly float MIN_INTENSITY = 0.0f;
    
    /// <summary>
    /// The angle offset per line
    /// </summary>
    [HideInInspector]
    public static readonly float OFFSET_ANGLE = 0.02f;

    /// <summary>
    /// the obstacle layer of the light
    /// </summary>
    public LayerMask mObstacleLayers;

    /// <summary>
    /// the light mesh layer
    /// </summary>
    public bool mLightingMask = false;

    /// <summary>
    /// Do super fit
    /// </summary>
    public bool mSuperFit = false;

    /// <summary>
    /// breath or not
    /// </summary>
    private bool mBreath = true;

    /// <summary>
    /// current breath value
    /// </summary>
    private float mBreathValue = 1.0f;

    /// <summary>
    /// the breath buttom
    /// </summary>
    private static readonly float BREATH_BUTTOMVALUE = 0.8f;

    /// <summary>
    /// breath tick time
    /// </summary>
    private static readonly float BREATH_TICK = 0.002f;

    /// <summary>
    /// the fineness of the light
    /// </summary>
    public enum Light_Fineness
    {
        /// <summary>
        /// Low
        /// </summary>
        LOW = 20,

        /// <summary>
        /// Middle
        /// </summary>
        MIDDLE = 10,

        /// <summary>
        /// High
        /// </summary>
        HIGH = 5
    }

    /// <summary>
    /// The fineness
    /// </summary>
    public Light_Fineness mFineness = Light_Fineness.LOW;

    /// <summary>
    /// is the light on?
    /// </summary>
    public bool on = true;

    /// <summary>
    /// this is the set of the Vertexs need to be check
    /// </summary>
    protected Vector3[] mVertexSet;

    /// <summary>
    /// the draw mesh
    /// </summary>
    protected MeshFilter mMeshFilter;

    /// <summary>
    /// the renderer of the mesh
    /// </summary>
    protected MeshRenderer mRender;

    /// <summary>
    /// the light material
    /// </summary>
    public Material mLightingMaterial;

    /// <summary>
    /// mesh data
    /// </summary>
    protected Mesh mMesh;

    /// <summary>
    /// this is the sort func of the vertexSet
    /// </summary>
    [HideInInspector]
    protected RayComparer mComaparer;

    /// <summary>
    /// To save CG
    /// </summary>
    private RaycastHit2D mHit;
    private Quaternion mQuatToward;
    private Vector3[] mVertexBuffer;
    private RayEntity mHandledRayEntity;
    private Collider2D[] mColliderSet;

    private PolygonCollider2D mPc2D;
    private BoxCollider2D mBc2D;
    private CircleCollider2D mCc2D;
    private Vector2[] mPointsBuffer;

    private ArrayList mVertexArray = new ArrayList();

    private WaitForEndOfFrame mWaitFrameEnd = new WaitForEndOfFrame();

    /// <summary>
    /// Turn breath Light On?
    /// </summary>
    /// <param name="_value"></param>
    public void OpenBreath(bool _value)
    {
        mBreath = _value;
        if (mBreath)
        {
            StartCoroutine("_excuteBreath");
        }
    }

    /// <summary>
    /// Detect a line cast to the Obstacle in scene
    /// </summary>
    /// <param name="_org">start point</param>
    /// <param name="_target">target point</param>
    /// <param name="_angle">offset angle</param>
    /// <returns></returns>
    protected RayEntity DetectRaybyOffset(Vector3 _org, Vector3 _target, float _angle)
    {
        // calculate the quaternion of the offset
        mQuatToward = Quaternion.AngleAxis(_angle, RayEntity.TOP_TOWORADS);

        // calculate new dir
        Vector3 towards = mQuatToward * (_target - _org).normalized;

        // get the hit of the ray
        mHit = Physics2D.Raycast(new Vector2(_org.x, _org.y), new Vector2(towards.x, towards.y), mRange, mObstacleLayers.value);

        // create rayEntity struct and return
        RayEntity rayEntity;
        if (mHit)
            rayEntity = new RayEntity(mHit.point, _org, mHit);
        else
            // if not hitted ,return the range
            rayEntity = new RayEntity(_org + mRange * towards, _org, mHit);
            
        //Debug.DrawLine(_org, rayEntity.vertex);
        return rayEntity;
    }

    /// <summary>
    /// Detect a line cast to the Obstacle in scene
    /// </summary>
    /// <param name="_org">start point</param>
    /// <param name="_target">target point</param>
    /// <param name="_angle">offset angle</param>
    /// <returns></returns>
    protected RayEntity DetectRaybyOffset(Vector3 _org, Vector3 _target, float _angle, Vector3 _relative)
    {
        // calculate the quaternion of the offset
        mQuatToward = Quaternion.AngleAxis(_angle, RayEntity.TOP_TOWORADS);

        // calculate new dir
        Vector3 towards = mQuatToward * (_target - _org).normalized;

        // get the hit of the ray
        mHit = Physics2D.Raycast(new Vector2(_org.x, _org.y), new Vector2(towards.x, towards.y), mRange, mObstacleLayers.value);

        // create rayEntity struct and return
        RayEntity rayEntity;
        if (mHit)
            rayEntity = new RayEntity(mHit.point, _org, mHit, _relative);
        else
            // if not hitted ,return the range
            rayEntity = new RayEntity(_org + mRange * towards, _org, mHit, _relative);

        //Debug.DrawLine(_org, rayEntity.vertex);
        return rayEntity;
    }

    /// <summary>
    /// Get all vertexs of obstacles in the scene
    /// </summary>
    protected void GetVertexsBuffer()
    {
        // get all the collider2D info
        mColliderSet = GameObject.FindObjectsOfType(typeof(Collider2D)) as Collider2D[];

        // create a vertexBuffer
        mVertexArray.Clear();

        foreach (Collider2D col in mColliderSet)
        {
            if (col.GetType().Equals(typeof(BoxCollider2D)))
            {
                // this is the BoxCollider2D
                // get all the vertexs of the collider
                mBc2D = (BoxCollider2D)col;

                float halfSizeX = mBc2D.size.x / 2.0f;
                float halfSizeY = mBc2D.size.y / 2.0f;

                // the sequence of adding the vertex based on the compararer of the vertexs
                // top left
                mVertexArray.Add(mBc2D.transform.TransformPoint(new Vector3(mBc2D.offset.x - halfSizeX, mBc2D.offset.y + halfSizeY)));

                // buttom left
                mVertexArray.Add(mBc2D.transform.TransformPoint(new Vector3(mBc2D.offset.x - halfSizeX, mBc2D.offset.y - halfSizeY)));

                // buttom right
                mVertexArray.Add(mBc2D.transform.TransformPoint(new Vector3(mBc2D.offset.x + halfSizeX, mBc2D.offset.y - halfSizeY)));

                // top right
                mVertexArray.Add(mBc2D.transform.TransformPoint(new Vector3(mBc2D.offset.x + halfSizeX, mBc2D.offset.y + halfSizeY)));

            }
            else if (col.GetType().Equals(typeof(PolygonCollider2D)))
            {
                // this is the PolygonCollider2D
                // get all the vertexs of the collider
                mPc2D = (PolygonCollider2D)col;
                mPointsBuffer = mPc2D.points;
                foreach (Vector2 point in mPointsBuffer)
                {
                    // translate the mat to vec3
                    Vector3 v = new Vector3(point.x, point.y);

                    // translate
                    v = mPc2D.transform.TransformPoint(v);

                    mVertexArray.Add(v);
                }
            }
            else if (col.GetType().Equals(typeof(CircleCollider2D)))
            {
                // this is the CircleCollider2D
                // get all the vertexs of the collider
                mCc2D = (CircleCollider2D)col;

                //dir
                Vector3 dir = new Vector3((mCc2D.transform.position - transform.position).x, (mCc2D.transform.position - transform.position).y, 0);
                Vector3 dirNormal = dir.normalized;

                // the radius of the circle
                float radius = mCc2D.radius * mCc2D.transform.localScale.x;

                // distance
                float distance = Mathf.Sqrt(dir.sqrMagnitude + radius * radius);

                // the angle of the triangle
                float angle = Mathf.Asin(radius / dir.magnitude) * 180.0f / Mathf.PI;

                // add leftbounds
                Quaternion leftQuat = Quaternion.AngleAxis(-angle, RayEntity.TOP_TOWORADS);
                mVertexArray.Add(transform.position + leftQuat * dirNormal * distance);

                // add rightbounds
                Quaternion rightQuat = Quaternion.AngleAxis(angle, RayEntity.TOP_TOWORADS);
                mVertexArray.Add(transform.position + rightQuat * dirNormal * distance);
            }
        }

        // create return sets
        mVertexSet = null;
        mVertexSet = new Vector3[mVertexArray.Count];

        for(int i = 0; i < mVertexArray.Count; i++)
            mVertexSet[i] = (Vector3)mVertexArray[i];
    }

    /// <summary>
    /// init the light
    /// </summary>
    protected void initLight()
    {
        // create the Lighting Mesh
        GameObject mesh = new GameObject();
        mesh.name = "lightMesh2D - auto";
        mesh.transform.position = new Vector3(0,0,0);
        if (mLightingMask) mesh.layer = 10;

        // init render
        mRender = mesh.AddComponent<MeshRenderer>();

        mRender.material = mLightingMaterial;

        // init filter
        mMeshFilter = mesh.AddComponent<MeshFilter>();
        
        // init mesh
        mMesh = new Mesh();
        mMeshFilter.mesh = mMesh;
        

        // the comparer of the vertex
        mComaparer = new RayComparer();

        // turn on the breath light
        OpenBreath(true);
    }

    /// <summary>
    /// Update Mesh
    /// </summary>
    /// <param name="_vertexBuffer"></param>
    protected void UpdateMesh(ArrayList _entityList, Vector3 _lightPoint, bool _isCircle)
    {

        // this is the vertexSet about the poly mesh
        mVertexBuffer = new Vector3[_entityList.Count + 1];
        for (int i = 0; i < mVertexBuffer.Length - 1; i++)
        {
            mVertexBuffer[i] = ((RayEntity)_entityList[i]).vertex;
        }

        // add the center of the poly into set
        mVertexBuffer[mVertexBuffer.Length - 1] = _lightPoint;

        // list the triangles
        int[] newTriangles = new int[mVertexBuffer.Length * 3];

        // list
        for (int i = 0; i < mVertexBuffer.Length - 2; i++)
        {
            newTriangles[i * 3] = mVertexBuffer.Length - 1;
            newTriangles[i * 3 + 1] = i;
            newTriangles[i * 3 + 2] = i + 1;
        }

        // if the light is a point light
        if (_isCircle)
        {
            // list the triangles data
            newTriangles[(mVertexBuffer.Length - 1) * 3] = mVertexBuffer.Length - 1;
            newTriangles[(mVertexBuffer.Length - 1) * 3 + 1] = mVertexBuffer.Length - 2;
            newTriangles[(mVertexBuffer.Length - 1) * 3 + 2] = 0;
        }

        // clear mesh and update
        mMesh.Clear();
        mMesh.vertices = mVertexBuffer;
        mMesh.triangles = newTriangles;

        // Update the position.z of the polyMesh
        mMeshFilter.transform.position = new Vector3(mMeshFilter.transform.position.x, mMeshFilter.transform.position.y, transform.position.z);
    }

    /// <summary>
    /// add four corner of the camera
    /// </summary>
    /// <param name="vertexBuffer"></param>
    protected virtual void AddCorner(ArrayList _vertexBuffer)
    {
        // 3 each side, totally 3 * 4 + 4
        int fineness = 360 / (int)mFineness;

        // transform world position 2D
        Vector3 transPoint2D = new Vector3(transform.position.x, transform.position.y);

        // cast ray
        for(int i = 0; i < fineness / 2; i++)
        {
            _vertexBuffer.Add(DetectRaybyOffset(transPoint2D, transPoint2D + new Vector3(0, 1, 0) * mRange, i * (int)mFineness));
        }

        for(int i = 0;i < fineness / 2; i++)
        {
            _vertexBuffer.Add(DetectRaybyOffset(transPoint2D, transPoint2D + new Vector3(0, -1, 0) * mRange, i * (int)mFineness));
        }
    }

    /// <summary>
    /// the breath light excuter
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator _excuteBreath()
    {
        // 0 means up
        // 1 means down
        int _state = 0;
        while (mBreath)
        {
            if(_state == 0)
            {
                mBreathValue += BREATH_TICK;
            }
            else
            {
                mBreathValue -= BREATH_TICK;
            }

            if(mBreathValue <= BREATH_BUTTOMVALUE)
            {
                _state = 0;
            }
            else if(mBreathValue >= 1.0f)
            {
                _state = 1;
            }

            yield return mWaitFrameEnd;
        }
    }

    /// <summary>
    /// Update Sahder
    /// </summary>
    protected virtual void UpdateShader(Vector3 _point, float _intensity)
    {
        mRender.material.SetVector("_SourcePos", new Vector4(_point.x, _point.y, _point.z, 0));
        mRender.material.SetFloat("_LightIntensity", _intensity);
        mRender.material.SetFloat("_Breath", mBreathValue);
    }
}
