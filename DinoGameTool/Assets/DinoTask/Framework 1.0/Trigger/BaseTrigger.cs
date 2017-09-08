using Dino_Core.AssetsUtils;
using UnityEngine;

namespace Dino_Core.Task
{
    public interface iTrigger : iController
    {
        bool Conditional();
    }

    public abstract class BaseTrigger : Runable, iTrigger
    {
        public BaseTask Owner
        {
            get ; set;
        }
        public BaseEvent[] Events
        {
            get; set;
        }

        public iEvent GetTarget(string _eventID)
        {
            for (int i = 0; i < Events.Length; i++)
            {
                if (Events[i].ID == _eventID)
                {
                    return Events[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 触发
        /// </summary>
        /// <returns></returns>
        public bool Conditional()
        {
            if (RState != RunningState.READY)
            {
                return false;
            }

            TConditional();

            for (int i = 0; i < Events.Length; i++)
            {
                if(!Events[i].StartEvent())
                {
                    this.DLog(this + "的" + i + "号事件没有能够正常出发！");
                }
            }

            RState = RunningState.END;
            return true;
        }
        public void Ready()
        {
            RState = RunningState.READY;
            TReady();
        }
        public void InitController()
        {
            if (GetComponent<MeshRenderer>())
            {
                GetComponent<MeshRenderer>().enabled = false;
            }

            RState = RunningState.NOTREADY;

            Events = GetComponentsInChildren<BaseEvent>();

            TInit();
        }
        public void ResetController()
        {
            RState = RunningState.NOTREADY;
            TReset();
        }
        public void BindTask(BaseTask _task)
        {
            Owner = _task;
        }

        protected virtual void OnMonitoring()
        {
        }
        protected virtual void TConditional()
        {
        }

        private void Update()
        {
            if (RState == RunningState.READY)
            {
                OnMonitoring();
            }
        }

    }
}