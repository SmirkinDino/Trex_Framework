using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
namespace Dino_Core.DinoUNet
{
    public class PlayerID : NetworkBehaviour
    {

        [SyncVar]
        private string m_UniqueIdentity;
        private NetworkInstanceId m_NetId;
        private Transform m_Transform;
        [SerializeField]
        private string m_prefabName = "";

        public override void OnStartLocalPlayer()
        {
            GetNetIdentity();
            SetIdentity();
        }

        public void Update()
        {
            if (m_Transform.name == "" || m_Transform.name == m_prefabName + "(Clone)")
            {
                SetIdentity();
            }
        }

        public void Awake()
        {
            m_Transform = transform;
        }

        private void GetNetIdentity()
        {
            m_NetId = GetComponent<NetworkIdentity>().netId;
            CmdTellServerMyIdentity(MakeUniqueIdentiy());
        }

        private void SetIdentity()
        {
            if (!isLocalPlayer)
            {
                m_Transform.name = m_UniqueIdentity;
            }
            else
            {
                m_Transform.name = MakeUniqueIdentiy();
            }
        }

        private string MakeUniqueIdentiy()
        {
            string uniqueName = "Player_" + m_NetId.ToString();
            return uniqueName;
        }

        [Command]
        private void CmdTellServerMyIdentity(string name)
        {
            m_UniqueIdentity = name;
        }





    }
}