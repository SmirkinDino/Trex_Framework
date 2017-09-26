using Dino_Core.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour {

    public TrexSerializableObject _obj = new TrexSerializableObject();

    public void Start()
    {
        TrexDataContainer.Enumerator _enumerator = _obj.DataTable.GetEnumerator();

        while (_enumerator.MoveNext())
        {
            Debug.Log(_enumerator.Current.Key + " " + _enumerator.Current.Value.Value);
        }
    }
}
