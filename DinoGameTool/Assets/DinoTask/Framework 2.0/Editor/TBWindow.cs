using Dino_Core.Core;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dino_Core.Task
{
    public class TBWindow : EditorWindow {

        protected static bool _isInit = false;
        protected static Type[] _drivedClasses = ExtendLib.GetAllSubTypes(typeof(DBaseNodeEditor));
        protected static TBWindowRightClickMenu _windowClickMenu;
        protected static TBWindowToolbar _toolbarMenu;

        protected static DEditorNodes _nodesRouter;
        public static DEditorNodes NodesRouter
        {
            get
            {
                return _nodesRouter;
            }
        }

        protected static DBaseNodeEditor _foucsNode;
        public static DBaseNodeEditor FoucsNode
        {
            get
            {
                return _foucsNode;
            }
            set
            {
                _foucsNode = value;
            }
        }

        protected Rect _windowRect;
        protected Rect _scrollRect = new Rect(0, 0, 1000, 1000);
        protected Vector2 _scrollPos = Vector2.zero;
        protected int _windownID = 0;

        public static TBWindow _mainWindow;

        public void InitRouter()
        {
            //_nodesRouter = (DNodes)DXMLSerializer.DeSerializeXmlToObject(Application.dataPath + "/Resources/Levels/" + SceneManager.GetActiveScene().name + ".xml", typeof(DNodes), _drivedClasses);
            _nodesRouter = Resources.Load(DTaskEditorConst.Level_Path_Editor + SceneManager.GetActiveScene().name) as DEditorNodes;

            if (_nodesRouter == default(DEditorNodes))
            {
                _nodesRouter = ScriptableObject.CreateInstance("Dino_Core.Task.DEditorNodes") as DEditorNodes;
            }

            DBaseNodeEditor.AUTO_UniqueID = _nodesRouter.AutoID;
        }


        [MenuItem("DCore/TaskEditor")]
        private static void CreateWindow()
        {
            _mainWindow = (TBWindow)EditorWindow.GetWindow(typeof(TBWindow));
        }

        private void OnEnable()
        {
            Init();
            InitComponent();
            InitRouter();
        }
        private void Init()
        {
            hideFlags = HideFlags.HideAndDontSave;

            InitComponent();

            InitRouter();

            _isInit = true;
        }
        private void InitComponent()
        {
            _windowClickMenu = new TBWindowRightClickMenu(this);

            _toolbarMenu = new TBWindowToolbar(this);
        }
        private void InputHandled()
        {
            if (Event.current == null)
            {
                return;
            }
            InputMouse(Event.current);
            InputGlobleKeyborad(Event.current);
        }
        private void InputMouse(Event _currentEvent)
        {
            if (!InWindow(_currentEvent.mousePosition))
            {
                return;
            }

            if (!_currentEvent.isMouse)
            {
                return;
            }

            switch (_currentEvent.button)
            {
                case 0: InputMouseLeft(_currentEvent); break;
                case 1: InputMouseRight(_currentEvent); break;
                case 2:
                    break;
            }
        }
        private void InputMouseLeft(Event _currentEvent)
        {
            _foucsNode = IsNode(_currentEvent.mousePosition);

            if (_foucsNode == null)
            {
                return;
            }

            if (TBLineRender._startNode != null && TBLineRender._startNode != _foucsNode)
            {
                // 如果选中节点已经在这个节点的Nexts列表
                if (TBLineRender._startNode.Nexts.Contains(_foucsNode.NodeID))
                {
                    TBLineRender._startNode = null;
                    return;
                }

                // 如果这个节点已经在选中节点的Nexts列表
                if (_foucsNode.Nexts.Contains(TBLineRender._startNode.NodeID))
                {
                    TBLineRender._startNode = null;
                    return;
                }

                TBLineRender._startNode.Nexts.Add(_foucsNode.NodeID);
            }

            TBLineRender._startNode = null;
        }
        private void InputMouseRight(Event _currentEvent)
        {
            TBLineRender._startNode = null;

            if (IsNode(_currentEvent.mousePosition) == null)
            {
                _windowClickMenu.PaintComponent();
            }
        }
        private void InputGlobleKeyborad(Event _currentEvent)
        {
            switch (_currentEvent.keyCode)
            {
                case KeyCode.Delete:
                    if (_foucsNode == null)
                    {
                        break;
                    }
                    DeleteNode(_foucsNode);
                    break;
            }
        }

        private void CalculateScrollRect()
        {
            if (_nodesRouter.Count == 0)
            {
                return;
            }

            float _minPositionY = 0;
            float _maxPositionY = 0;

            for (int i = 0; i < _nodesRouter.Count; i++)
            {
                if (_nodesRouter[i] != null)
                {
                    if (_nodesRouter[i].NodeRect.y < _minPositionY)
                    {
                        _minPositionY = _nodesRouter[i].NodeRect.y;
                    }
                    else if (_nodesRouter[i].NodeRect.y > _maxPositionY)
                    {
                        _maxPositionY = _nodesRouter[i].NodeRect.y;
                    }
                }
            }

            _scrollRect.height = _maxPositionY - _minPositionY + DTaskEditorConst.NodeWindowSize.y + 50.0f;
        }
        private DBaseNodeEditor IsNode(Vector2 _mousePosition)
        {
            for (int i = 0; i < _nodesRouter.Count; i++)
            {
                if (_nodesRouter[i].NodeRect.x < _mousePosition.x &&
                    _nodesRouter[i].NodeRect.x + _nodesRouter[i].NodeRect.width > _mousePosition.x &&
                    _nodesRouter[i].NodeRect.y < _mousePosition.y &&
                    _nodesRouter[i].NodeRect.y + _nodesRouter[i].NodeRect.height > _mousePosition.y)
                {
                    return _nodesRouter[i];
                }
            }

            return null;
        }
        private bool InWindow(Vector2 _mousePosition)
        {
            if (_mousePosition.x > 0 &&
                _mousePosition.x < 0 + position.width &&
                _mousePosition.y > 0 &&
                _mousePosition.y < 0 + position.height)
            {
                return true;
            }
            return false;
        }
        private void DeleteNode(DBaseNodeEditor _node)
        {
            _foucsNode.Nexts.Clear();
            _nodesRouter.Remove(_foucsNode);
            _foucsNode.IsValid = false;
            _foucsNode = null;
        }

        private void OnGUI()
        {
            if (!_isInit)
            {
                Init();
            }

            _toolbarMenu.PaintComponent();

            InputHandled();

            TBLineRender.PreDrawLine(Event.current.mousePosition);

            GUILayout.BeginVertical();
            {
                CalculateScrollRect();

                _scrollPos = GUI.BeginScrollView(new Rect(0, 18, position.width, position.height), _scrollPos, _scrollRect, true, true);
                {
                    BeginWindows();
                    for (int i = 0; i < _nodesRouter.Count; i++)
                    {
                        if (_nodesRouter[i] != null)
                        {
                            _nodesRouter[i].NodeRect = GUI.Window(_nodesRouter[i].NodeID, _nodesRouter[i].NodeRect, _nodesRouter[i].DrawWindow, _nodesRouter[i].NodeName);
                            _nodesRouter[i].DrawBeziers();
                        }
                    }
                    EndWindows();
                }
                GUI.EndScrollView();
            }
            GUILayout.EndVertical();

            Repaint();
        }
        private void OnDestroy()
        {
            _mainWindow = null;
            _nodesRouter = null;
        }

    }
}