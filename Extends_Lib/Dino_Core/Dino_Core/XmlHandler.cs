using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace Dino_Core
{

    /// <summary>
    /// 封装 XML 操作类
    /// </summary>
    public static class XmlHandler
    {
        /// <summary>
        /// 创建 xml 文件并添加内容到指定节点
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static bool AddNodesToFile(string _path, string _rootNodeName, NodeStruct[] _nodeList)
        {
            XmlDocument _xmlDoc = new XmlDocument();
            if (!File.Exists(_path))
            {
                try
                {
                    // 创建根节点
                    XmlElement _rootNode = _xmlDoc.CreateElement("Data-List");
                    _xmlDoc.AppendChild(_rootNode);
                    // 创建用户所要求的父节点
                    XmlElement _userRootNode = _xmlDoc.CreateElement(_rootNodeName);
                    _rootNode.AppendChild(_userRootNode);

                    for (int i = 0; i < _nodeList.Length; i++)
                        AddChildtoNode(_xmlDoc, _userRootNode, _nodeList[i]);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    return false;
                }
            }
            else
            {
                _xmlDoc.Load(_path);
                XmlElement _userRootNode = _xmlDoc.SelectSingleNode("Data-List/" + _rootNodeName) as XmlElement;
                for (int i = 0; i < _nodeList.Length; i++)
                    AddChildtoNode(_xmlDoc, _userRootNode, _nodeList[i]);
            }
            _xmlDoc.Save(_path);
            return true;
        }

        /// <summary>
        /// 读取信息从指定的 xml 文件节点
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_rootNodeName"></param>
        /// <returns></returns>
        public static NodeStruct ReadNodesFromFile(string _path, string _rootNodeName)
        {
            NodeStruct _result = new NodeStruct();
            _result.setName(_rootNodeName);

            if (!File.Exists(_path))
            {
                return _result;
            }

            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(_path);

            XmlNodeList _nodeList = _xmlDoc.SelectSingleNode("Data-List/" + _rootNodeName).ChildNodes;
            foreach (XmlNode _node in _nodeList)
            {
                // 遍历改节点子树
                _recursionTraverse(_result,_node);
            }

            return _result;
        }
        public static NodeStruct ReadNodeFromFile(string _path, string _rootNodeName)
        {
            if (!File.Exists(_path))
            {
                return null;
            }

            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(_path);

            XmlNode _node = _xmlDoc.SelectSingleNode("Data-List/" + _rootNodeName);

            if (_node != null)
                return new NodeStruct(_node.Name, _node.InnerText);
            else
                return null;
        }

        private static void _recursionTraverse(NodeStruct _parent, XmlNode _parentNode)
        {
            // 先将当前节点加入
            NodeStruct _current = new NodeStruct(_parentNode.Name, _parentNode.InnerText);
            _parent.addChild(_current);

            // 遍历当前节点子树
            XmlNodeList _childList = _parentNode.ChildNodes;
            foreach (XmlNode _node in _childList)
            {
                // 递归
                _recursionTraverse(_current, _node);
            }
        }

        /// <summary>
        /// 添加节点到指定节点
        /// </summary>
        /// <param name="_targetDoc"></param>
        /// <param name="_targetNode"></param>
        /// <param name="_name"></param>
        /// <param name="_content"></param>
        private static void AddChildtoNode(XmlDocument _targetDoc, XmlElement _targetNode, NodeStruct _node)
        {
            XmlElement _element = _targetDoc.CreateElement(_node.getName());
            _element.InnerText = _node.getValue();
            _targetNode.AppendChild(_element);

            if (_node.getChildrenCount() != 0)
            {
                List<NodeStruct> _children = _node.getChildren();
                foreach (NodeStruct _ns in _children)
                {
                    AddChildtoNode(_targetDoc, _element, _ns);
                }
            }
        }

    }

}