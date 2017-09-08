//#define DINO_DEBUG
//if enable debug ,open this Micro

using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace Dino_Core.AssetsUtils
{
    /// <summary>
    /// 类库拓展方法集合
    /// </summary>
    public static class ExtendLib
    {
        public static void DLog(this System.Object _obj, object _message)
        {
#if UNITY_EDITOR
            Debug.Log(string.Format("{0} - {1}",_obj, _message ));
#endif
        }

        public static Type[] GetAllSubTypes(this Type _aBaseClass)
        {
            List<Type> _result = new List<Type>();
            Assembly[] _assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < _assemblies.Length; i++)
            {
                Type[] _subTypes = _assemblies[i].GetTypes();

                for (int x = 0; x < _subTypes.Length; x++)
                {
                    if (_subTypes[x].IsSubclassOf(_aBaseClass))
                    {
                        _result.Add(_subTypes[x]);
                    }
                }
            }

            return _result.ToArray();
        }
    }
}

