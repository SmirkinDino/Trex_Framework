using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dino_Core_Test
{
    class Program
    {
        static void Main(string[] args)
        {

            //List<Dino_Core.NodeStruct> _listMain = new List<Dino_Core.NodeStruct>();
            //Dino_Core.NodeStruct _node = new Dino_Core.NodeStruct("child", "");
            //Dino_Core.NodeStruct _node2 = new Dino_Core.NodeStruct("child2", "");

            //_node.addChild(new Dino_Core.NodeStruct("sss", "dddddddddddd"));
            //_node.addChild(new Dino_Core.NodeStruct("sss", "dddddddddddd"));
            //_node.addChild(new Dino_Core.NodeStruct("sss", "dddddddddddd"));
            //_node.addChild(_node2);

            //_node2.addChild(new Dino_Core.NodeStruct("sss", "dddddddddddd"));
            //_node2.addChild(new Dino_Core.NodeStruct("sss", "dddddddddddd"));
            //_node2.addChild(new Dino_Core.NodeStruct("sss", "dddddddddddd"));
            //_node2.addChild(new Dino_Core.NodeStruct("sss", "dddddddddddd"));
            //_node2.addChild(new Dino_Core.NodeStruct("sss", "dddddddddddd"));

            //_listMain.Add(new Dino_Core.NodeStruct("ddd", "sss"));
            //_listMain.Add(new Dino_Core.NodeStruct("ddd", "sss"));
            //_listMain.Add(new Dino_Core.NodeStruct("ddd", "sss"));
            //_listMain.Add(new Dino_Core.NodeStruct("ddd", "sss"));
            //_listMain.Add(_node);

            //Dino_Core.XmlHandler.AddNodesToFile("test.xml", "Root", _listMain.ToArray());

            //System.Console.ReadLine();


            Dino_Core.NodeStruct _node = Dino_Core.XmlHandler.ReadNodesFromFile("test.xml", "Root");

            System.Console.ReadLine();
        }
    }
}
