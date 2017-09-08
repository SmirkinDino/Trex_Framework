using UnityEngine;

namespace Dino_Core.Task
{
    public interface iEvent : iController
    {
        bool StartEvent();
        void EndEvent();
    }

    public enum EventGizmos_Type
    {
        Spawner_Bot,
        Spawner_Esky,
        Spawner_Spider,
    }

    public abstract class BaseEvent : Runable , iEvent
    {
        public BaseTask Owner
        {
            get; protected set;
        }
        public EventGizmos_Type GizmosType;
        public Mesh GizmosMesh;

        protected void Update()
        {
            if (RState == RunningState.RUNNING)
            {
                OnExcute();
            }
        }
        protected virtual void OnExcute()
        {
        }

        public void Ready()
        {
            TReady();
            RState = RunningState.READY;
        }
        public void EndEvent()
        {
            TEnd();
            RState = RunningState.END;
        }
        public bool StartEvent()
        {
            if (RState == RunningState.READY)
            {
                TStart();
                RState = RunningState.RUNNING;
                return true;
            }

            return false;
        }
        public void InitController()
        {
            if (GetComponent<MeshRenderer>())
            {
                GetComponent<MeshRenderer>().enabled = false;
            }

            RState = RunningState.NOTREADY;

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
        public void RegisterEntity(TaskConst.Enemy_Type _type, Transform _trans)
        {
            if (Owner)
            {
                Owner.AddEntity(_type, _trans);
            }
        }
        public void UnregisterEntity(TaskConst.Enemy_Type _type, Transform _trans)
        {
            if (Owner)
            {
                Owner.RemoveEntity(_type, _trans);
            }
        }

        private void OnDrawGizmos()
        {
            switch (GizmosType)
            {
                case EventGizmos_Type.Spawner_Bot:
                    Gizmos.color = new Color(0.8f,0.2f,0.2f,0.35f);
                    break;
                case EventGizmos_Type.Spawner_Spider:
                    Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.35f);
                    break;
                case EventGizmos_Type.Spawner_Esky:
                    Gizmos.color = new Color(0.2f, 0.2f, 0.8f, 0.35f);
                    break;
                default:
                    break;
            }

            if (GizmosMesh)
            {
                Gizmos.DrawMesh(GizmosMesh, transform.position,transform.rotation, transform.localScale);
            }
            else
            {
                Gizmos.DrawCube(transform.position, new Vector3(2, 2, 2));
            }
        }
    }
}


