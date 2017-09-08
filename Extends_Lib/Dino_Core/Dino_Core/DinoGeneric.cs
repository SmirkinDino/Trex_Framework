using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Dino_Core
{
    public static class DinoGeneric
    {
        /// <summary>
        /// 添加实体进Dic
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="_targetDic"></param>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        public static void AddEntity<K, V>(this Dictionary<K, V> _targetDic,K _key ,V _value)
        {
            if (_key == null)
            {
                //Debug.Log("Key值为null");
                return;
            }

            if (_value == null)
            {
                //Debug.Log("Value值为null");
            }

            if (_targetDic.ContainsKey(_key))
            {
                //Debug.Log(_key.ToString() + "已经包含在" + _targetDic.ToString() + "中！请勿重复添加");
            }
            else
            {
                _targetDic.Add(_key,_value);
            }
        }

        /// <summary>
        /// 得到字典中实体
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="_targetDic"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public static V GetEntity<K, V>(this Dictionary<K, V> _targetDic, K _key)
        {
            if (_key == null)
            {
                //Debug.Log("Key值为null");
                return default(V);
            }

            if (_targetDic.ContainsKey(_key))
            {
                return _targetDic[_key];
            }
            else
            {
                //Debug.Log(_key.ToString() + "不存在" + _targetDic.ToString() + "中！");
                return default(V);
            }
        }

        /// <summary>
        /// 从字典中移除目标
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="_targetDic"></param>
        /// <param name="_key"></param>
        public static void RemoveEntity<K, V>(this Dictionary<K, V> _targetDic, K _key)
        {
            if (_key == null)
            {
                //Debug.Log("Key值为null");
            }

            if (_targetDic.ContainsKey(_key))
            {
                _targetDic.Remove(_key);
            }
            else
            {
                //Debug.Log(_key.ToString() + "不存在" + _targetDic.ToString() + "中！");
            }
        }

        /// <summary>
        /// 倒叙链表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_targetList"></param>
        public static void InvertedOrder<T>(this List<T> _targetList)
        {
            int _head = 0;
            int _tail = _targetList.Count - 1;
            T _temp = default(T);

            while (_head < _tail)
            {
                _temp = _targetList[_head];
                _targetList[_head] = _targetList[_tail];
                _targetList[_tail] = _temp;

                _head++;
                _tail--;
            }
        }

        /// <summary>
        /// 倒叙数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_targetArray"></param>
        public static void InvertedOrder<T>(this T[] _targetArray)
        {
            int _head = 0;
            int _tail = _targetArray.Length - 1;
            T _temp = default(T);

            while (_head < _tail)
            {
                _temp = _targetArray[_head];
                _targetArray[_head] = _targetArray[_tail];
                _targetArray[_tail] = _temp;

                _head++;
                _tail--;
            }
        }

        /// <summary>
        /// 从数组中获取最大的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_targetArray"></param>
        /// <returns></returns>
        public static float GetMaxEntity<T>(this float[] _targetArray)
        {
            if(_targetArray == null || _targetArray.Length == 0)
            {
                return default(float);
            }

            float _result = 0;

            for (int i = 0; i < _targetArray.Length;i++)
            {
                if (_result < _targetArray[i])
                {
                    _result = _targetArray[i];
                }
            }

            return _result;
        }
        public static int GetMaxEntity<T>(this int[] _targetArray)
        {
            if (_targetArray == null || _targetArray.Length == 0)
            {
                return default(int);
            }

            int _result = 0;

            for (int i = 0; i < _targetArray.Length; i++)
            {
                if (_result < _targetArray[i])
                {
                    _result = _targetArray[i];
                }
            }

            return _result;
        }
    }
}
