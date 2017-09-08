using UnityEngine;
using System.Collections;

/// <summary>
/// 摄像机基类
/// 提供摄像机行为
/// </summary>
public class BaseCamera : MonoBehaviour
{
    //摄像机最长视角
    public static float MAXDISTANCE = 30;
    //摄像机最近视角
    public static float MINDISTANCE = 1;
    //摄像机事件响应最长距离
    public static float EVENTCALLDISTANCE = 2000;

    //鼠标事件枚举
    public enum Mouse_Event
    {
        LEFT = 0,
        RIGHT = 1,
        MIDDLE = 2
    }
    //控件事件枚举
    public enum Translate_Type
    {
        MOVING = 0,
        ROTATION = 1,
        SCALE = 2,
    }
    //删除模式与创建模式
    public enum EditMode_Type
    { 
        CREATE = 0,
        DELETE,
    }

    public enum CustomPrefs_Type
    { 
        SCENECAMERA_TRANSLATE_SENSITIVITY = 0,
        SCENEEDIT_ROTATE_SENSITIVITY,
    }

    public static string[] CustomPrefs_Name = { "SCENECAMERA_TRANSLATE_SENSITIVITY",
                                              "SCENEEDIT_ROTATE_SENSITIVITY"};

    public static Vector2[] CustomPrefs_MinandMax = { new Vector2(1,4),
                                                      new Vector2(10,30)};


    /// <summary>
    /// 静态方法
    /// 射出射线返回所有碰撞物体
    /// </summary>
    /// <param name="_cmaEventCamera">事件摄像机</param>
    /// <param name="_fDistance">射线长度</param>
    /// <returns></returns>
    public static RaycastHit[] RaytoWorld(Camera _cmaEventCamera,float _fDistance)
    {
        //所有摄像机所碰撞到的物体
        RaycastHit[] _Hits;
        //发射射线
        _Hits = Physics.RaycastAll(_cmaEventCamera.transform.position, _cmaEventCamera.transform.forward, _fDistance);
        //返回
        return _Hits;
    }

    /// <summary>
    /// 指定摄像机与鼠标交互
    /// </summary>
    /// <param name="_cmaEventCamera">事件摄像机</param>
    /// <returns></returns>
    public static RaycastHit RaytoWorld(Camera _cmaEventCamera)
    {
        //所有摄像机所碰撞到的物体
        RaycastHit _Hit;
        //得到鼠标位置并计算方向
        Ray _ray = _cmaEventCamera.ScreenPointToRay(Input.mousePosition);
        //发射射线
        if(Physics.Raycast(_ray,out _Hit))
        {
            return _Hit;
        }
        //返回
        return _Hit;
    }

    /// <summary>
    /// 指定摄像机与鼠标交互
    /// </summary>
    /// <param name="_cmaEventCamera">事件摄像机</param>
    /// <returns></returns>
    public static Vector3 GetRayCastPoint(Camera _cmaEventCamera)
    {
        //所有摄像机所碰撞到的物体
        RaycastHit _Hit;
        //得到鼠标位置并计算方向
        Ray _ray = _cmaEventCamera.ScreenPointToRay(Input.mousePosition);
        //发射射线
        if (Physics.Raycast(_ray, out _Hit))
        {
            return _Hit.point;
        }
        //返回
        return new Vector3(0,0,0);
    }

    public static Vector3 GetRayCastPoint(Camera _cmaEventCamera,GameObject _target)
    {
        //目标碰撞盒子
        Collider _coll = _target.GetComponent<Collider>();
        //所有摄像机所碰撞到的物体
        RaycastHit _Hit;
        //得到鼠标位置并计算方向
        Ray _ray = _cmaEventCamera.ScreenPointToRay(Input.mousePosition);
        if (_coll.Raycast(_ray, out _Hit, EVENTCALLDISTANCE))
        {
            return _Hit.point;
        }
        return new Vector3(0,0,0);
    }
}
