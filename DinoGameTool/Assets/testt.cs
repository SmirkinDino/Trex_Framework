using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testt : MonoBehaviour {

    public Transform Trans;

	// Use this for initialization
	void Start () {
        Transform _tran = Instantiate(Trans);
        Debug.Log(_tran.GetComponentInChildren<ScriptableObject>());
    }

}
