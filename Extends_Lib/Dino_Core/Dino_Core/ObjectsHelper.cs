using System;
using UnityEngine;
using System.Collections;

namespace Dino_Core
{
    public static class ObjectsHelper
    {
        /// <summary>
        /// 深度优先遍历所有子物体物体，不会返回要求查找的物体本身
        /// </summary>
        /// <param name="_parent">要求查找的对象</param>
        /// <param name="_resultList">结果列表</param>
        /// <returns></returns>
        public static ArrayList GetAllChildOfAParent(GameObject _parent, ArrayList _resultList)
        {
            for (int i = 0; i < _parent.transform.childCount; i++)
            {
                GameObject _child = _parent.transform.GetChild(i).gameObject;
                _resultList.Add(_child);
                _resultList = GetAllChildOfAParent(_child, _resultList);
            }

            return _resultList;
        }
    }
}