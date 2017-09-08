using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
namespace Dino_Core.DinoUNet
{
    // 同步子物体
    // 这个类智能挂在PlayerObject上
    // 同步的数据有POSITION 和 Rotation
    // ..
    public class SyncTransformChild : NetworkBehaviour
    {

        [SerializeField]
        public Transform mTarget;

        [SyncVar]
        private Vector3 mPlayerSyncPos;

        [SyncVar]
        private Vector3 mLastPlayerPos;

        [SyncVar]
        private Quaternion mPlayerSyncRotation;

        [SyncVar]
        private Quaternion mPlayerLastRotation;

        [SerializeField]
        private float mLerpRate = 15.0f;


        void FixedUpdate()
        {
            TransmitPosition();

            TransmitRotation();

            LerpPosition();

            LerpRotation();
        }


        private void LerpPosition()
        {
            if (!isLocalPlayer)
            {
                mTarget.position = Vector3.Lerp(mTarget.position, mPlayerSyncPos, Time.deltaTime * mLerpRate);
            }
        }

        // 这个函数在客户端上调用，在服务器上执行
        [Command]
        private void CmdSyncPosition(Vector3 pos)
        {
            mPlayerSyncPos = pos;
        }

        // 这个函数只能在客户端上运行
        [ClientCallback]
        private void TransmitPosition()
        {
            if (isLocalPlayer && Vector3.Distance(mLastPlayerPos, mTarget.position) >= 0.1f)
            {
                CmdSyncPosition(mTarget.position);
                mLastPlayerPos = mTarget.position;
            }
        }

        private void LerpRotation()
        {
            if (!isLocalPlayer)
            {
                try
                {
                    if (Time.deltaTime * mLerpRate != 0)
                        mTarget.rotation = Quaternion.Lerp(mTarget.rotation, mPlayerSyncRotation, Time.deltaTime * mLerpRate);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e);
                }
            }
        }

        [Command]
        private void CmdSyncRotation(Quaternion _rotaion)
        {
            mPlayerSyncRotation = _rotaion;
        }

        [ClientCallback]
        private void TransmitRotation()
        {
            if (isLocalPlayer && Quaternion.Angle(mTarget.rotation, mPlayerLastRotation) > 1.0f)
            {
                CmdSyncRotation(mTarget.rotation);
                mPlayerLastRotation = mTarget.rotation;
            }
        }
    }

}