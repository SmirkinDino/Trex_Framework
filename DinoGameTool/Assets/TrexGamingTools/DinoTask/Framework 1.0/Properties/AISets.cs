using System.Collections.Generic;
using UnityEngine;

public class AISets
{
    public int Count
    {
        get
        {
            return _router.Count;
        }
    }
    protected List<Transform> _router = new List<Transform>();
    public void Add(Transform _trans)
    {
        _router.Add(_trans);
    }
    public void Remove(Transform _trans)
    {
        _router.Remove(_trans);
    }
}
