using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
namespace Dino_Core.DinoUNet
{
    public class NetworkEntity : NetworkBehaviour
    {

        // U-Networking 网络组件的自定义基类
        // 这个类封装了一下使用U-Networking 的通用信息比如 NetworkIdentity
        // 拓展player和non-player（U-NET概念中）都有的属性和方法在这个类中拓展
        // ..

        // network 身份组件
        protected NetworkIdentity m_NetIdentity;

        // 初始化函数，初始化标志
        public virtual void init()
        {
            m_NetIdentity = GetComponent<NetworkIdentity>();
        }
    }
}