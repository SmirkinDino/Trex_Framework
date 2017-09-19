using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dino_Core.Task
{
    public class DBaseNode
    {
        /// <summary>
        /// ID
        /// </summary>
        public int NodeID;

        /// <summary>
        /// Name
        /// </summary>
        public string NodeName;

        /// <summary>
        /// is this node a check point ? This only be mark for editor mod
        /// </summary>
        public bool IsCheckNode = false;

        /// <summary>
        /// Link List
        /// </summary>
        public List<int> Nexts;

        /// <summary>
        /// This will be call when task system init
        /// </summary>
        public virtual void Init()
        {
            OnInit();
        }

        /// <summary>
        /// Users Interface
        /// </summary>
        public virtual void OnInit() { }

        /// <summary>
        /// On this Node start been excuted
        /// </summary>
        /// <param name="_spreadNode">prev node, this could be different</param>
        public virtual void Selected(DBaseNode _spreadNode)
        {
            OnSelected(_spreadNode);
        }

        /// <summary>
        /// Users Interface
        /// </summary>
        /// <param name="_spreadNode">prev node, this could be different</param>
        public virtual void OnSelected(DBaseNode _spreadNode) { }

        /// <summary>
        /// this will be call every frame
        /// </summary>
        public virtual bool Excute()
        {
            OnExcute();
            return true;
        }

        /// <summary>
        /// Users Interface
        /// </summary>
        public virtual void OnExcute() { }

        /// <summary>
        /// this will be call node event end
        /// </summary>
        public virtual void End()
        {
            OnEnd();
        }

        /// <summary>
        /// Users Interface
        /// </summary>
        public virtual void OnEnd() { }
    }

    public class DRunable : DBaseNode
    {
        /// <summary>
        /// 没有开始的进度
        /// </summary>
        public static readonly float NonProcess = 0;

        /// <summary>
        /// 结束的进度
        /// </summary>
        public static readonly float EndProcess = 1;

        /// <summary>
        /// Process of the event
        /// </summary>
        [Range(0, 1)]
        public float Process;

        public override sealed void Selected(DBaseNode _spreadNode)
        {
            OnSelected(_spreadNode);

            this.Process = NonProcess;
        }

        public override sealed bool Excute()
        {
            OnExcute();

            return this.Process == EndProcess;
        }

        public override sealed void End()
        {
            OnEnd();

            this.Process = NonProcess;
        }
    }

    public class DCondition : DBaseNode
    {
        /// <summary>
        /// whether this Condition is true or false
        /// </summary>
        /// <returns></returns>
        public virtual bool OnCondition()
        {
            return true;
        }

        public override sealed void Selected(DBaseNode _spreadNode)
        {
            OnSelected(_spreadNode);
        }

        public override sealed bool Excute()
        {
            OnExcute();

            return OnCondition();
        }

        public override sealed void OnExcute() { }

        public override sealed void End()
        {
            OnEnd();
        }
    }
}

