using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Dino_Core.DinoUNet
{

    public class Player_NetController : NetworkBehaviour
    {

        public MonoBehaviour[] m_listEnable;
        public GameObject m_cameraHead;

        void Start()
        {
            if (isLocalPlayer)
            {
                m_cameraHead.SetActive(true);

                foreach (MonoBehaviour mn in m_listEnable)
                {
                    mn.enabled = true;
                }
            }
        }


    }
}