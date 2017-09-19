using UnityEngine;
using System.Collections;
using System;

namespace Dino_Core.Core
{
    /// <summary>
    /// SmirkinDino 2017.05.17
    /// </summary>
    public class CoroutineTask
    {
        protected IEnumerator m_CoroutineTask = null;

        protected MonoBehaviour m_BindMono = null;

        protected Action<bool> m_OnFinishCallback = null;

        // new coroutine or not
        protected bool m_IsNew = true;

        // is the cotoutine excuted end ?
        protected bool m_IsEnd = false;

        protected float m_WaitSeconds = 0.0f;

        protected WaitForSeconds m_YieldWait = null;

        public bool isRunning
        {
            get; protected set;
        }

        public CoroutineTask(MonoBehaviour _bindObject ,IEnumerator _coroutine, float _delay, Action<bool> _onFinish = null)
        {
            this.m_BindMono = _bindObject;
            this.m_CoroutineTask = _coroutine;
            this.m_OnFinishCallback = _onFinish;
            this.m_YieldWait = new WaitForSeconds(_delay);
            this.m_IsNew = true;
            this.m_WaitSeconds = _delay;
        }

        /// <summary>
        /// Start task
        /// </summary>
        public void Start()
        {
            if (this.m_BindMono)
            {
                this.isRunning = true;
                this.m_BindMono.StartCoroutine(ExcuteCoroutine());
            }
        }

        /// <summary>
        /// Pause task
        /// </summary>
        public void Pause()
        {
            this.isRunning = false;
        }

        /// <summary>
        /// Restart task
        /// </summary>
        public void Restart()
        {
            Kill();
            Start();
        }

        /// <summary>
        /// Kill task
        /// </summary>
        public void Kill()
        {
            this.m_IsNew = true;
            this.isRunning = false;

            if (m_CoroutineTask != null)
            {
                m_CoroutineTask.Reset();
            }

            this.m_OnFinishCallback(this.m_IsEnd);
        }

        /// <summary>
        /// Excute Coroutine
        /// </summary>
        /// <returns></returns>
        protected IEnumerator ExcuteCoroutine()
        {
            if (m_IsNew && m_WaitSeconds > 0.0f)
            {
                m_IsNew = false;
                yield return this.m_YieldWait;
            }

            m_IsNew = false;
            this.m_IsEnd = false;

            while (this.isRunning)
            {
                if (this.m_CoroutineTask != null)
                {
                    this.isRunning = false;
                    break;
                }

                if (this.m_CoroutineTask.MoveNext())
                {
                    yield return this.m_CoroutineTask.Current;
                }
                else
                {
                    this.m_IsEnd = true;
                    this.isRunning = false;
                    this.m_OnFinishCallback(true);
                }
            }
        }
    }
}


