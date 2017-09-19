using Dino_Core.Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dino_Core.Task
{
    public class TBWindowRightClickMenu : TBGenericMenu
    {
        protected static readonly Dictionary<string, Type> _menuContentTable = new Dictionary<string, Type>()
        {
            {"Task/Normal", null },
            {"Task/Root", null },

            {"Event/Base",typeof(DEventNodeEditor)}
        };

        protected Dictionary<string, Type>.KeyCollection.Enumerator _handleEnumerator;

        public TBWindowRightClickMenu(TBWindow _window) : base(_window)
        {
            _menu = new GenericMenu();

            if (_menuContentTable.Keys == null)
            {
                ExtendLib.DLog("TaskEditor RightClick Menu", "Null Table");
            }

            _handleEnumerator = _menuContentTable.Keys.GetEnumerator();

            while (_handleEnumerator.MoveNext())
            {
                AddMenuItem(_handleEnumerator.Current);
            }
        }

        protected override void OnMenuSelected(string _message, Vector2 _mousePoint)
        {
            DBaseNodeEditor _node;

            try
            {
                _node = (DBaseNodeEditor)Activator.CreateInstance(_menuContentTable[_message]);
            }
            catch (Exception)
            {
                ExtendLib.DLog("TaskEditor", "Wrong Node Type");
                return;
            }

            _node.NodeRect = new Rect(_mousePoint, DTaskEditorConst.NodeWindowSize);
            TBWindow.NodesRouter.Add(_node);
        }
    }
}