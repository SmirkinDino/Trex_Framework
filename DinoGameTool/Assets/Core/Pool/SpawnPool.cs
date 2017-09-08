using UnityEngine;
using System;
using System.Collections.Generic;

namespace Dino_Core.AssetsUtils
{
    [Serializable]
    public class SpawnPool
    {
        public string PoolName = "";

        public List<SpawnPrefab> _pool = new List<SpawnPrefab>();

        public SpawnPrefab this[int index]
        {
            get
            {
                return _pool[index];
            }
            set
            {
                this.DLog("Not impliment set func");
                throw new NotImplementedException();
            }
        }

        public SpawnPrefab this[string key]
        {
            get
            {
                for (int i = 0; i < _pool.Count; i++)
                {
                    if (_pool[i].Resouces.name.Equals(key)) return _pool[i];
                }
                this.DLog(string.Format("not found key : {0}", key));
                return null;
            }
            set
            {
                this.DLog("Not impliment set func");
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                return _pool.Count;
            }
        }

        public int Add(object value)
        {
            _pool.Add(value as SpawnPrefab);
            return _pool.Count;
        }

        public void RemoveAt(int index)
        {
            _pool.RemoveAt(index);
        }
    }
}