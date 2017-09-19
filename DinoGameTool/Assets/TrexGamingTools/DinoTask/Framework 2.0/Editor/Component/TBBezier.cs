using UnityEditor;
using UnityEngine;

namespace Dino_Core.Task
{
    public class TBLineRender
    {
        public static DBaseNodeEditor _startNode;
        public static bool _preDraw = false;
        public static Vector3 _mousePoint;

        public static readonly Color _bezierColor = new Color(0.2f, 0.8f, 0.2f,1.0f);
        public static readonly float _bezierWidth = 5f;
        public static readonly float _bezierLinkForce = 50.0f;

        public static void DrawLine(Vector3 _startPoint, Vector3 _endPoint)
        {
            Handles.color = _bezierColor;
            Handles.DrawAAPolyLine(_bezierWidth, new Vector3[] { _startPoint, _endPoint });
        }

        public static void PreDrawLine(Vector2 _mouse)
        {
            if (_preDraw == false || _startNode == null)
            {
                return;
            }

            _mousePoint = _mouse;

            DrawLine(_startNode.NodeRect.position + _startNode.NodeRect.size / 2, _mouse);
        }

        public static void DrawBezier(Vector3 _startPoint, Vector3 _endPoint, Vector3 _startTan, Vector3 _endTan)
        {
            Handles.DrawBezier(_startPoint, _endPoint, _startTan, _endTan, _bezierColor, null, _bezierWidth);
        } 

        public static void PreDrawBezier(Vector2 _mouse)
        {
            //if (_preDraw == false || _startNode == null)
            //{
            //    return;
            //}

            //_mousePoint = _mouse;

            //DrawBezier(_startNode.NodeRect.position + _startNode.NodeRect.size / 2, 
            //    _mouse,
            //    _startNode.NodeRect.position + _startNode.NodeRect.size / 2 + (_mousePoint - new Vector3(_startNode.NodeRect.position + _startNode.NodeRect.size / 2)) * _bezierLinkForce,
            //    new Vector3(_mouse.x, _mouse.y, 0) + (_startNode.LinkAnchor - _mousePoint) * _bezierLinkForce);
        }
    }
}