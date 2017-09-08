using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
namespace Dino_Core.DinoUNet
{
    /// <summary>
    /// 
    /// </summary>
    public class NetworkMgr : NetworkManager
    {


        public Text m_serverAddress;
        public Text m_serverPort;
        public Text m_ClientPort;

        private GameObject m_Player;

        public void StartAsHost()
        {

            NetworkManager.singleton.networkPort = int.Parse(m_serverPort.text);

            NetworkManager.singleton.StartHost();
        }

        public void StartAsClient()
        {

            NetworkManager.singleton.networkPort = int.Parse(m_ClientPort.text);

            NetworkManager.singleton.networkAddress = m_serverAddress.text;

            NetworkManager.singleton.StartClient();
        }

        //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        //{
        //    NetworkServer.AddPlayerForConnection(conn,m_Player,playerControllerId);
        //}

        //public void AddPlayer(GameObject _player)
        //{
        //    m_Player = _player;
        //    if (!m_Player.GetComponent<NetworkIdentity>())
        //    {
        //        NetworkIdentity _identity = m_Player.AddComponent<NetworkIdentity>();
        //        _identity.localPlayerAuthority = true;
        //    }

        //    if (!m_Player.GetComponent<NetworkTransform>())
        //    {
        //        m_Player.AddComponent<NetworkTransform>();
        //    }

        //    ClientScene.AddPlayer(0);
        //}


        public static void ChangeScene(string _sceneName)
        {
            NetworkManager.singleton.ServerChangeScene(_sceneName);
        }

    }
}