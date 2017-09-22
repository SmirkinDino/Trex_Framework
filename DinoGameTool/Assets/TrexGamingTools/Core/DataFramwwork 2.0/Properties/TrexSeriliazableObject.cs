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
    public sealed class TrexSeriliazableObject : PropertyAttribute
    {
        [SerializeField] private TrexDataContainer _dataTable;
        public TrexDataContainer DataTable { get { return _dataTable; } set { _dataTable = value; } }

        [System.NonSerialized] private TrexDataContainer.KeyCollection.Enumerator _keyEnumerator;
        [System.NonSerialized] private TrexDataContainer.ValueCollection.Enumerator _valueEnumerator;
        [System.NonSerialized] private List<string> _keys;
        [System.NonSerialized] private List<TrexSeriliazableItem> _values;

        public object this[string _key] { get { return _dataTable[_key].Value; } set { _dataTable[_key].Value = value; } }

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
        /// <summary>
        /// it won't create a new location in memory, so care the operation out side
        /// </summary>
        public object[] Values
        {
            get
            {
                _values.Clear();

                _valueEnumerator = _dataTable.Values.GetEnumerator();
                while (_valueEnumerator.MoveNext())
                {
                    _values.Add(_valueEnumerator.Current);
                }

                return _values.ToArray();
            }
        }

        public TrexSeriliazableObject()
        {
            _dataTable = new TrexDataContainer();
            _keys = new List<string>();
            _values = new List<TrexSeriliazableItem>();
        }
        public bool SerializeObject(string _filePath)
        {
            FileInfo _fileInfo = new FileInfo(_filePath);

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

    public sealed class TrexDataContainer : TrexSeriliazableDictionary<string, TrexSeriliazableItem> { public TrexDataContainer() : base() { } }
}