using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;



namespace Dino_Core.DinoUNet
{
    /// <summary>
    /// 
    /// 
    /// 
    /// Created by 龚楚涵
    /// 
    /// uNetwork组件不允许泛型的存在，所以不能写NetworkBehaviour的泛型单例基类
    /// 这个类的所有数据和方法都只能在服务器端调用
    /// .. 
    /// </summary>
    public class NetworkPlayerManager : NetworkBehaviour
    {

        #region singleton
        private static NetworkPlayerManager m_Manager = null;
        private NetworkPlayerManager()
        {

        }
        public static NetworkPlayerManager singleton
        {
            get
            {
                if (m_Manager == null)
                    m_Manager = GameObject.Find("Manager").AddComponent<NetworkPlayerManager>();
                if (m_Manager)
                {
                    return m_Manager;
                }
                else
                {
                    Debug.LogError("NetworkPlayerManager init failed");
                    return null;
                }
            }
        }
        #endregion

        public static readonly string GAME_SCENENAME = "testGame";

        [SerializeField]
        private Dictionary<string, bool> m_PlayerReadyStatus = new Dictionary<string, bool>();

        public void AddPlayer(string _player)
        {
            if (isServer)
            {
                AddStatuToStatusDic(_player, false);
            }
        }

        public void TellPlayerStatus(string _player, bool _status)
        {
            if (isServer)
            {
                SetStatusInStatusDic(_player, _status);

                if (isAllPlayerReady())
                {
                    Debug.Log("game starting");
                    NetworkMgr.ChangeScene(GAME_SCENENAME);
                }
            }
        }

        // 添加玩家到玩家列表中
        [ServerCallback]
        private void AddStatuToStatusDic(string _identity, bool _status)
        {
            if (m_PlayerReadyStatus.ContainsKey(_identity))
            {
                Debug.Log(_identity + " has already been added!");
                return;
            }
            else
            {
                Debug.Log(_identity + " has been added!");
                m_PlayerReadyStatus.Add(_identity, _status);
            }
        }

        // 设置玩家状态
        [ServerCallback]
        private void SetStatusInStatusDic(string _identity, bool _status)
        {
            if (m_PlayerReadyStatus.ContainsKey(_identity))
            {
                m_PlayerReadyStatus[_identity] = true;
                Debug.Log(_identity + " has " + (_status ? "ready" : "not ready") + "!");
                return;
            }
            else
            {
                Debug.Log(_identity + " not connected");
                m_PlayerReadyStatus.Add(_identity, _status);
            }
        }

        // 确认所有的玩家都准备完毕
        [ServerCallback]
        private bool isAllPlayerReady()
        {
            foreach (string _key in m_PlayerReadyStatus.Keys)
            {
                if (!m_PlayerReadyStatus[_key])
                {
                    Debug.Log(_key + " is not ready yet");
                    return false;
                }
            }

            Debug.Log("All player ready!");
            return true;
        }

    }
}