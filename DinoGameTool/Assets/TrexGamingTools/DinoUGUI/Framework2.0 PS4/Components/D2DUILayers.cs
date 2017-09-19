using Dino_Core.DinoUGUI;
using System;
using System.Collections.Generic;

public abstract class D2DUILayers<T> : MonoSingleton<T> where T : MonoSingleton<T>
{
    protected UILayer[] _layers;

    protected Dictionary<Type, UILayer> _router = new Dictionary<Type, UILayer>();

    public virtual void InitUI()
    {
        _router.Clear();

        _layers = GetComponentsInChildren<UILayer>();

        for (int i = 0; i < _layers.Length; i++)
        {
            if (_router.ContainsKey(_layers[i].GetType()))
            {
            }
            else
            {
                _router.Add(_layers[i].GetType(), _layers[i]);
                _layers[i].Init();
            }
        }
    }

    public V GetLayer<V>() where V : UILayer
    {
        if (_router.ContainsKey(typeof(V)))
        {
            return _router[typeof(V)] as V;
        }

        return null;
    }
}
