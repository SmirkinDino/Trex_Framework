using System.Collections.Generic;
using UnityEngine;

namespace Dino_Core.Task
{
    public abstract class BaseTask : Runable , iController
    {
        #region INSPECTOR
        public BaseTask NextTask;
        #endregion

        protected Dictionary<TaskConst.Enemy_Type, AISets> _AIRouter = new Dictionary<TaskConst.Enemy_Type, AISets>();

        public AISets GetSets(TaskConst.Enemy_Type _type)
        {
            if (_AIRouter.ContainsKey(_type))
            {
                return _AIRouter[_type];
            }

            return null;
        }
        public void AddEntity(TaskConst.Enemy_Type _type,Transform _trans)
        {
            if (!_AIRouter.ContainsKey(_type))
            {
                _AIRouter.Add(_type, new AISets());
            }

            _AIRouter[_type].Add(_trans);
        }
        public void RemoveEntity(TaskConst.Enemy_Type _type, Transform _trans)
        {
            if (!_AIRouter.ContainsKey(_type))
            {
                return;
            }

            _AIRouter[_type].Remove(_trans);
        }

        public BaseEvent[] Events
        {
            get; protected set;
        }
        public T GetEvent<T>(string _eventID) where T : BaseEvent
        {
            if (Events == null)
            {
                return null;
            }

            for (int i = 0; i < Events.Length; i++)
            {
                if(Events[i].ID == _eventID)
                {
                    return Events[i] as T;
                }
            }

            return null;
        }
        public BaseTrigger[] Triggers
        {
            get; protected set;
        }
        public T GetTrigger<T>(string _eventID) where T : BaseTrigger
        {
            if (Triggers == null)
            {
                return null;
            }

            for (int i = 0; i < Triggers.Length; i++)
            {
                if (Triggers[i].ID == _eventID)
                {
                    return Triggers[i] as T;
                }
            }

            return null;
        }

        public void Ready()
        {
            ResetController();

            RState = RunningState.READY;

            for (int i = 0; i < Events.Length; i++)
            {
                Events[i].Ready();
            }

            for (int i = 0; i < Triggers.Length; i++)
            {
                Triggers[i].Ready();
            }

            TReady();
        }
        public void StartTask()
        {
            if (RState == RunningState.READY)
            {
                RState = RunningState.RUNNING;
                TStart();
            }
        }
        public void EndTask()
        {
            RState = RunningState.END;

            for (int i = 0; i < Events.Length; i++)
            {
                Events[i].EndEvent();
            }

            for (int i = 0; i < Triggers.Length; i++)
            {
                Triggers[i].ResetController();
            }

            TEnd();
        }
        public void InitController()
        {
            Events = GetComponentsInChildren<BaseEvent>();
            for (int i = 0; i < Events.Length; i++)
            {
                Events[i].BindTask(this);
                Events[i].InitController();
            }

            Triggers = GetComponentsInChildren<BaseTrigger>();
            for (int i = 0; i < Triggers.Length; i++)
            {
                Triggers[i].BindTask(this);
                Triggers[i].InitController();
            }

            RState = RunningState.NOTREADY;

            TInit();
        }
        public void ResetController()
        {
            RState = RunningState.NOTREADY;

            for (int i = 0; i < Events.Length; i++)
            {
                Events[i].ResetController();
            }

            for (int i = 0; i < Triggers.Length; i++)
            {
                Triggers[i].ResetController();
            }

            TReset();
        }

        protected virtual void Update()
        {
            if (RState != RunningState.RUNNING)
            {
                return;
            }

            // 如果任务没有结束
            if (MonitorEventsRunning())
            {
                return;
            }

            RState = RunningState.END;

            if (NextTask != null)
            {
                NextTask.Ready();
            }
        }
        private bool MonitorEventsRunning()
        {
            for (int i = 0; i < Events.Length; i++)
            {
                // 此处枚举请参看定义的注释
                if ((int)Events[i].RState < (int)RunningState.NOTFINISH)
                {
                    return true;
                }
            }

            return false;
        }
    }
}