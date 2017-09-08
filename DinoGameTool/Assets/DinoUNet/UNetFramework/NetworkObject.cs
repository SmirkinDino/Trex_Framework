using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

namespace Dino_Core.DinoUNet
{
    /// <summary>
    /// 
    /// 
    /// 这个类是不要让PLAYER类（U-NETWORK概念下的PLAYER）继承，Player请继承NetworkEntity
    /// </summary>
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkObject : NetworkEntity, iAuthorityAble
    {

        // iAuthorityAble接口表示该物体在当前连接中可以用有发送Command命令的权限，
        // 注册事件只能在服务器上发生
        // ..
        // 关于这个申请权限AssignClientAuthority和RemoveClientAuthority这两个函数的效率问题不得而知，但在现有的测试和DEMO中频繁调用会造成卡顿（不排除其他原因）
        // 尽量少使用这两个命令
        // ..
        // ..
        // 得到发送命令的权限
        public void Assign()
        {
            if (!isServer) return;

            if (m_NetIdentity.clientAuthorityOwner != null)
            {
                m_NetIdentity.RemoveClientAuthority(m_NetIdentity.clientAuthorityOwner);
            }

            m_NetIdentity.AssignClientAuthority(NetworkServer.localConnections[0]);

        }

        // 注销发送命令的权限
        public void Unassign()
        {
            if (!isServer) return;

            if (m_NetIdentity.clientAuthorityOwner != null)
            {
                m_NetIdentity.RemoveClientAuthority(m_NetIdentity.clientAuthorityOwner);
            }
        }
    }
}