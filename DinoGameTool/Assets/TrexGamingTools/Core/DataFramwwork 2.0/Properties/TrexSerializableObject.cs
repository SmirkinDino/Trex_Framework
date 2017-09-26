/// <summary>
/// Trex Game Data Framework 2.0
/// for serilization, please use simple type as value, comlex type maybe cause
/// multiseriliation problems.
/// 
/// RunningTrex 2017.09.08
/// </summary>
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Dino_Core.Core
{
    [System.Serializable]
    public sealed class TrexSerializableObject : PropertyAttribute
    {
        [SerializeField] private TrexDataContainer _dataTable;
        public TrexDataContainer DataTable { get { return _dataTable; } set { _dataTable = value; } }

        [System.NonSerialized] private TrexDataContainer.KeyCollection.Enumerator _keyEnumerator;
        [System.NonSerialized] private TrexDataContainer.ValueCollection.Enumerator _valueEnumerator;

        [SerializeField] private List<string> _keys;
        [SerializeField] private List<TrexSerializableItem> _values;

        public TrexSerializableItem this[string _key] { get { return _dataTable[_key]; } set { _dataTable[_key] = value; } }

        /// <summary>
        /// it won't create a new location in memory, so care the operation out side
        /// </summary>
        public List<string> Keys
        {
            get
            {
                _keys.Clear();

                _keyEnumerator = _dataTable.Keys.GetEnumerator();
                while (_keyEnumerator.MoveNext())
                {
                    _keys.Add(_keyEnumerator.Current);
                }

                return _keys;
            }
        }
        /// <summary>
        /// it won't create a new location in memory, so care the operation out side
        /// </summary>
        public List<TrexSerializableItem> Values
        {
            get
            {
                _values.Clear();

                _valueEnumerator = _dataTable.Values.GetEnumerator();
                while (_valueEnumerator.MoveNext())
                {
                    _values.Add(_valueEnumerator.Current);
                }

                return _values;
            }
        }

        public TrexSerializableObject()
        {
            _dataTable = new TrexDataContainer();
            _keys = new List<string>();
            _values = new List<TrexSerializableItem>();
        }

        public bool SerializeObject(string _filePath)
        {
            return true;
        }
        public bool DeserializeObject(string _filePath)
        {
            return true;
        }
        public bool DeserializeObjectFromCache(string _filePath)
        {
            return true;
        }
    }

    [System.Serializable]
    public sealed class TrexDataContainer : TrexSerializableDictionary<string, TrexSerializableItem>
    {
        public TrexDataContainer() : base() { }

    }
}