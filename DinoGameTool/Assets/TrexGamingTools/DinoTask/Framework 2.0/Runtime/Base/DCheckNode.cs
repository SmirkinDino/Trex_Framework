using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dino_Core.Task
{
    public class DCheckNode : DBaseNode
    {
        public override sealed void Selected(DBaseNode _spreadNode)
        {
            OnSelected(_spreadNode);
        }
        public override sealed bool Excute()
        {
            OnExcute();
            return true;
        }
        public override sealed void End()
        {
            OnEnd();
        }
    }
}