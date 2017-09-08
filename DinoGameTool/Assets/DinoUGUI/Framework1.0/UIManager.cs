using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Dino_Core.AssetsUtils;

namespace Dino_Core.DinoUGUI
{
    public delegate void CORE_ONCLICKED_CALLBACK();

    public class UIManager : MonoSingleton<UIManager>
    {
        private Dictionary<string, DUIPanel> m_panelList;
        private Dictionary<string, DUIEntity> m_entityList;
        private Dictionary<string, CORE_ONCLICKED_CALLBACK> m_onClickedEventSet;

        private UISyncManager m_SyncManager;
        private GameObject m_UIRoot;

        public void Init()
        {
            // 初始化管理器参数
            initManagerConfig();

            // 添加事件监听
            AttachClickListener(m_UIRoot);

            // 初始化UI树
            initUITree();
        }

        public void Show(string _panelName)
        {
            DUIPanel _panel = GetPanel(_panelName);
            if (_panel)
            {
                SendActionRequest(new UIActionRequest(_panel,Action_Request_Type.ACTION_SHOW));
            }
        }

        public void Hide(string _panelName)
        {
            DUIPanel _panel = GetPanel(_panelName);
            if (_panel)
            {
                SendActionRequest(new UIActionRequest(_panel, Action_Request_Type.ACTION_HIDE));
            }
        }

        public void SendActionRequest(UIActionRequest _request)
        {
            m_SyncManager.SendActionRequest(_request);
        }

        public void SendUpdateCommand()
        {
            foreach (DUIPanel _panel in m_panelList.Values)
            {
                _panel.UpdateUI();
            }
        }
        public void SendUpdateCommand(string _panelName)
        {
            DUIPanel _panel = m_panelList.GetEntity(_panelName);
            if(_panel)
            {
                _panel.UpdateUI();
            }
        }
        public void RegistClickEvent(string _targetName, CORE_ONCLICKED_CALLBACK _eventEntity)
        {
            try
            {
                CORE_ONCLICKED_CALLBACK _event = m_onClickedEventSet.GetEntity(_targetName);
                if (_event == null)
                {
                    m_onClickedEventSet.Add(_targetName, _eventEntity);
                }
                else
                {
                    _event += _eventEntity;
                }
            }
            catch (Exception ex)
            {
                this.DLog(string.Format("点击事件注册出错 {0} {1}", _targetName, ex));
            }
        }
        public void UnregistClickEvent(string _targetName, CORE_ONCLICKED_CALLBACK _eventEntity)
        {
            try
            {
                CORE_ONCLICKED_CALLBACK _event = m_onClickedEventSet.GetEntity(_targetName);
                _event -= _eventEntity;
            }
            catch (Exception ex)
            {
                Debug.Log("点击事件注销出错 " + _targetName + " " + ex);
            }
        }


        public DUIPanel GetPanel(string _name)
        {
            return m_panelList.GetEntity(_name);
        }
        public DUIEntity GetEntity(string _id)
        {
            return m_entityList.GetEntity(_id);
        }

        public void LoadUI(string _prefabPath)
        {
            StartCoroutine(_excuteLoadUI(_prefabPath));
        }

        private IEnumerator _excuteLoadUI(string _prefabPath)
        {
            // Load Resource
            UnityEngine.Object _res = Resources.Load(_prefabPath);
            UnityEngine.GameObject _entity = GameObject.Instantiate(_res) as GameObject;

            yield return null;

            // 设置父对象
            _entity.transform.SetParent(m_UIRoot.transform,false);

            // 初始化Panel
            DUIPanel _panel = _entity.GetComponent<DUIPanel>();

            _panel.Init();

            AddPanel(_panel.getName(), _panel);

            DoAttackListener(_entity);

            // 隐藏UI
            Hide(_panel.getName());

            yield return null;
        }

        private void AddPanel(string _name, DUIPanel _panel)
        {
            m_panelList.AddEntity(_name, _panel);
        }
        private void initUITree()
        {
            // 初始化Panel列表
            if (this.m_panelList != null)
            {
                this.m_panelList.Clear();
            }
            else
            {
                this.m_panelList = new Dictionary<string, DUIPanel>();
            }

            DUIPanel[] _panels = m_UIRoot.GetComponentsInChildren<DUIPanel>();
            foreach (DUIPanel _panel in _panels)
            {
                _panel.Init();
                AddPanel(_panel.gameObject.name, _panel);
            }

            // 初始化所有UI控件列表
            if (this.m_entityList != null)
            {
                this.m_entityList.Clear();
            }
            else
            {
                this.m_entityList = new Dictionary<string, DUIEntity>();
            }

            DUIEntity[] _entities = m_UIRoot.GetComponentsInChildren<DUIEntity>();
            foreach (DUIEntity _entity in _entities)
            {
                m_entityList.AddEntity(_entity.UniqueID, _entity);
            }
        }
        private void initManagerConfig()
        {
            m_UIRoot = ((Canvas)GameObject.FindObjectOfType(typeof(Canvas))).gameObject;
            if (!m_UIRoot)
            {
                Debug.LogError("UIManager 初始化失败,没有找到UI ROOT");
                return;
            }

            // 初始化UI异步事件管理器
            if (!m_SyncManager)
            {
                m_SyncManager = gameObject.AddComponent<UISyncManager>();
            }

            // 初始化事件分发器
            UIActionDispatcher.InitActionDispatcher();
        }

        /// <summary>
        /// 添加点击事件监听
        /// </summary>
        private void AttachClickListener(GameObject _parent)
        {
            if(m_onClickedEventSet != null)
            {
                m_onClickedEventSet.Clear();
            }
            else
            {
                m_onClickedEventSet = new Dictionary<string, CORE_ONCLICKED_CALLBACK>();
            }

            DoAttackListener(_parent);
        }
        private void DoAttackListener(GameObject _parent)
        {
            Button[] _buttonSet = _parent.GetComponentsInChildren<Button>();
            foreach (Button _btn in _buttonSet)
            {
                _btn.onClick.AddListener(OnClicked);
            }
        }
        private void OnClicked()
        {
            GameObject _currentSelected = EventSystem.current.currentSelectedGameObject;

            if (_currentSelected)
            {
                CORE_ONCLICKED_CALLBACK _event = m_onClickedEventSet.GetEntity(_currentSelected.name);
                if (_event != null)
                {
                    _event();
                }
            }

            /// <summary>
            /// 修复unity5.4.2f版本按钮动画的bug
            /// </summary>
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}