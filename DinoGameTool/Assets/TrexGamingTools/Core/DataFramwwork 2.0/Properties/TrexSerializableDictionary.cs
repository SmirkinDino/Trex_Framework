/// <summary>
/// Trex Game Data Framework 2.0
/// for serilization, please use simple type as value, comlex type maybe cause
/// multiseriliation problems.
/// 
/// RunningTrex 2017.09.08
/// </summary>
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dino_Core.Core
{
    [System.Serializable]
    public class TrexSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> _keys;
        [SerializeField] private List<TValue> _values;

        [NonSerialized] private Enumerator _enumerator;
        [NonSerialized] private List<KeyValuePair<TKey, TValue>> _keyValuePairs;

        public List<KeyValuePair<TKey, TValue>> KeyValuePairs
        {
            get
            {
                _keyValuePairs.Clear();

                OnBeforeSerialize();

                for (int i = 0; i < _keys.Count; i++)
                {
                    _keyValuePairs.Add(new KeyValuePair<TKey, TValue>(_keys[i], _values[i]));
                }

                return _keyValuePairs;
            }
        }

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _keys.Capacity = this.Count;

            _values.Clear();
            _values.Capacity = this.Count;

            _enumerator = this.GetEnumerator();
            while (_enumerator.MoveNext())
            {
                _keys.Add(_enumerator.Current.Key);
                _values.Add(_enumerator.Current.Value);
            }
        }
        public void OnAfterDeserialize()
        {
            Clear();

            try
            {
                for (int i = 0; i < _keys.Count; ++i)
                {
                    Add(_keys[i], _values[i]);
                }
            }
            catch (Exception)
            {
                this.DLog("key - value dosn't match");
            }
        }
        public TrexSerializableDictionary() : base()
        {
            _values = new List<TValue>();
            _keys = new List<TKey>();
            _keyValuePairs = new List<KeyValuePair<TKey, TValue>>();
        }
    }
}
