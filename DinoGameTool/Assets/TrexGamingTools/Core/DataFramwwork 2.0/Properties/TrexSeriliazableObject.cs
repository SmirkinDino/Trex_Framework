/// <summary>
/// Trex Game Data Framework 2.0
/// for serilization, please use simple type as value, comlex type maybe cause
/// multiseriliation problems.
/// 
/// RunningTrex 2017.09.08
/// </summary>
using System.Collections.Generic;
using UnityEngine;
namespace Dino_Core.Core
{
    [System.Serializable]
    public sealed class TrexSeriliazableObject : ScriptableObject
    {
        [SerializeField] private TrexSeriliazableDictionary<string, object> _dataTable = new TrexSeriliazableDictionary<string, object>();

        [System.NonSerialized] private TrexSeriliazableDictionary<string, object>.KeyCollection.Enumerator _keyEnumerator;
        [System.NonSerialized] private List<string> _keys = new List<string>();

        public object this[string _key]
        {
            get
            {
                return _dataTable[_key];
            }
            set
            {
                _dataTable[_key] = value;
            }
        }

        /// <summary>
        /// it won't create a new location in memory, so care the operation out side
        /// </summary>
        public string[] Keys
        {
            get
            {
                _keys.Clear();

                _keyEnumerator = _dataTable.Keys.GetEnumerator();
                while (_keyEnumerator.MoveNext())
                {
                    _keys.Add(_keyEnumerator.Current);
                }

                return _keys.ToArray();
            }
        }
    }
}