using UnityEngine;
using System.Collections;
using System;

public class RayEntity
{
    public static readonly Vector3 UP_TOWORDS = new Vector3(0,1,0);
    public static readonly Vector3 RIGHT_TOWARDS = new Vector3(1,0,0);
    public static readonly Vector3 TOP_TOWORADS = new Vector3(0,0,1);

    public static readonly Vector3 TOP_RIGHT = new Vector3(16, 9, 0);
    public static readonly Vector3 DOWN_RIGHT = new Vector3(16, -9, 0);
    public static readonly Vector3 DOWN_LEFT = new Vector3(-16, -9, 0);
    public static readonly Vector3 TOP_LEFT = new Vector3(-16, 9, 0);

    /// <summary>
    /// the vertex of the end of the ray
    /// </summary>
    public Vector3 vertex
    {
        get; private set;
    }

    /// <summary>
    /// this is the angle between UPTOWARDS and the Ray in 2D space
    /// </summary>
    public float angle
    {
        get; private set;
    }

    /// <summary>
    /// hit obj
    /// </summary>
    public RaycastHit2D hit
    {
        get; private set;
    }

    /// <summary>
    /// the create func
    /// </summary>
    /// <param name="_v">intersection</param>
    /// <param name="_m">light org</param>
    /// <param name="_hit">hit obj</param>
    public RayEntity(Vector3 _v, Vector3 _m, RaycastHit2D _hit)
    {
        vertex = _v;
        hit = _hit;
        Vector3 dir = _v - _m;

        if(Vector3.Cross(dir, UP_TOWORDS).z > 0)
        {
            this.angle = Vector3.Angle(dir.normalized, UP_TOWORDS);
        }
        else
        {
            this.angle = -1 * Vector3.Angle(dir.normalized, UP_TOWORDS);
        }
    }

    /// <summary>
    /// the create func
    /// </summary>
    /// <param name="_v"></param>
    /// <param name="_m"></param>
    /// <param name="_hit"></param>
    /// <param name="_relative"></param>
    public RayEntity(Vector3 _v,Vector3 _m,RaycastHit2D _hit,Vector3 _relative)
    {
        vertex = _v;
        hit = _hit;
        Vector3 dir = _v - _m;

        if (Vector3.Cross(dir, _relative).z > 0)
        {
            this.angle = Vector3.Angle(dir.normalized, _relative);
        }
        else
        {
            this.angle = -1 * Vector3.Angle(dir.normalized, _relative);
        }
    }
}
