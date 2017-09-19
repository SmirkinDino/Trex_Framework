using UnityEngine;
using System.Collections;
namespace Dino_Core.DinoUNet
{
    [System.Serializable]
    public class Pool
    {

        /// <summary>
        /// 
        /// </summary>
        public string PoolName
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int PoolSize
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private int mP = 0;

        /// <summary>
        /// 
        /// </summary>
        private GameObject[] mObjectsArray;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_poolSize"></param>
        public Pool(string _name, int _poolSize)
        {
            this.PoolName = _name;
            this.PoolSize = _poolSize;
            mObjectsArray = new GameObject[_poolSize];
        }

        /// <summary>
        /// Add object
        /// </summary>
        /// <returns></returns>
        public GameObject getObject()
        {
            // end
            if (mP == 0) return null;

            // 
            GameObject _obj = mObjectsArray[mP - 1];
            mObjectsArray[mP--] = null;
            return _obj;
        }

        /// <summary>
        /// Add object
        /// </summary>
        /// <param name="_obj"></param>
        /// <returns></returns>
        public bool AddObject(GameObject _obj)
        {
            if (this.PoolSize == this.mP) return false;

            //
            mObjectsArray[this.mP++] = _obj;

            return true;
        }


    }
}