using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


namespace Dino_Core.DinoUGUI
{
    public class DUIEntity : MonoBehaviour, iController
    {
        public static int CURRENT_INDEX = 0;

        public string UniqueID
        {
            get; private set;
        }

        public bool HideOnEnable = false;

        protected Internal.UpdateEvent m_UpdateEventPool;

        protected Dictionary<string, DUIEntity> m_Children;

        protected List<DinoTweener> m_Animations;

        public float AnimationDuring
        {
            get; protected set;
        }

        public void Init()
        {
            UniqueID = "DUI_" + CURRENT_INDEX++;

            m_Children = new Dictionary<string, DUIEntity>();

            Calculate_AnimationDuring();

            InitData();

            OnInit();

            if (HideOnEnable) gameObject.SetActive(false);
        }

        public virtual void InitData()
        {

        }
        public void Show()
        {
            OnShow();

            if (m_Animations != null && m_Animations.Count > 0)
            {
                foreach (DinoTweener _dt in m_Animations)
                {
                    _dt.PlayForward();
                }
            }

            foreach (string _key in m_Children.Keys)
            {
                m_Children[_key].Show();
            }
        }
        public void Hide()
        {
            OnHide();

            if (m_Animations != null && m_Animations.Count > 0)
            {
                foreach (DinoTweener _dt in m_Animations)
                {
                    _dt.PlayBack();
                }
            }

            foreach (string _key in m_Children.Keys)
            {
                m_Children[_key].Hide();
            }
        }

        public virtual void OnShow()
        {
        }
        public virtual void OnHide()
        {
        }
        public virtual void OnUpdate()
        {
        }
        public virtual void OnInit()
        {
        }

        public void UpdateUI()
        {
            OnUpdate();
            if (m_UpdateEventPool != null) m_UpdateEventPool();
        }
        public void UpdateUI(Internal.UpdateEvent _event)
        {
            OnUpdate();
            if (_event != null) _event();
        }
        public void RegistUpdateDelegate(Internal.UpdateEvent _event)
        {
            m_UpdateEventPool += _event;
        }
        public void UnregistUpdateDelegate(Internal.UpdateEvent _event)
        {
            m_UpdateEventPool -= _event;
        }

        public void AddChild<T>(T _child) where T : DUIEntity
        {
            m_Children.AddEntity(_child.gameObject.name, _child);
        }

        public void AddChildren<T>(T[] _children) where T : DUIEntity
        {
            foreach (DUIEntity _entity in _children)
            {
                AddChild(_entity);
            }
        }

        protected void InitChildren<T>() where T : DUIEntity
        {
            AddChildren<T>(GetComponentsInChildren<T>());

            foreach (string _id in m_Children.Keys)
            {
                m_Children[_id].Init();
            }
        }
        public DUIEntity GetChild(string _id)
        {
            return m_Children.GetEntity(_id);
        }
        public void RemoveChild(DUIEntity _child)
        {
            RemoveChild(_child.gameObject.name);
        }

        private void RemoveChild(string _id)
        {
            m_Children.RemoveEntity(_id);
        }
        private void Calculate_AnimationDuring()
        {
            m_Animations = new List<DinoTweener>(GetComponents<DinoTweener>());

            AnimationDuring = 0.0f;

            foreach (DinoTweener _dt in m_Animations)
            {
                if (AnimationDuring < _dt.During)
                {
                    AnimationDuring = _dt.During;
                }
            }
        }

    }
}