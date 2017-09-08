using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dino_Core.DinoUGUI
{
    public class UISyncManager : MonoBehaviour
    {
        [SerializeField]
        private Queue<UIActionRequest> m_RequestQueue = new Queue<UIActionRequest>();

        private bool m_isExcuterRunning = false;

        private UIActionCallback On_RequestFinish;

        [SerializeField]
        private int QueueLength = 2;

        [SerializeField]
        private float QueueSnap = 0.0f;
        public static readonly float MAX_QUEUE_SNAP = 1.0f;
        public static readonly float MIN_QUEUE_SNAP = 0.0f;

        public void SendActionRequest(UIActionRequest _newAction)
        {
            // 当前的队列数大于规定最大队列数，拒绝请求
            if(m_RequestQueue.Count > QueueLength)
            {
                return;
            }

            // 加入队列
            m_RequestQueue.Enqueue(_newAction);

            // 如果队列协成没有启动，则启动
            if(!m_isExcuterRunning)
            {
                StartCoroutine("_ActionExcuter");
            }
        }
        
        private IEnumerator _ActionExcuter()
        {
            m_isExcuterRunning = true;

            while (m_RequestQueue.Count > 0)
            {
                UIActionRequest _action = m_RequestQueue.Dequeue();

                On_RequestFinish = UIActionDispatcher.DispatchAction(_action.ActionOwner,_action.ActionType);

                yield return new WaitForSeconds(_action.ActionDuring + QueueSnap);

                if (On_RequestFinish != null)
                {
                    On_RequestFinish();
                }
            }

            m_isExcuterRunning = false;
        }

        public void SetMaxQueueLength(int _length)
        {
            this.QueueLength = _length;
        }
        public void SetQueueSnap(float _snap)
        {
            QueueSnap = Mathf.Clamp(_snap, MIN_QUEUE_SNAP, MAX_QUEUE_SNAP);
        }
    }
}


