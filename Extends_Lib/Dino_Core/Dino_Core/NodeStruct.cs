using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Dino_Core
{
    public class NodeStruct
    {
        private string m_NodeName = "";
        private string m_Value = "";

        private List<NodeStruct> m_Children;

        public NodeStruct(string _name,string _value)
        {
            this.m_NodeName = _name;
            this.m_Value = _value;
            m_Children = new List<NodeStruct>();
        }
        public NodeStruct()
        {
            this.m_NodeName = "";
            this.m_Value = "";
            m_Children = new List<NodeStruct>();
        }

        public List<NodeStruct> getChildren()
        {
            return this.m_Children;
        }
        public void addChild(NodeStruct _node)
        {
            m_Children.Add(_node);
        }
        public NodeStruct getChild(string _nodeName)
        {
            foreach (NodeStruct _ns in m_Children)
            {
                if (_ns.m_NodeName == _nodeName)
                {
                    return _ns;
                }
            }
            return null;
        }
        public int getChildrenCount()
        {
            return m_Children.Count;
        }

        public string getName()
        {
            return this.m_NodeName;
        }
        public string getValue()
        {
            return this.m_Value;
        }
        public void setName(string _name)
        {
            this.m_NodeName = _name;
        }
        public void setValue(string _value)
        {
            this.m_Value = _value;
        }
    }
}
