using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    #region SINGLETON
    protected static T _managerInstance = null;
    protected MonoSingleton() { }
    public static Transform _root{ get; protected set; }
    public static T Instance
    {
        get
        {
            if (_root == null)
            {
                _managerInstance = null;

                _root = new GameObject("Dino_Core-" + typeof(T).ToString()).transform;

                _managerInstance = _root.gameObject.AddComponent<T>();

                DontDestroyOnLoad(_root.gameObject);
            }
            else
            {
                _managerInstance = _root.GetComponent<T>();
                if (_managerInstance == null)
                {
                    _managerInstance = _root.gameObject.AddComponent<T>();
                }
            }

            if (_root == null || _managerInstance == null)
            {
#if DINO_DEBUG
                //DScreemLogger.Instance.LogToScreen("Instance Error");
#endif
            }

            return _managerInstance;
        }
    }
    #endregion

    protected void OnDestroy()
    {
        _managerInstance = null;
        _root = null;
        OnDestroyManager();
    }
    protected virtual void OnDestroyManager() { }
}
