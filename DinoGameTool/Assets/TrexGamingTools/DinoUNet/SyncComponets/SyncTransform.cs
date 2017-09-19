using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;



namespace Dino_Core.DinoUNet
{
    // 线性插值同步
    // 同步的数据有 POSITION ROTATION
    // 同样只能用于 Player玩家
    // ..

    public class SyncTransform : NetworkBehaviour
    {

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

        void Update()
        {
            TransmitPosition();

            TransmitRotation();
        }

        void FixedUpdate()
        {
            LerpPosition();

            LerpRotation();
        }


        private void LerpPosition()
        {
            if (!isLocalPlayer)
            {
                transform.position = Vector3.Lerp(transform.position, mPlayerSyncPos, Time.deltaTime * mLerpRate);
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
            if (isLocalPlayer && Vector3.Distance(mLastPlayerPos, transform.position) >= 0.1f)
            {
                CmdSyncPosition(transform.position);
                mLastPlayerPos = transform.position;
            }
        }

        private void LerpRotation()
        {
            if (!isLocalPlayer)
            {
                try
                {
                    if (Time.deltaTime * mLerpRate != 0)
                        transform.rotation = Quaternion.Lerp(transform.rotation, mPlayerSyncRotation, Time.deltaTime * mLerpRate);
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
            if (isLocalPlayer && Quaternion.Angle(transform.rotation, mPlayerLastRotation) > 1.0f)
            {
                CmdSyncRotation(transform.rotation);
                mPlayerLastRotation = transform.rotation;
            }
        }
    }
}