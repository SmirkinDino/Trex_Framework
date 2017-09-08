using UnityEngine;
using System.Collections;
using System;

namespace Dino_Core.DinoUGUI
{
    /// <summary>
    /// 
    /// SmirkinDino 2017.05.11
    /// 
    /// </summary>
    public class JoystickController : UIComponet
    {
        [Tooltip("This is the Vertical Location of the controller, range [0-20]")]
        public int LocationVertical = 0;

        [Tooltip("This is the Horizontal Location of the controller, range [0-50]")]
        public int LocationHorizontal = 0;

        public bool Enabled { get; set; }

        // 委托回掉函数
        public Action<JoystickController> OnPressedEventHandler { get; set; }
        public Action<JoystickController> OnSelectedEventHandler { get; set; }
        public Action CancelSelectedEventHandler { get; set; }

        public override void Init()
        {
            base.Init();
            Enabled = true;
        }

        public void Select()
        {
            if (OnSelectedEventHandler != null)
            {
                OnSelectedEventHandler(this);
            }
            OnSelected();
        }
        public virtual void OnSelected()
        {
        }
        public void CancelSelect()
        {
            if (CancelSelectedEventHandler != null)
            {
                CancelSelectedEventHandler();
            }
            OnCancelSelected();
        }
        public virtual void OnCancelSelected()
        {
        }
        public void Press(JoystickController _sender)
        {
            if (OnPressedEventHandler != null)
            {
                OnPressedEventHandler(_sender);
                //OnPressed(_sender);
            }
            OnPressed(_sender);
        }
        public virtual void OnPressed(JoystickController _sender)
        {
        }

        public virtual void OnShow(float duration, float delay = 0)
        {
        }
        public virtual void OnHide(float duration, float delay = 0)
        {
        }
    }
}