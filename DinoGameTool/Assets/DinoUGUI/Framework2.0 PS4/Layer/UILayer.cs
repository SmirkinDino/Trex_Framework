using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Dino_Core.DinoUGUI
{
    public class UILayer : MonoBehaviour
    {
        [SerializeField]
        public bool AutoShow = false;
        public bool HideOnEnable = false;

        protected List<UIComponet> _components;
        public UIComponet this[string _key]
        {
            get
            {
                for (int i = 0; i < _components.Count; i++)
                {
                    if (_components[i].name.Equals(_key))
                    {
                        return _components[i];
                    }
                }

                return null;
            }
        }

        protected ControllerGroup _controllerManager;

        protected DinoTweener[] _tweeners;
        protected DinoTweener _tweenerSelf;
        protected float _transitionDuring = 0;
        protected bool _lock = false;
        protected bool _isShow = false;
        protected bool _enable = true;

        protected virtual void LayerActionInput() { }

        public virtual void Init()
        {
            _components = new List<UIComponet>(GetComponentsInChildren<UIComponet>());

            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Init();
            }

            _controllerManager = gameObject.AddComponent<ControllerGroup>();
            _controllerManager.Init();

            CalculateAnimation();

            if (HideOnEnable)
            {
                Hide();
                return;
            }

            if (AutoShow)
            {
                Show();
            }
        }
        public virtual void Show()
        {
            if (_lock)
            {
                return;
            }

            gameObject.SetActive(true);

            if(_tweeners != null && _tweeners.Length > 0)
            {
                for (int i = 0; i < _tweeners.Length; i++)
                {
                    _tweeners[i].PlayForward();
                }
            }

            if (_tweenerSelf) { _tweenerSelf.PlayForward(); }

            _isShow = true;
            Invoke("OnTransitionFinish", _transitionDuring);

            SetControllerEnable(true);
        }
        public virtual void Hide()
        {
            if (_lock)
            {
                return;
            }

            if (_tweeners != null && _tweeners.Length > 0)
            {
                for (int i = 0; i < _tweeners.Length; i++)
                {
                    _tweeners[i].PlayBack();
                }
            }

            _tweenerSelf.PlayBack();

            _isShow = false;
            Invoke("OnTransitionFinish", _transitionDuring);

            SetControllerEnable(false);
        }
        public virtual void UpdateUI()
        {
            for (int i = 0;i < _components.Count; i++)
            {
                _components[i].UpdateUI();
            }
        }
        
        public void SetControllerEnable(bool _isEnable)
        {
            if (_controllerManager)
            {
                _controllerManager.Enabled = _isEnable;
            }

            _enable = _isEnable;
        }

        public void RegisterOnPressListener(string _targetName, Action<JoystickController> _callback)
        {
            try
            {
                _controllerManager.GetControllerByName(_targetName).OnPressedEventHandler += _callback;
            }
            catch (NullReferenceException)
            {
            }
        }
        public void UnregisterOnPressListener(string _targetName, Action<JoystickController> _callback)
        {
            try
            {
                _controllerManager.GetControllerByName(_targetName).OnPressedEventHandler -= _callback;
            }
            catch (NullReferenceException)
            {
            }
        }
        public void RegisterOnSelectListener(string _targetName, Action<JoystickController> _callback)
        {
            try
            {
                _controllerManager.GetControllerByName(_targetName).OnSelectedEventHandler += _callback;
            }
            catch (NullReferenceException)
            {
            }
        }
        public void UnregisterOnSelectListener(string _targetName, Action<JoystickController> _callback)
        {
            try
            {
                _controllerManager.GetControllerByName(_targetName).OnSelectedEventHandler -= _callback;
            }
            catch (NullReferenceException)
            {
            }
        }

        private void Update()
        {
            if (_enable)
            {
                LayerActionInput();
            }
        }

        private void OnTransitionFinish()
        {
            Unlock();
            gameObject.SetActive(_isShow);
        }
        private void Unlock()
        {
            _lock = false;
        }
        private void CalculateAnimation()
        {
            _tweeners = GetComponentsInChildren<DinoTweener>();

            if (_tweeners != null && _tweeners.Length > 0)
            {
                for (int i = 0; i < _tweeners.Length; i++)
                {
                    _transitionDuring = _transitionDuring < _tweeners[i].During ? _tweeners[i].During : _transitionDuring;
                }
            }

            _tweenerSelf = GetComponent<DinoTweener>();

            if (_tweenerSelf)
            {
                _transitionDuring = Mathf.Max(_tweenerSelf.During, _transitionDuring);
            }

        }
    }
}