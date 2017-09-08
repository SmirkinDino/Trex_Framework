using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
namespace Dino_Core.DinoUNet
{
    public class NetworkObjectsPool : NetworkBehaviour
    {
        #region singleton
        private static NetworkObjectsPool mPool = null;
        private NetworkObjectsPool()
        {

        }
        public static NetworkObjectsPool singleton
        {
            get
            {
                if (mPool == null) mPool = GameObject.Find("Managers").GetComponent<NetworkObjectsPool>();
                return mPool;
            }
        }
        #endregion

        #region var

        // 物体池
        private Dictionary<string, Pool> mPools = new Dictionary<string, Pool>();

        // 默认的单个物体池大小
        public readonly static int DEFAULT_POOLSIZE = 100;

        // 当物体从物体池中取出时的操作方法
        public delegate void OnAwakeHandler(GameObject _obj);
        public delegate GameObject InstanceObjecHandler();

        private GameObject m_tempObject;

        #endregion




        // Spawn物体，并带一个标签名字存入物体池
        public void SpawnObject(string _poolName, InstanceObjecHandler _instanceHandler, OnAwakeHandler _awakeHandler)
        {
            DoSpawnObject(_poolName, _instanceHandler, _awakeHandler);
        }



        // 这个函数只会在服务器执行，将物体Spawn到各个客户端
        [Server]
        private void DoSpawnObject(string _poolName, InstanceObjecHandler _instanceHandler, OnAwakeHandler _awakeHandler)
        {
            if (NetworkServer.active)
            {
                GetObject(_poolName);
                GameObject _go = m_tempObject;

                if (_go != null)
                {
                    // 如果物体池中有同类物体，则将该物体取出，并设置为active
                    RpcSetActive(_go, true);
                    _awakeHandler(_go);
                }
                else
                {
                    // 物体池没有此类物体，Spwan 新物体
                    NetworkServer.Spawn(_instanceHandler());
                }
            }
        }



        // 添加物体到物体池，这个函数只会在服务器运行
        [Server]
        public void AddtoReusePool(GameObject _obj, string _poolName)
        {
            if (isServer)
                AddtoPool(_obj, _poolName);
        }



        // 添加物体到相应标签的物体池中，这个函数只会在服务器上调用，在客户端上执行，用于同步物体池
        [Server]
        private void AddtoPool(GameObject _obj, string _poolName)
        {
            if (!_obj) return;

            RpcSetActive(_obj, false);

            if (mPools.ContainsKey(_poolName)) mPools[_poolName].AddObject(_obj);
            else
            {
                // 没有这个物体池，所以添加新的物体池
                mPools.Add(_poolName, new Pool(_poolName, DEFAULT_POOLSIZE));

                mPools[_poolName].AddObject(_obj);
            }
        }



        // 得到物体池中的物体，这个函数只会在客户端运行，在服务器调用
        [Server]
        private void GetObject(string _poolName)
        {
            if (mPools.ContainsKey(_poolName))
            {
                m_tempObject = mPools[_poolName].getObject();
            }
            else
            {
                m_tempObject = null;
            }
        }



        [ClientRpc]
        private void RpcSetActive(GameObject _obj, bool _active)
        {
            _obj.SetActive(_active);
            MonoBehaviour _bullet = _obj.GetComponent<MonoBehaviour>();
            if (_bullet)
                _bullet.StopAllCoroutines();
        }
    }


}