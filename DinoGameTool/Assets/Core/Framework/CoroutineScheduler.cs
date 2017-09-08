using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Dino_Core
{
    /// <summary>
    /// SmirkinDino 2017.05.17
    /// </summary>
    public class CoroutineScheduler : MonoSingleton<CoroutineScheduler>
    {
        // the router
        private static Dictionary<string, CoroutineTask> m_CoRouter = new Dictionary<string, CoroutineTask>();

        // To save CG and memeroy malloc, Ignore this var is ok
        private CoroutineTask m_HandleTask;

        // To save CG and memeroy malloc, Ignore this var is ok
        private Dictionary<string, CoroutineTask>.Enumerator m_HandleEnumerator;

        /// <summary>
        /// Add Corourine Task to CoroutineScheduler, and return Coroutine Task Entity
        /// </summary>
        /// <param name="_mono">the target monobehaviour</param>
        /// <param name="_key">store key</param>
        /// <param name="_coroutine">coroutine entity</param>
        /// <param name="_delay">delay time, 0 means wait no time</param>
        /// <param name="_onFinish">call when every time finish the corourine</param>
        /// <returns></returns>
        public CoroutineTask AddCoroutineTask(MonoBehaviour _mono, string _key, IEnumerator _coroutine,float _delay, Action<bool> _onFinish = null)
        {
            CoroutineTask _task = new CoroutineTask(_mono,_coroutine,_delay,_onFinish);
            m_CoRouter.AddEntity(_key, _task);
            return _task;
        }

        public CoroutineTask AddCoroutineTask(MonoBehaviour _mono, string _key, IEnumerator _coroutine,  Action<bool> _onFinish = null)
        {
            return AddCoroutineTask(_mono,_key,_coroutine, 0.0f, _onFinish);
        }

        public CoroutineTask AddCoroutineTask(string _key, IEnumerator _coroutine,  Action<bool> _onFinish = null)
        {
            return AddCoroutineTask(this, _key, _coroutine, 0.0f,_onFinish);
        }

        /// <summary>
        /// start corourine task
        /// </summary>
        /// <param name="_key"></param>
        public void StartCoroutineTask(string _key)
        {
            m_HandleTask = m_CoRouter.GetEntity(_key);
            if (m_HandleTask != null)
            {
                m_HandleTask.Start();
            }
        }

        /// <summary>
        /// start all corourine task
        /// </summary>
        public void StartAllCoroutineTasks()
        {
            m_HandleEnumerator = m_CoRouter.GetEnumerator();
            while (m_HandleEnumerator.MoveNext())
            {
                m_HandleEnumerator.Current.Value.Start();
            }
        }

        /// <summary>
        /// Pause corourine task
        /// </summary>
        /// <param name="_key"></param>
        public void PauseCoroutineTask(string _key)
        {
            m_HandleTask = m_CoRouter.GetEntity(_key);
            if (m_HandleTask != null)
            {
                m_HandleTask.Pause();
            }
        }

        /// <summary>
        /// Pause all corourine task
        /// </summary>
        public void PauseAllCoroutineTasks()
        {
            m_HandleEnumerator = m_CoRouter.GetEnumerator();
            while (m_HandleEnumerator.MoveNext())
            {
                m_HandleEnumerator.Current.Value.Pause();
            }
        }

        /// <summary>
        /// Restart corourine task
        /// </summary>
        /// <param name="_key"></param>
        public void RestartCoroutineTask(string _key)
        {
            m_HandleTask = m_CoRouter.GetEntity(_key);
            if (m_HandleTask != null)
            {
                m_HandleTask.Restart();
            }
        }

        /// <summary>
        /// Restart all corourine task
        /// </summary>
        public void RestartAllCoroutineTasks()
        {
            m_HandleEnumerator = m_CoRouter.GetEnumerator();
            while (m_HandleEnumerator.MoveNext())
            {
                m_HandleEnumerator.Current.Value.Restart();
            }
        }

        /// <summary>
        /// Kill corourine task
        /// </summary>
        /// <param name="_key"></param>
        public void KillCoroutineTask(string _key)
        {
            m_HandleTask = m_CoRouter.GetEntity(_key);
            if (m_HandleTask != null)
            {
                m_HandleTask.Kill();
            }
        }

        /// <summary>
        /// Kill all corourine task
        /// </summary>
        public void KillAllCoroutineTasks()
        {
            m_HandleEnumerator = m_CoRouter.GetEnumerator();
            while (m_HandleEnumerator.MoveNext())
            {
                m_HandleEnumerator.Current.Value.Kill();
            }
        }

        /// <summary>
        /// Remove corourine task
        /// </summary>
        /// <param name="_key"></param>
        public void RemoveCoroutineTask(string _key)
        {
            m_HandleTask = null;
            m_HandleTask = m_CoRouter.GetEntity(_key);

            if (m_HandleTask != null)
            {
                m_HandleTask.Kill();
                m_CoRouter.RemoveEntity(_key);
            }
        }

        /// <summary>
        /// Remove all corourine task
        /// </summary>
        public void RemoveAllCoroutineTasks()
        {
            KillAllCoroutineTasks();
            m_CoRouter.Clear();
        }
    }
}