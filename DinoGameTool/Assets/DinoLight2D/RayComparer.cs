using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The comparer of the vertexs
/// </summary>
public class RayComparer : IComparer {

    public int Compare(object x, object y)
    {
        RayEntity a = (RayEntity)x;
        RayEntity b = (RayEntity)y;

        if (a.angle > b.angle) return 1;
        else if(a.angle < b.angle) return -1;
        else return 0;
    }
}
