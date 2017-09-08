using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System;
using System.Text;

namespace Dino_Core.AssetsUtils
{

    /// <summary>
    /// 
    /// SnmirkinDino 2017 04 28
    /// spawnManager
    /// 
    /// </summary>
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        /// <summary>
        /// this is the reuse pool ,the key of this dictionary is poolName + Key, such as "Bullet-AK47"
        /// </summary>
        private static Dictionary<string, Queue<Transform>> _objectsPool;

        /// <summary>
        /// this is the despawn orderset , all delay despawn orders cache here 
        /// </summary>
        private static List<DespawnOrder> _despawnOrders;

        /// <summary>
        /// this is the time piece when despawnExcuter tick
        /// </summary>
        private static WaitForSeconds _despawnTaskTimePiece = new WaitForSeconds(0.05f);

        /// <summary>
        /// To save CG
        /// </summary>
        private static Queue<Transform> _handledQueue;

        /// <summary>
        /// To save CG 
        /// </summary>
        private static SpawnPool _handleSpawnPool;

        /// <summary>
        /// prefab pools
        /// </summary>
        private static List<SpawnPool> _pools;

        /// <summary>
        /// is the DespawnExcuterRunning ?
        /// </summary>
        private bool _isDespawnExcuterRunning = true;

        /// <summary>
        /// Init pool
        /// </summary>
        public void InitPool()
        {
            if (_objectsPool == null) _objectsPool = new Dictionary<string, Queue<Transform>>();
            else _objectsPool.Clear();

            if (_despawnOrders == null) _despawnOrders = new List<DespawnOrder>();
            else _despawnOrders.Clear();

            try
            {
                _pools = (Resources.Load("ConfigurationAsset/Dino_Core.AssetsUtils.SpawnPoolConfig") as SpawnPoolConfig).Pools;
            }
            catch (Exception)
            {
#if DINO_DEBUG
            this.LogEditorOnly(string.Format("found SpawnPool Prefab failed!"));
#endif
            }

#if DINO_DEBUG
            this.LogEditorOnly(string.Format("found {0} pools !", _pools.Count));
#endif
            _root.name = "DinoCore-Spawner";

            Preload();

            this.StartCoroutine("_despawnExcuter");
        }

        /// <summary>
        /// release memory and delete objects in pool
        /// </summary>
        public void ReleaseTracedObjects()
        {
        }

        /// <summary>
        /// Spawn Object
        /// </summary>
        /// <param name="_poolName">Pool Name of the prefab</param>
        /// <param name="_key">key in the pool of the prefab</param>
        /// <param name="_position">world position of the prefab instance</param>
        /// <param name="_quat">Quaternion of the prefab instance</param>
        /// <param name="_parent">parent of the prefab instance</param>
        /// <param name="_active">active?</param>
        /// <returns></returns>
        public Transform Spawn(string _poolName, string _key, Vector3 _position, Quaternion _quat, Transform _parent, bool _active)
        {
            // find in reuse pool first
            Transform _trans = SearchReusePool(string.Format("{0}|{1}", _poolName, _key));
            if (_trans)
            {
                _trans.SetParent(_parent);
                _trans.position = _position;
                _trans.rotation = _quat;
                _trans.gameObject.SetActive(_active);
                return _trans;
            }

            //Dospawn
            return Dospawn(_poolName, _key, _position, _quat, _parent, _active);
        }
        public Transform Spawn(string _poolName, string _key, Vector3 _position, Quaternion _quat, Transform _parent)
        {
            return Spawn(_poolName, _key, _position, _quat, _parent, true);
        }
        public Transform Spawn(string _poolName, string _key, Vector3 _position, Quaternion _quat)
        {
            return Spawn(_poolName, _key, _position, _quat, _root, true);
        }
        public Transform Spawn(string _poolName, string _key, Vector3 _position)
        {
            // nothing in reuse pool, try create a new one
            _handleSpawnPool = GetPool(_poolName);

            // invalid key
            if (_handleSpawnPool == null)
            {
#if DINO_DEBUG
                this.LogEditorOnly(string.Format("invalid key {0}-{1}!", _poolName, _key));
#endif
                return null;
            }

            if (_handleSpawnPool[_key] == null)
            {
#if DINO_DEBUG
                this.LogEditorOnly(string.Format("no key {0}-{1}!", _poolName, _key));
#endif
                return null;
            }

            return Spawn(_poolName, _key, _position, _handleSpawnPool[_key].Resouces.rotation, _root, true);
        }
        public Transform Spawn(string _poolName, string _key)
        {
            // nothing in reuse pool, try create a new one
            _handleSpawnPool = GetPool(_poolName);

            // invalid key
            if (_handleSpawnPool == null)
            {
#if DINO_DEBUG
                this.LogEditorOnly(string.Format("invalid key {0}-{1}!", _poolName, _key));
#endif
                return null;
            }

            if (_handleSpawnPool[_key] == null)
            {
#if DINO_DEBUG
                this.LogEditorOnly(string.Format("no key {0}-{1}!", _poolName, _key));
#endif
                return null;
            }

            return Spawn(_poolName, _key, _handleSpawnPool[_key].Resouces.position, _handleSpawnPool[_key].Resouces.rotation, _root, true);
        }

        /// <summary>
        /// Despawn object
        /// </summary>
        /// <param name="_trans">trans</param>
        /// <param name="_delay">delay</param>
        public void Despawn(Transform _trans, float _delay)
        {
            if (!_trans)
            {
                return;
            }

            // check valid
            var _id = _trans.name.Split('#')[0].Split('|');

            if (GetPool(_id[0]) == null)
            {
                // no pool
#if DINO_DEBUG
                this.LogEditorOnly(string.Format("count not despawn {0} because found no pool!", _trans));
#endif
                return;
            }
            else if (GetPool(_id[0])[_id[1]] == null)
            {
                // no key
#if DINO_DEBUG
                this.LogEditorOnly(string.Format("count not despawn {0} because found no key!", _trans));
#endif
                return;
            }

            // if no delay
            if (_delay == 0)
            {
                AddReusePool(string.Format("{0}|{1}", _id[0], _id[1]), _trans);
                return;
            }

            _despawnOrders.Add(DespawnOrder.create(_trans, string.Format("{0}|{1}", _id[0], _id[1]), Time.time + _delay));
        }
        public void Despawn(Transform _trans)
        {
            Despawn(_trans, 0);
        }

        /// <summary>
        /// Force destroy object without objects pool
        /// </summary>
        /// <param name="_trans"></param>
        /// <param name="_delay"></param>
        public void ForceDespawn(Transform _trans, float _delay)
        {
            Destroy(_trans, _delay);
        }
        public void ForceDespawn(Transform _trans)
        {
            ForceDespawn(_trans, 0);
        }

        /// <summary>
        /// get spawnpool
        /// </summary>
        /// <param name="_poolName">poolname</param>
        /// <returns></returns>
        public SpawnPool GetPool(string _poolName)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                if (_pools[i].PoolName.Equals(_poolName)) return _pools[i];
            }
#if DINO_DEBUG
            this.LogEditorOnly(string.Format("pool {0} not found", _poolName));
#endif
            return null;
        }

        /// <summary>
        /// prespawn func, not use reusepool
        /// </summary>
        /// <param name="_poolName"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        private Transform Dospawn(string _poolName, string _key, Vector3 _position, Quaternion _quat, Transform _parent, bool _active)
        {
            // nothing in reuse pool, try create a new one
            _handleSpawnPool = GetPool(_poolName);

            // invalid key
            if (_handleSpawnPool == null)
            {
#if DINO_DEBUG
                this.LogEditorOnly(string.Format("invalid key {0}-{1}!", _poolName, _key));
#endif
                return null;
            }

            if (_handleSpawnPool[_key] == null)
            {
#if DINO_DEBUG
                this.LogEditorOnly(string.Format("no key {0}-{1}!", _poolName, _key));
#endif
                return null;
            }

            // limit
            if (_handleSpawnPool[_key].Limit != 0 && _handleSpawnPool[_key].SpawnedCount >= _handleSpawnPool[_key].Limit)
            {
#if DINO_DEBUG
                this.LogEditorOnly(string.Format("pool key {0}-{1} reach limit!", _poolName, _key));
#endif
                return null;
            }

            // log message if needed
            if (!_handleSpawnPool[_key].LogMessage.Equals(""))
            {
#if DINO_DEBUG
                this.LogEditorOnly(_handleSpawnPool[_key].LogMessage);
#endif
            }

            Transform _trans = GameObject.Instantiate(_handleSpawnPool[_key].Resouces, _position, _quat, _root) as Transform;
            _trans.gameObject.SetActive(_active);
            _trans.name = string.Format("{0}|{1}#{2}", _poolName, _key, _handleSpawnPool[_key].SpawnedCount++);
            return _trans;
        }

        /// <summary>
        /// if the there is target id object, return it
        /// </summary>
        /// <param name="_id">strFormat poolName-key</param>
        /// <returns></returns>
        private Transform SearchReusePool(string _id)
        {
            _handledQueue = null;
            _objectsPool.TryGetValue(_id, out _handledQueue);

            if (_handledQueue != null && _handledQueue.Count > 0)
            {
                // remove the last element in the queue
                return _handledQueue.Dequeue();
            }
            // didnot find it
            return null;
        }

        /// <summary>
        /// Add object to reuse pool
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_trans"></param>
        private void AddReusePool(string _id, Transform _trans)
        {
            if (!_trans)
            {
#if DINO_DEBUG
                this.LogEditorOnly(string.Format("object add reuse pool failed because of null !"));
#endif
                return;
            }

            _trans.gameObject.SetActive(false);

            _handledQueue = null;

            _objectsPool.TryGetValue(_id, out _handledQueue);

            if (_handledQueue == null)
            {
                Queue<Transform> _queue = new Queue<Transform>();
                _queue.Enqueue(_trans);
                _objectsPool.Add(_id, _queue);
                return;
            }

            if (_trans) _handledQueue.Enqueue(_trans);
        }

        /// <summary>
        /// excute delay despawn
        /// </summary>
        /// <param name="_trans"></param>
        /// <param name="_id"></param>
        /// <param name="_delay"></param>
        /// <returns></returns>
        private IEnumerator _despawnExcuter()
        {
            while (_isDespawnExcuterRunning)
            {
                for (int i = 0; i < _despawnOrders.Count ; i++)
                {
                    // despawn ?
                    if (_despawnOrders[i].DespawnTime < Time.time)
                    {
                        AddReusePool(_despawnOrders[i].ResID, _despawnOrders[i].Res);
                        _despawnOrders.RemoveAt(i);
                    }
                }

                yield return _despawnTaskTimePiece;
            }
        }

        /// <summary>
        /// Preload objects
        /// </summary>
        private void Preload()
        {
            // search all pools
            for (int i = 0; i < _pools.Count; i++)
            {
                // search all prefabs in pool
                for (int j = 0; j < _pools[i].Count; j++)
                {
                    // reset the spawnedCount
                    _pools[i][j].SpawnedCount = 0;

                    // prespawn transforms user required and add to reusepool
                    for (int k = 0; k < _pools[i][j].Preload; k++)
                    {
                        AddReusePool(string.Format("{0}|{1}", _pools[i].PoolName, _pools[i][j].Resouces.name),
                                     Dospawn(_pools[i].PoolName, _pools[i][j].Resouces.name, Vector3.zero, Quaternion.identity, _root, false));
                    }
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        private void ManagerDispose()
        {
            ReleaseTracedObjects();

            _despawnTaskTimePiece = null;

            _handledQueue = null;

            _handleSpawnPool = null;

            _pools = null;
        }

        protected override void OnDestroyManager()
        {
            ManagerDispose();
        }
    }

    public class DespawnOrder
    {
        public Transform Res { get; set; }
        public float DespawnTime { get; set; }
        public string ResID { get; set;}

        public DespawnOrder(Transform _res, string _id, float _despawnTime)
        {
            Res = _res;
            ResID = _id;
            DespawnTime = _despawnTime;
        }

        public static DespawnOrder create(Transform _res, string _id, float _despawnTime)
        {
            return new DespawnOrder(_res,_id,_despawnTime);
        }
    }

}