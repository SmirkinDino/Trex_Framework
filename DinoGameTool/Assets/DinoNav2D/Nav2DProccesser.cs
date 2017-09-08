using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Dino_Core.DinoNav2D
{

    public class Nav2DProccesser 
    {
        private static Dictionary<int, Nav2DNode> mNode_Graph;
        private static bool DynamicGraph = false;

        private static List<Nav2DNode> mOpenSet = new List<Nav2DNode>();
        private static List<Nav2DNode> mCloseSet = new List<Nav2DNode>();

        public enum InitType
        {
            INIT,
            REFRESH
        }

        // 是否开启动态监测
        public static void OpenDynamic_Graph(bool _isOpen)
        {
            DynamicGraph = _isOpen;
        }

        // 初始化或者刷新网格
        public static void InitOrRefresh_Graph(InitType _type)
        {
            switch (_type)
            {
                case InitType.INIT:
                    _calculate_grid();
                    _refresh_grid_all();
                    break;
                case InitType.REFRESH:
                    _refresh_grid_all();
                    break;
            }
        }

        // 计算路径
        public static List<Nav2DNode> Calculate_Navigation(Vector3 _startPos,Vector3 _endPos)
        {
            // 初始化开表 和 闭表
            mOpenSet.Clear();
            mCloseSet.Clear();

            // 创建起始节点
            Nav2DNode _start = new Nav2DNode(_startPos);
            Nav2DNode _end = new Nav2DNode(_endPos);

            // 添加起始节点进网格,检查当前的位置是否在可行走的区域内，如果不在，则返回最近的可行走点作为路径返回
            List<Nav2DNode> _walkableCheck = _add_StartEnd_graph(_start, _end);
            if (_walkableCheck != null)
            {
                return _walkableCheck;
            }

            // 更新网格信息
            // 动态重绘，这个选项将重绘所有网格信息，消耗较大
            // 静态重绘，这个选项只会绘制有变化的网格信息，消耗较小
            if (DynamicGraph)
            {
                _refresh_grid_all();
            }
            else
            {
                _refresh_grid_partly(_end);
            }

            // 重新计算各个顶点的权值，权值分为三个分别是 估计值 实际值 最终评估值
            // 估计值：只与当前顶点到目标点的直接距离
            // 实际值：起始顶点通过寻路图到当前节点的路程
            // 评估值：估计值 加上 实际值
            _refresh_cost(_end);

            // 将起始顶点加入开表中
            mOpenSet.Add(_start);

            // 如果开表中还有顶点
            while (mOpenSet.Count > 0)
            {
                // 从开表中选择评估值最小的节点
                Nav2DNode _current = _min_fcostnode(mOpenSet);

                //Debug.Log("当前路径所在的节点为" + _current.NodeID + "------------------------" );

                // 如果没有最小值或者开表为空了，则空，表示寻路完成并且没有找到可行路径
                if (_current == null)
                {
                    //Debug.Log("没有有效的路径");
                    return null;
                }

                // ..
                // ..
                // ..
                // 如果当前顶点已连接顶点已连接终点，则返回路径
                if (_current.VisibleNodes.Contains(_end))
                {
                    //Debug.Log("-------------------------找到路径，最终节点为" + _current.NodeID);

                    // 将最终顶点的前置节点设置为当前顶点
                    _end.PrevNode = _current;

                    // 返回路径，并从图中清除此次寻路的起始顶点
                    mNode_Graph.Remove(_start.NodeID);
                    mNode_Graph.Remove(_end.NodeID);
                    return _retrieve_path(_end);
                }

                // 当前顶点考察完毕，把当前顶点加入闭表中
                mCloseSet.Add(_current);

                // 把当前顶点从开表中移除
                mOpenSet.Remove(_current);

                // 现在开始访问当前顶点的所有相连的顶点
                for (int i = 0; i < _current.VisibleNodes.Count; i++)
                {
                    // 如果这个顶点为空，则直接访问下一个顶点
                    // 如果这个顶点在闭表中，直接访问下一个顶点
                    if (_current.VisibleNodes[i] == null || mCloseSet.Contains(_current.VisibleNodes[i]))
                    {
                        //Debug.Log("查看子节点" + _current.VisibleNodes[i].NodeID + ">>>>>在闭表中或者这个节点为空");
                        continue;
                    }

                    // 如果这个顶点不合法或者不可到达
                    if (!_current.VisibleNodes[i].ReachAble)
                    {
                        continue;
                    }

                    // 通过当前节点路径到这个顶点的实际值
                    float _toChildGCost = _current.G_Cost + _calculate_cost(_current, _current.VisibleNodes[i]);

                    // 如果这个顶点在不在开表中
                    if (!mOpenSet.Contains(_current.VisibleNodes[i]))
                    {
                        // 设置这个顶点的实际值
                        _current.VisibleNodes[i].G_Cost = _toChildGCost;

                        // 设置这个顶点的前置节点为当前节点
                        _current.VisibleNodes[i].PrevNode = _current;

                        //Debug.Log("查看子节点" + _current.VisibleNodes[i].NodeID + ">>>>>不在开表中，加入开表>>>" + "设置Prev为" + _current.NodeID);
                        
                        // 把这个节点添加进开表中
                        mOpenSet.Add(_current.VisibleNodes[i]);
                    }
                    else
                    {
                        // 如果这个顶点在开表中，并且其实际值大于当前节点路径到这个顶点（没有当前路径优）
                        if (_current.VisibleNodes[i].G_Cost > _toChildGCost)
                        {
                            // 更新其路径，并把这个顶点的前置节点设置为当前节点
                            _current.VisibleNodes[i].G_Cost = _toChildGCost;
                            _current.VisibleNodes[i].PrevNode = _current;
                            //Debug.Log("查看子节点" + _current.VisibleNodes[i].NodeID + ">>>>>在开表中且优于当前路径");
                        }
                    }
                    
                }
            }

            //Debug.Log("没有找到路径");

            // 查看完所有顶点，并没有找到路径
            // 按道理来说程序是不会运行到这里的
            // ..
            mNode_Graph.Remove(_start.NodeID);
            mNode_Graph.Remove(_end.NodeID);

            return null;
        }

        // 得到当前的图信息
        public static Dictionary<int, Nav2DNode> GetGraph()
        {
            return mNode_Graph;
        }

        // 计算网格信息
        private static void _calculate_grid()
        {
            if (mNode_Graph == null)
            {
                mNode_Graph = new Dictionary<int, Nav2DNode>();
            }
            else
            {
                mNode_Graph.Clear();
            }

            Nav2DNode.Reset_NodeUniqueID_StartFromZero();

            Nav2DNonWalkable[] _areas = GameObject.FindObjectsOfType<Nav2DNonWalkable>();

            foreach (Nav2DNonWalkable _area in _areas)
            {
                _area.UpdateVertexBuffer();
                Vector3[] _vertexs = _area.GetVertexBuffer().ToArray();

                for (int i = 0; i < _vertexs.Length; i++)
                {
                    Nav2DNode _node = new Nav2DNode(_vertexs[i]);
                    mNode_Graph.Add(_node.NodeID, _node);

                    if (!_is_vertext_valid(_vertexs[i]))
                    {
                        _node.ReachAble = false;
                    }
                }

                // 分别将多边形左右节点加入可视列表
                for (int i = Nav2DNode.Node_UniqueID - _vertexs.Length; i < Nav2DNode.Node_UniqueID; i++ )
                {
                    if(i - 1 >= 0)
                        mNode_Graph[i].AdjNodeLeft = mNode_Graph[i - 1];
                    else
                        mNode_Graph[i].AdjNodeLeft = mNode_Graph[Nav2DNode.Node_UniqueID - 1];

                    if(i + 1 < Nav2DNode.Node_UniqueID)
                        mNode_Graph[i].AdjNodeRight = mNode_Graph[i + 1];
                    else
                        mNode_Graph[i].AdjNodeRight = mNode_Graph[Nav2DNode.Node_UniqueID - _vertexs.Length];
                }
            }
        }

        // 重绘网格结构，所有顶点都将重新计算，十分耗费资源
        private static void _refresh_grid_all()
        {
            foreach (Nav2DNode _node in mNode_Graph.Values)
            {
                _node.Calculate_LinkedNodes_All(GetGraph());
            }
        }

        // 重绘网格结构，只计算与目标的改变，节省资源
        private static void _refresh_grid_partly(Nav2DNode _target)
        {
            foreach (Nav2DNode _node in mNode_Graph.Values)
            {
                _node.Calculate_LinkedNodes_Partly(_target);
            }
        }

        // 重新计算各个顶点权值
        private static void _refresh_cost(Nav2DNode _targetNode)
        {
            foreach (Nav2DNode _node in mNode_Graph.Values)
            {
                _node.G_Cost = 0;
                _node.H_Cost = _calculate_cost(_node, _targetNode);
            }
        }

        // 计算两点之间的消耗
        private static float _calculate_cost(Nav2DNode _from,Nav2DNode _to)
        {
            return Vector3.Distance(_from.NodePosition,_to.NodePosition); 
        }

        // 计算顶点是否合法
        private static bool _is_vertext_valid(Vector3 _point)
        {
            if( Physics2D.OverlapPoint(_point , 1 << Nav2DConst.Walkable_LayerID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 增加开始和结束两个节点进图
        private static List<Nav2DNode> _add_StartEnd_graph(Nav2DNode _start, Nav2DNode _end)
        {
            // start
            // 如果起始节点在不可行走区域
            if (Physics2D.OverlapPoint(_start.NodePosition, 1 << Nav2DConst.Non_Walkable_LayerID))
            {

                List<Nav2DNode> _path = new List<Nav2DNode>();
                _path.Add(_min_disnode(_start));
                return _path;
            }

            _start.Calculate_LinkedNodes_All(GetGraph());
            mNode_Graph.Add(_start.NodeID, _start);

            // end
            _end.Calculate_LinkedNodes_All(GetGraph());
            mNode_Graph.Add(_end.NodeID, _end);

            return null;
        }

        // 得到路径
        private static List<Nav2DNode> _retrieve_path(Nav2DNode _node)
        {

            List<Nav2DNode> _resultSet = new List<Nav2DNode>();
            Nav2DNode _currentNode = _node;
            while (_currentNode.PrevNode != null)
            {
                _resultSet.Add(_currentNode);
                _currentNode = _currentNode.PrevNode;
            }

            // 结果倒叙
            _resultSet.InvertedOrder();

            // 返回结果列表
            return _resultSet;
        }

        // 得到下一个顶点
        private static Nav2DNode _min_fcostnode(List<Nav2DNode> _list)
        {
            if (_list == null && _list.Count == 0)
                return null;

            Nav2DNode _result = _list[0];

            for (int i = 1; i < _list.Count; i++)
            {
                if (_list[i].F_Cost < _result.F_Cost)
                {
                    _result = _list[i];
                }
            }
            return _result;
        }

        private static Nav2DNode _min_disnode(Nav2DNode _cur)
        {
            int _curID = 1;
            int _minID = 0;
            float _minDis = 0;

            _minDis = Vector3.Distance(mNode_Graph[0].NodePosition, _cur.NodePosition);

            while (_curID < mNode_Graph.Count)
            {
                float _temp = Vector3.Distance(mNode_Graph[_curID].NodePosition, _cur.NodePosition);
                if (_temp < _minDis)
                {
                    _minDis = _temp;
                    _minID = _curID;
                }
                _curID++;
            }

            return mNode_Graph[_minID];
        }

    }
}