using Dino_Core.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Dino_Core.Task
{
    public class TBGenericMenu : ITBComponent
    {
        protected GenericMenu _menu;

        protected Vector2 _menuAnchor;

        public TBGenericMenu(TBWindow _window) : base(_window)
        {
            _menu = new GenericMenu();
            ParentWindow = _window;
        }

        public override void PaintComponent()
        {
            _menu.ShowAsContext();

            Event _currentEvent = Event.current;

            if (_currentEvent != null)
            {
                _menuAnchor = _currentEvent.mousePosition;
            }
        }

        protected void AddMenuItem(string _name)
        {
            _menu.AddItem(new UnityEngine.GUIContent(_name), false, MenuCallback, _name);
        }

        private void MenuCallback(object _selected)
        {
            OnMenuSelected(_selected.ToString(), _menuAnchor);
        }

        protected virtual void OnMenuSelected(string _message, Vector2 _mousePoint)
        {

        }
    }
}