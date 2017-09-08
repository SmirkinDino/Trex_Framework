using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dino_Core.DinoNav2D
{
    public class Nav2DNode
    {
        public static int Node_UniqueID = 0;

        [SerializeField]
        public int NodeID
        {
            get; protected set;
        }

        [SerializeField]
        public Vector3 NodePosition
        {
            get; set;
        }

        [SerializeField]
        public List<Nav2DNode> VisibleNodes
        {
            get; set;
        }

        [SerializeField]
        public bool ReachAble;

        [SerializeField]
        public float G_Cost
        {
            get; set;
        }

        [SerializeField]
        public float H_Cost
        {
            get; set;
        }

        [SerializeField]
        public float F_Cost
        {
            get
            {
                return this.H_Cost + this.G_Cost;
            }
            set
            {

            }
        }

        [SerializeField]
        public Nav2DNode AdjNodeLeft
        {
            get; set;
        }

        [SerializeField]
        public Nav2DNode AdjNodeRight
        {
            get; set;
        }

        [SerializeField]
        public Nav2DNode PrevNode
        {
            get; set;
        }

        private int VisibleNodesCount_ExceptTarget = 0;

        public Nav2DNode(Vector3 _pos)
        {
            this.NodeID = Node_UniqueID++;
            this.NodePosition = _pos;
            this.G_Cost = 0.0f;
            this.H_Cost = 0.0f;
            this.F_Cost = 0.0f;
            this.VisibleNodes = new List<Nav2DNode>();

            this.AdjNodeLeft = null;
            this.AdjNodeRight = null;

            this.ReachAble = true;
        }

        /// <summary>
        /// 重新计算这个点相连的所有顶点
        /// </summary>
        /// <param name="_graph"></param>
        public void Calculate_LinkedNodes_All(Dictionary<int, Nav2DNode> _graph)
        {
            this.VisibleNodes.Clear();

            // 添加左节点
            this.VisibleNodes.Add(AdjNodeLeft);

            // 添加右节点
            this.VisibleNodes.Add(AdjNodeRight);

            foreach (Nav2DNode _node in _graph.Values)
            {
                // 当前节点不是它自己并且不是他的左右节点
                if(_node == this || _node == AdjNodeLeft || _node == AdjNodeRight)
                {
                    continue;
                }

                Vector3 _dir = _node.NodePosition - this.NodePosition;

                RaycastHit2D _hit = Physics2D.Raycast(this.NodePosition + _dir.normalized * 0.1f, _dir, _dir.magnitude - 0.2f, 1 << Nav2DConst.Non_Walkable_LayerID);

                if (!_hit)
                {
                    this.VisibleNodes.Add(_node);
                    //Debug.Log("     " + this.NodeID + "可以看到节点 -------- " + _node.NodeID);
                }
            }

            this.VisibleNodesCount_ExceptTarget = this.VisibleNodes.Count;
        }

        /// <summary>
        /// 只计算这个点和目标顶点的连接情况
        /// </summary>
        /// <param name="_targetNode"></param>
        public void Calculate_LinkedNodes_Partly(Nav2DNode _targetNode)
        {

            if (this.VisibleNodesCount_ExceptTarget < this.VisibleNodes.Count)
            {
                this.VisibleNodes.RemoveAt(this.VisibleNodes.Count - 1);
            }

            Vector3 _dir = this.NodePosition - _targetNode.NodePosition;

            if (_targetNode == this)
            {
                return;
            }

            if (!Physics2D.Raycast(_targetNode.NodePosition + _dir.normalized * 0.1f, _dir, _dir.magnitude - 0.2f, 1 << Nav2DConst.Non_Walkable_LayerID))
            {
                // 如果与目标点相连
                if (!this.VisibleNodes.Contains(_targetNode))
                {
                    this.VisibleNodes.Add(_targetNode);
                }
            }

        }

        public static void Reset_NodeUniqueID_StartFromZero()
        {
            Node_UniqueID = 0;
        }
    }
}