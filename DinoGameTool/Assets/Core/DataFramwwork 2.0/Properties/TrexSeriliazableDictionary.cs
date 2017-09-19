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
    public sealed class TrexSeriliazableDictionary<TKey, TValue> :  Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        public new TValue this[TKey _key]
        {
            get
            {
                if (this.ContainsKey(_key))
                {
                    return this[_key];
                }

                this.DLog(_key + " not this key");

                return default(TValue);
            }
            set
            {
                if (this == null)
                {
                    return;
                }

                if (this.ContainsKey(_key))
                {
                    this[_key] = value;
                }
                else
                {
                    this.Add(_key, value);
                }
            }
        }

        [SerializeField] private List<TKey> _keys = new List<TKey>();
        [SerializeField] private List<TValue> _values = new List<TValue>();

        [NonSerialized] private Enumerator _enumerator;

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            _keys.Capacity = this.Count;
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
    }

}
