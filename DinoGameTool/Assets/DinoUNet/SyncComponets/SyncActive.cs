using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
namespace Dino_Core.DinoUNet
{
    // 同步显示ACTIVE状态
    // 这个类只可以挂在PLAYER(U-NET概念下)上，挂在NON-PLAYER物体上会失效出错
    // ..
    // ..
    [NetworkSettings(sendInterval = 0.2f)]
    public class SyncActive : NetworkBehaviour
    {

        public GameObject[] m_listGameObjects;

        void Update()
        {
            if (isLocalPlayer)
            {
                for (int i = 0; i < m_listGameObjects.Length; i++)
                {
                    CmdSyncState(i, m_listGameObjects[i].activeSelf);
                }
            }
        }

        // 同步申请，这个函数根据Update时间调用 [NetworkSettings(sendInterval = 0.2f)]
        [Command]
        void CmdSyncState(int _index, bool _active)
        {
            RpcSyncState(_index, _active);
        }

        // 同步方法，在服务器调用，在所有客户端执行
        [ClientRpc]
        void RpcSyncState(int _index, bool _active)
        {
            m_listGameObjects[_index].SetActive(_active);
        }
    }
}