using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Dino_Core.Task
{
    [System.Serializable]
    public class DEventNodeEditor : DBaseNodeEditor
    {
        public DEventNodeEditor()
        {
            NodeName = "Event";
        }
    }
}