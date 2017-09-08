using UnityEngine;
using System.Collections;
namespace Dino_Core.DinoUNet
{
    // 申请拥有连接控制权的接口
    // 拥有这个接口的物体表示拥有申请连接控制权的能力
    // ..
    public interface iAuthorityAble
    {
        // 申请
        void Assign();
        // 注销
        void Unassign();
    }
}