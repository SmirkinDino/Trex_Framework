using UnityEngine;
using System.Collections;


/// <summary>
/// 场景摄像机控件
/// </summary>
[AddComponentMenu("Mananger/Camera/ViewManager")]
public class SceneCameraManager : BaseCamera
{
    #region Var

    /// <summary>
    /// 灵敏度
    /// </summary>
    private float sensitivity
    {
        get { return this.m_fSensitivity; }
        set
        {
            this.m_fSensitivity = value;
            if (value > BaseCamera.CustomPrefs_MinandMax[(int)BaseCamera.CustomPrefs_Type.SCENECAMERA_TRANSLATE_SENSITIVITY].x)
                this.m_fSensitivity = BaseCamera.CustomPrefs_MinandMax[(int)BaseCamera.CustomPrefs_Type.SCENECAMERA_TRANSLATE_SENSITIVITY].x;
            if (value < BaseCamera.CustomPrefs_MinandMax[(int)BaseCamera.CustomPrefs_Type.SCENECAMERA_TRANSLATE_SENSITIVITY].y)
                this.m_fSensitivity = BaseCamera.CustomPrefs_MinandMax[(int)BaseCamera.CustomPrefs_Type.SCENECAMERA_TRANSLATE_SENSITIVITY].y;
            
            //存入预设
            PlayerPrefs.SetFloat(BaseCamera.CustomPrefs_Name[(int)BaseCamera.CustomPrefs_Type.SCENECAMERA_TRANSLATE_SENSITIVITY], m_fSensitivity);
            
        }
    }

    //摄像机旋转参数
    private const float m_fHorizontalSpeed = 3.2f;
    private const float m_fVerticalSpeed = 1.8f;
    private float m_fSensitivity;

    //这两个值始终记录摄像机的欧拉角参数
    private float m_fCurrentAxisX = 0;
    private float m_fCurrentAxisY = 0;
    private float m_fTargetAxisX = 0;    //旋转平滑辅助参数
    private float m_fTargetAxisY = 0;    //旋转平滑辅助参数
    private float m_fSmoothRate = 10;    //旋转平滑速度

    //这个是记录相机的目标偏移量
    private float m_fTargetOffsetX = 0;
    private float m_fTargetOffsetY = 0;

    //这个值记录摄像机相对于目标点的距离
    private float m_fDistance = 10f;
    private float m_fTargetDistance = 10f;
    //摄像机观察目标点
    private Vector3 m_vec3TargetPoint = new Vector3(0, 0, 0);


    public bool isRotate
    {
        get; set;
    } 
    public bool isMove
    {
        get; set;
    }
    public bool isRoll
    {
        get; set;
    }

    #endregion

    void Start ()
    {
        
        UpdateData();
	}

    void Update()
    {
        //旋转
        CameraRotate();
        //移动
        CameraMove();
        //缩放
        CameraRoll();
    }

    /// <summary>
    /// 以目标为中心旋转
    /// </summary>
    private void CameraRotate()
    {   
        //判断是否旋转
        if (Input.GetMouseButton((int)BaseCamera.Mouse_Event.RIGHT))
        {
            //计算X方向的角度变化
            //真实世界的坐标与数遍变化X轴相反
            //所以为减号
            m_fTargetAxisX -= m_fHorizontalSpeed * Input.GetAxis("Mouse Y") * m_fSensitivity;

            //计算Y方向的角度变化
            m_fTargetAxisY += m_fVerticalSpeed * Input.GetAxis("Mouse X") * m_fSensitivity;

        }

        //差值终点
        if (m_fTargetAxisX - m_fCurrentAxisX < 0.01f && m_fTargetAxisX - m_fCurrentAxisX > -0.01f)
            if (m_fTargetAxisY - m_fCurrentAxisY < 0.01f && m_fTargetAxisY - m_fCurrentAxisY > -0.01f)
                return;

        //差值计算新的角度
        m_fCurrentAxisX += (m_fTargetAxisX - m_fCurrentAxisX) / m_fSmoothRate;
        m_fCurrentAxisY += (m_fTargetAxisY - m_fCurrentAxisY) / m_fSmoothRate;

        //当前的角度四元数
        Quaternion _Rotation = Quaternion.Euler(m_fCurrentAxisX, m_fCurrentAxisY, 0);

        //变换位置
        transform.position = _Rotation * new Vector3(0, 0, -m_fDistance) + m_vec3TargetPoint;

        //变换角度
        transform.rotation = _Rotation;
    }

    /// <summary>
    /// 摄像机平移
    /// </summary>
    private void CameraMove()
    {
        //是否按下鼠标中键
        if (Input.GetMouseButton((int)BaseCamera.Mouse_Event.MIDDLE))
        {
            //得到偏移量
            m_fTargetOffsetX += Input.GetAxis("Mouse X") * 0.2f;
            m_fTargetOffsetY += Input.GetAxis("Mouse Y") * 0.2f;
        }
        
        //得到偏移量
        float _fMouseAxisX = m_fTargetOffsetX / m_fSmoothRate;
        float _fMouseAxisY = m_fTargetOffsetY / m_fSmoothRate;

        //更新目标偏移
        m_fTargetOffsetX *= (m_fSmoothRate - 1) / m_fSmoothRate;
        m_fTargetOffsetY *= (m_fSmoothRate - 1) / m_fSmoothRate;

        //差值终点
        if(m_fTargetOffsetX > 0.01f || m_fTargetOffsetX < -0.01f)
            if (m_fTargetOffsetY < 0.01f || m_fTargetOffsetY > -0.01f)
            {
                //得到方向
                Vector3 _vec3Direction = Vector3.Cross(transform.forward, transform.up).normalized;

                //移动相机
                transform.position += _vec3Direction * _fMouseAxisX * m_fSensitivity;
                transform.position -= transform.up * _fMouseAxisY * m_fSensitivity;
            }

        /// <summary>
        /// 设置相机目标点
        /// 
        /// 若在摄像机当前距离内射线碰撞到地面
        /// 则将目标点设置为射线与地面的接触位置
        /// 如果没有碰撞到物体
        /// 则将目标点设置为相机Forward方向前进相机距离
        /// </summary>
        RaycastHit[] _Hits = BaseCamera.RaytoWorld(Camera.main, BaseCamera.MAXDISTANCE);
        if (_Hits.Length <= 0)
        {
            //移动目标点到指定位置
            m_vec3TargetPoint = transform.rotation * new Vector3(0, 0, m_fDistance) + transform.position;
            return;
        }
        else
        { 
            RaycastHit _Hit = _Hits[0];
            //得到碰撞点
            m_vec3TargetPoint = _Hit.point;
            //重新计算距离
            m_fDistance = Vector3.Distance(m_vec3TargetPoint,transform.position);
        }
    }

    /// <summary>
    /// 摄像机缩放
    /// </summary>
    private void CameraRoll()
    {
        if (m_fTargetDistance - m_fDistance > 0.1f ||
            m_fTargetDistance - m_fDistance < -0.1f)
        {
            //先做移动
            m_fDistance += (m_fTargetDistance - m_fDistance) / m_fSmoothRate;
            //移动摄像机与目标点的距离
            transform.position = m_vec3TargetPoint + transform.rotation * new Vector3(0, 0, -m_fDistance);
        }
        //判断滚轮是否
        //若是则刷新新的目标值，若不是则return
        if (Input.GetAxis("Mouse ScrollWheel") == 0)
            return;

        //当前移动的值
        float _fOffset = -Input.GetAxis("Mouse ScrollWheel") * m_fSensitivity * 5.0f;

        //计算摄像机当前距离
        m_fTargetDistance += _fOffset;
        m_fTargetDistance = Mathf.Clamp(m_fTargetDistance, BaseCamera.MINDISTANCE, BaseCamera.MAXDISTANCE);
    }

    /// <summary>
    /// 欧拉角裁剪
    /// </summary>
    /// <param name="_fAngle">目标欧拉角</param>
    /// <returns></returns>
    private float ClampAngle(float _fAngle)
    {
        if (_fAngle < -360)
            _fAngle += 360;
        if (_fAngle > 360)
            _fAngle -= -360;
        return _fAngle;
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    private void UpdateData()
    {

        //摄像机数据初始化
        m_fCurrentAxisX = transform.eulerAngles.y;
        m_fCurrentAxisY = transform.eulerAngles.x;

        //此时相机的位置
        transform.position = Quaternion.Euler(new Vector3(m_fCurrentAxisY, m_fCurrentAxisX, 0)) * new Vector3(0, 0, m_fDistance) + m_vec3TargetPoint;
        transform.rotation = Quaternion.Euler(new Vector3(m_fCurrentAxisY, m_fCurrentAxisX, 0));

        //一些预设需要存入
        if (PlayerPrefs.HasKey(BaseCamera.CustomPrefs_Name[(int)BaseCamera.CustomPrefs_Type.SCENECAMERA_TRANSLATE_SENSITIVITY]))
        {
            m_fSensitivity = PlayerPrefs.GetFloat(BaseCamera.CustomPrefs_Name[(int)BaseCamera.CustomPrefs_Type.SCENECAMERA_TRANSLATE_SENSITIVITY]);
        }
        else
        {
            m_fSensitivity = 2;
            PlayerPrefs.SetFloat(BaseCamera.CustomPrefs_Name[(int)BaseCamera.CustomPrefs_Type.SCENECAMERA_TRANSLATE_SENSITIVITY],m_fSensitivity);
        }
    }

    /// <summary>
    /// 用户设置更新
    /// </summary>
    /// <param name="_sensitivity"></param>
    public void UpdateOption(float _sensitivity)
    {
        sensitivity = _sensitivity;
    }
}
