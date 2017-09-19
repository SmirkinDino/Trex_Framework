using System;
using UnityEngine;
using System.Collections;
using Dino_Core.DinoUGUI;

public class ControllerGroup : UIComponet
{
    /// <summary>
    /// 所有按钮的索引图
    /// </summary>
    public JoystickControllerMap ControllerMap { get; private set; }

    public bool Enabled { get { return _enabled; } set { this._enabled = value; } }
    public bool Locker { get; protected set; }

    protected bool _enabled = true;
    protected int _curBtnIndex;
    protected bool _canHandle = true;
    protected Vector2 _stickLocation = Vector2.zero;
    protected float _inputAngle = 0;

    public override void Init()
    {
        base.Init();

        if (ControllerMap == null)
        {
            ControllerMap = JoystickControllerMap.create();
        }

        ControllerMap.Init(GetComponentsInChildren<JoystickController>());
    }

    private void Update()
    {
        if (!_enabled)
        {
            return;
        }

        if (Locker)
        {
            return;
        }

        Debug.Log("please set input！");
        //_stickLocation.y = DLInputManager.Vertical(ActionCode.DVertical) - DLInputManager.Vertical(ActionCode.LVertical);
        //_stickLocation.x = DLInputManager.Vertical(ActionCode.DHorizontal) - DLInputManager.Vertical(ActionCode.LHorizontal);

        if (_stickLocation.sqrMagnitude > 0.2f && _canHandle)
        {
            Locker = true;

            _canHandle = false;

            StartCoroutine(UnLock(0.1f));

            handleInput();

            ChangeButton();
        }

        //if (DLInputManager.GetButtonDown(ActionCode.Cross))
        //{
        //    if (ControllerMap.Current != null)
        //    {
        //        ControllerMap.Current.Press(ControllerMap.Current);
        //    }
        //}

        OnInputDetection();
    }

    /// <summary>
    /// 切换按钮
    /// </summary>
    private void ChangeButton()
    {
        if (ControllerMap.Previous != null)
        {
            ControllerMap.Previous.CancelSelect();
        }

        if (ControllerMap.Current != null)
        {
            ControllerMap.Current.Select();
        }
    }

    private void handleInput()
    {
        _inputAngle = Vector2.Angle(Vector2.up, _stickLocation);

        // up
        if (_inputAngle < 45.0f)
        {
            ControllerMap.SearchForUp();
        }
        // left and right
        else if (_inputAngle > 45.0f && _inputAngle < 135.0f)
        {
            // left
            if (_stickLocation.x > 0) ControllerMap.SearchForLeft();
            // right
            else ControllerMap.SearchForRight();
        }
        // down
        else
        {
            ControllerMap.SearchForDown();
        }
    }

    public void RegisterOnPressListener(string _targetKey, Action<JoystickController> _callback)
    {
        try
        {
            GetControllerByName(_targetKey).OnPressedEventHandler += _callback;
        }
        catch (NullReferenceException)
        {
        }
    }

    public void UnregisterOnPressListener(string _targetKey, Action<JoystickController> _callback)
    {
        try
        {
            GetControllerByName(_targetKey).OnPressedEventHandler -= _callback;
        }
        catch (NullReferenceException)
        {
        }
    }

    protected IEnumerator UnLock(float waitTime)
    {
        // TODO: 可以的话这里的逻辑可以放到DOTWEEN的回调里面，参看DOTween API
        // ..
        // ..
        // ..
        // ..
        yield return new WaitForSecondsRealtime(waitTime);

        Locker = false;

        SetCanhandle();

        StopAllCoroutines();
    }

    protected void SetCanhandle()
    {
        _canHandle = true;
    }

    protected virtual void OnInputDetection() { }

    /// <summary>
    /// 显示
    /// </summary>
    public virtual void OnShow(float duration)
    {
        Locker = true;

        gameObject.SetActive(true);

        // 显示按钮
        for (int i = 0; i < ControllerMap.AllControllers.Length; i++)
        {
            ControllerMap.AllControllers[i].OnShow(0.5f, Mathf.Lerp(0, duration - 0.5f, (i + 1) / (float)ControllerMap.AllControllers.Length));
        }

        // 设置默认选择的按钮
        if (ControllerMap.Current != null)
        {
            ControllerMap.Current.Select();
        }

        Invoke("ShowFinishCallback", duration);
    }

    /// <summary>
    /// 显示完成后调用
    /// </summary>
    protected virtual void OnShowOverCallback()
    {
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    public virtual void OnHide(float duration)
    {
        Locker = true;

        for (int i = 0; i < ControllerMap.AllControllers.Length; i++)
        {
            ControllerMap.AllControllers[i].OnHide(0.5f, Mathf.Lerp(0, duration - 0.5f, (i + 1) / (float)ControllerMap.AllControllers.Length));
        }

        Invoke("HideFinishCallback", duration);
    }

    /// <summary>
    /// 隐藏完成后调用
    /// </summary>
    protected virtual void OnHideOverCallback()
    {
    }

    /// <summary>
    /// 通过名字获取到 Button
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public JoystickController GetControllerByName(string name)
    {
        return ControllerMap.Find(name);
    }

    /// <summary>
    /// 取消激活一个按钮组
    /// </summary>
    /// <param name="btns"></param>
    protected void DisabledControllers(JoystickController[] btns)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            ControllerMap.Find(btns[i].gameObject.name).Enabled = false;
        }
    }

    /// <summary>
    /// 激活一个按钮集合
    /// </summary>
    /// <param name="btns"></param>
    protected void EnabledControllers(JoystickController[] btns)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            ControllerMap.Find(btns[i].gameObject.name).Enabled = true;
        }
    }

    protected void ShowFinishCallback()
    {
        Locker = false;
        OnShowOverCallback();
    }
    protected void HideFinishCallback()
    {
        Locker = false;
        gameObject.SetActive(false);
        OnHideOverCallback();
    }
}
