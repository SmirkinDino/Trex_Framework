using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace Dino_Core.Task
{
    public class DTaskEditorConst
    {
        public static readonly string Level_Path_Editor = "ConfigurationAsset/Levels/Editor/";
        public static readonly string Level_Path = "ConfigurationAsset/Levels/";
        public static readonly string Level_Path_Relative = "Assets/Resources/ConfigurationAsset/Levels/";

        public static readonly GUIStyle Style = EditorStyles.toolbarButton;
        public static readonly Vector2 NodeWindowSize = new Vector2(120, 150);
    }

    [System.Serializable]
    public class DBaseNodeEditor
    {
        public static int AUTO_UniqueID = 0;
        protected int _uniqueID = -1;

        // 这个标志用于表示这个节点是否已经被删除，如果被标记为true，将会在下一帧被删除
        public bool IsValid { get; set; }

        public string NodeName;
        public int NodeID;
        public Rect DRect;
        public Rect NodeRect
        {
            get
            {
                if (TBWindow._mainWindow)
                {
                    DRect.x = Mathf.Clamp(DRect.x, 0, TBWindow._mainWindow.position.width - DTaskEditorConst.NodeWindowSize.x - 15.0f);
                }
                //_nodeRect.y = Mathf.Clamp(_nodeRect.y, 0, TBWindow._mainWindow.position.height - DTaskEditorConst.NodeWindowSize.y);
                DRect.y = Mathf.Max(0, DRect.y);

                return this.DRect;
            }
            set
            {
                this.DRect = value;
            }
        }
        public List<int> Nexts;

        public DBaseNodeEditor()
        {
            Nexts = new List<int>();
            NodeID = AUTO_UniqueID++;
            IsValid = true;
        }

        public void DrawWindow(int i)
        {
            DrawBaseComponent();
            CheckValid();

            OnDrawWindow();

            GUI.DragWindow();
        }
        public void DrawBeziers()
        {
            if (Nexts.Count > 0)
            {
                for (int i = 0; i < Nexts.Count; i++)
                {
                    //TBLineRender.DrawBezier(LinkAnchor,
                    //    Nexts[i].LinkAnchor,
                    //    LinkAnchor + (Nexts[i].LinkAnchor - LinkAnchor) * TBLineRender._bezierLinkForce,
                    //    Nexts[i].LinkAnchor + (LinkAnchor - Nexts[i].LinkAnchor) * TBLineRender._bezierLinkForce);
                    TBLineRender.DrawLine(NodeRect.position + NodeRect.size / 2, TBWindow.NodesRouter.GetNodeByID(Nexts[i]).NodeRect.position + TBWindow.NodesRouter.GetNodeByID(Nexts[i]).NodeRect.size / 2);
                }
            }
        }
        protected virtual void OnDrawWindow()
        {
        }
        protected void CheckValid()
        {
            for (int i = 0; i < Nexts.Count; i++)
            {

                if (TBWindow.NodesRouter.GetNodeByID(Nexts[i]) == null)
                {
                    Nexts.RemoveAt(i);
                }

                if (!TBWindow.NodesRouter.GetNodeByID(Nexts[i]).IsValid)
                {
                    Nexts.RemoveAt(i);
                }
            }
        }
        protected void DrawBaseComponent()
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("CLEAR", EditorStyles.toolbarButton, GUILayout.Width(54.0f)))
                {
                    ClearNext();
                }

                if (GUILayout.Button("NEXT", EditorStyles.toolbarButton, GUILayout.Width(54.0f)))
                {
                    LinkNext();
                }
            }
            GUILayout.EndHorizontal();
        }
        private void LinkNext()
        {
            TBLineRender._startNode = this;
            TBLineRender._preDraw = true;
        }
        private void ClearNext()
        {
            Nexts.Clear();
        }
    }
}

