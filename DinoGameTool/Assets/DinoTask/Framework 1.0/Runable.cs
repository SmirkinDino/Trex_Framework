using UnityEngine;

namespace Dino_Core.Task
{
    public class Runable : MonoBehaviour
    {
        public enum RunningState
        {
            NONE = -1,

            /// <summary>
            /// 不可以执行的阶段
            /// </summary>
            NOTREADY = 0,

            /// <summary>
            /// 可以触发了，但是还没有触发
            /// </summary>
            READY = 1,

            /// <summary>
            /// 已经触发了，并且正在执行
            /// </summary>
            RUNNING = 2,

            /// <summary>
            /// 未完成状态截止编号，这个枚举没有实际意义，只做标记使用
            /// </summary>
            NOTFINISH = 3,

            /// <summary>
            /// 已经执行结束了，等待重置
            /// </summary>
            END = 10,
        }

        [HideInInspector]
        public RunningState RState;

        private string _id = "";
        public string ID
        {
            get
            {
                if (_id == "")
                {
                    _id = name;
                }

                return _id;
            }
        }

        protected virtual void TReady()
        {
        }
        protected virtual void TStart()
        {
        }
        protected virtual void TInit()
        {
        }
        protected virtual void TReset()
        {
        }
        protected virtual void TEnd()
        {
        }
    }
}