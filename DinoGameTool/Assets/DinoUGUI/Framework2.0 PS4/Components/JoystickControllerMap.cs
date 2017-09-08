using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Dino_Core.DinoUGUI
{
    /// <summary>
    /// 
    /// SmirkinDino 2017.05.11
    /// 
    /// </summary>
    public class JoystickControllerMap
    {
        public static readonly int MAX_LENGTH = 50;
        public static readonly int MAX_HEIGHT = 20;

        private JoystickControllerGroup[] _controllers;
        private JoystickController[] _controllersRestore;

        protected int _pointerHorizital = 0;
        protected int _pointerVertical = 0;

        protected int _prePointerHorizital = 0;
        protected int _prePointerVertical = 0;

        /// <summary>
        /// To save CG
        /// </summary>
        protected JoystickController _handledJoystick;

        /// <summary>
        /// Current
        /// </summary>
        public JoystickController Current
        {
            get
            {
                return _controllers[_pointerVertical][_pointerHorizital];
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                _prePointerHorizital = _pointerHorizital;
                _prePointerVertical = _pointerVertical;
                _pointerHorizital = value.LocationHorizontal;
                _pointerVertical = value.LocationVertical;
            }
        }

        /// <summary>
        /// Previous
        /// </summary>
        public JoystickController Previous
        {
            get
            {
                return _controllers[_prePointerVertical][_prePointerHorizital];
            }
            set
            {
                // Not impliment
            }
        }

        /// <summary>
        /// All Controllers
        /// </summary>
        public JoystickController[] AllControllers
        {
            get
            {
                return this._controllersRestore;
            }
            set
            {
                // Not impliment
            }
        }

        public static JoystickControllerMap create()
        {
            return new JoystickControllerMap();
        }

        private JoystickControllerMap()
        {
        }

        /// <summary>
        /// add by given loaction
        /// </summary>
        /// <param name="_hor"></param>
        /// <param name="_vet"></param>
        /// <param name="_controller"></param>
        public void Add(JoystickController _controller)
        {
            try
            {
                if (_controllers[_controller.LocationVertical][_controller.LocationHorizontal])
                {
                }
                _controllers[_controller.LocationVertical][_controller.LocationHorizontal] = _controller;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// remove by given loaction
        /// </summary>
        /// <param name="_hor"></param>
        /// <param name="_vet"></param>
        public void Remove(int _hor, int _vet)
        {
            try
            {
                _controllers[_vet][_hor] = null;
            }
            catch (Exception)
            {
            }
        }

        public void Remove(JoystickController _controller)
        {
            Remove(_controller.LocationVertical, _controller.LocationHorizontal);
        }

        /// <summary>
        /// reset
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < MAX_HEIGHT; i++)
            {
                for (int j = 0; j < MAX_LENGTH; j++)
                {
                    _controllers[i][j] = null;
                }
            }
            _pointerHorizital = 0;
            _pointerVertical = 0;
        }

        /// <summary>
        /// init
        /// </summary>
        /// <param name="_controllerSet"></param>
        public void Init(JoystickController[] _controllerSet)
        {
            if(_controllers != null) Reset();
            else _controllers = new JoystickControllerGroup[MAX_HEIGHT];

            _controllersRestore = _controllerSet;

            for (int i = 0; i < _controllers.Length; i++)
            {
                _controllers[i] = JoystickControllerGroup.create();
            }

            for (int i = 0; i < _controllerSet.Length; i++)
            {
                _controllers[_controllerSet[i].LocationVertical][_controllerSet[i].LocationHorizontal] = _controllerSet[i];
            }

            Current = (_controllersRestore != null && _controllersRestore.Length > 0) ? _controllersRestore[0] : null;
        }

        /// <summary>
        /// search down
        /// </summary>
        /// <returns></returns>
        public JoystickController SearchForDown()
        {
            int _vet = _pointerVertical;
            while (_vet < MAX_HEIGHT - 1)
            {
                _vet++;

                // 检查当前指针有没有
                _handledJoystick = _controllers[_vet][_pointerHorizital];
                if (_handledJoystick && _handledJoystick.Enabled)
                {
                    this.Current = _handledJoystick;
                    return _handledJoystick;
                }

                // 向左搜索
                _handledJoystick = doSearchLeft(_vet);

                // 如果向左没有搜索到就向右所搜
                if (_handledJoystick != null) return _handledJoystick;
                else _handledJoystick = doSearchRight(_vet);

                // 如果右边有则返回右边，没有则继续下一层
                if (_handledJoystick != null) return _handledJoystick;
            }

            return null;
        }

        /// <summary>
        /// search up
        /// </summary>
        /// <returns></returns>
        public JoystickController SearchForUp()
        {
            int _vet = _pointerVertical;
            while (_vet > 0)
            {
                _vet--;

                // 检查当前指针有没有
                _handledJoystick = _controllers[_vet][_pointerHorizital];
                if (_handledJoystick && _handledJoystick.Enabled)
                {
                    this.Current = _handledJoystick;
                    return _handledJoystick;
                }

                // 向左搜索
                _handledJoystick = doSearchLeft(_vet);

                // 如果向左没有搜索到就向右所搜
                if (_handledJoystick != null) return _handledJoystick;
                else _handledJoystick = doSearchRight(_vet);

                // 如果右边有则返回右边，没有则继续下一层
                if (_handledJoystick != null) return _handledJoystick;
            }

            _handledJoystick = null;
            return null;
        }

        /// <summary>
        /// search left
        /// </summary>
        /// <returns></returns>
        public JoystickController SearchForLeft()
        {
            return doSearchLeft(_pointerVertical);
        }

        /// <summary>
        /// search right
        /// </summary>
        /// <returns></returns>
        public JoystickController SearchForRight()
        {
            return doSearchRight(_pointerVertical);
        }

        /// <summary>
        /// find the first one which gameobject name is the given key
        /// </summary>
        /// <param name="_key"></param>
        /// <returns></returns>
        public JoystickController Find(string _key)
        {
            for (int i = 0; i < _controllers.Length; i++)
            {
                if(_controllers[i][_key]) return _controllers[i][_key];
            }
            return null;
        }

        private JoystickController doSearchLeft(int _vertical)
        {
            int _hor = _pointerHorizital;
            while (_hor > 0)
            {
                _hor--;
                if (_controllers[_vertical][_hor] && _controllers[_vertical][_hor].Enabled)
                {
#if DINO_DEBUG
                    this.LogEditorOnly(this.Current.LocationHorizontal + " " + this.Current.LocationVertical);
#endif
                    this.Current = _controllers[_vertical][_hor];
                    return _controllers[_vertical][_hor];
                }
            }
            return null;
        }

        private JoystickController doSearchRight(int _vertical)
        {
            int _hor = _pointerHorizital;
            while (_hor < MAX_LENGTH - 1)
            {
                _hor++;
                if (_controllers[_vertical][_hor] && _controllers[_vertical][_hor].Enabled)
                {
#if DINO_DEBUG
                    this.LogEditorOnly(this.Current.LocationHorizontal + " " + this.Current.LocationVertical);
#endif
                    this.Current = _controllers[_vertical][_hor];
                    return _controllers[_vertical][_hor];
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 
    /// SmirkinDino 2017.05.11
    /// 
    /// </summary>
    public class JoystickControllerGroup
    {
        protected JoystickController[] _group;

        public JoystickController this[int i]
        {
            get
            {
                return _group[i];
            }
            set
            {
                _group[i] = value;
            }
        }

        public JoystickController this[string _key]
        {
            get
            {
                for (int i = 0;i < _group.Length; i++)
                {
                    if (_group[i] && _group[i].gameObject.name.Equals(_key))
                    {
                        return _group[i];
                    }
                }
                return null;
            }
            set
            {
                // Not impliment
            }
        }

        public int Count
        {
            get
            {
                return _group.Length;
            }
            set
            {
                // Not impliment
            }
        }

        public static JoystickControllerGroup create()
        {
            return new JoystickControllerGroup();
        }

        private JoystickControllerGroup()
        {
            _group = new JoystickController[JoystickControllerMap.MAX_LENGTH];

            for (int i = 0; i < _group.Length; i++)
            {
                _group[i] = null;
            }
        }
    }

}