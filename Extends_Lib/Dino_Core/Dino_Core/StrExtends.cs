using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dino_Core
{

    /// <summary>
    /// 字符串拓展集
    /// </summary>
    public static class StrExtends
    {
        /// <summary>
        /// 将字符串转换为整型
        /// </summary>
        /// <param name="_str">要转换的字符串</param>
        /// <returns>转换成功的整型</returns>
        public static int SwitchToInteger( string _str)
        {
            return int.Parse(_str);
        }

        /// <summary>
        /// 将字符串转换为浮点型
        /// </summary>
        /// <param name="_str">要转换的字符串</param>
        /// <returns>转换成功的浮点型</returns>
        public static float SwitchToFloat( string _str)
        {
            return float.Parse(_str);
        }

        /// <summary>
        /// 将字符串转换为二维向量
        /// </summary>
        /// <param name="_str">要转换的字符串</param>
        /// <returns>转换成功的二维向量</returns>
        public static Vector2 SwitchToVector2( string _str)
        {
            float[] _vecFloat = SwitchToVector(_str, 2);
            return new Vector4(_vecFloat[0], _vecFloat[1]);
        }

        /// <summary>
        /// 将字符串转换为三维向量
        /// </summary>
        /// <param name="_str">要转换的字符串</param>
        /// <returns>转换成功的三维向量</returns>
        public static Vector3 SwitchToVector3( string _str)
        {
            float[] _vecFloat = SwitchToVector(_str, 3);
            return new Vector4(_vecFloat[0], _vecFloat[1], _vecFloat[2]);
        }

        /// <summary>
        /// 将字符串转换为四维向量
        /// </summary>
        /// <param name="_str">要转换的字符串</param>
        /// <returns>转换成功的四维向量</returns>
        public static Vector4 SwitchToVector4( string _str)
        {
            float[] _vecFloat = SwitchToVector(_str, 4);
            return new Vector4(_vecFloat[0], _vecFloat[1], _vecFloat[2], _vecFloat[3]);
        }

        /// <summary>
        /// 将字符串转换为向量，内部使用，返回一个float的数组
        /// </summary>
        /// <param name="_str">要转换的字符串</param>
        /// <returns>转换成功的数组</returns>
        public static float[] SwitchToVector( string _str, int _dimension)
        {
            string[] _vecStr = _str.Split(',');
            float[] _vecFloat = new float[_dimension];

            if (_vecStr.Length < _dimension)
            {
                Debug.LogError("非法字符");
                return null;
            }

            for (int i = 0; i < _dimension; i++)
            {
                _vecFloat[i] = float.Parse(_vecStr[i]);
            }

            return _vecFloat;
        }

        /// <summary>
        /// 将字符串转换为布尔型
        /// </summary>
        /// <param name="_str">转换的对象</param>
        /// <returns>返回转换的结果</returns>
        public static bool SwitchToBool( string _str)
        {
            return _str.Equals("True") ? true : false;
        }

        /// <summary>
        /// 将字符串转换为整型数组
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static int[] SwitchToIntArray( string _str)
        {
            string[] _valueStr = _str.Split(' ');
            int[] _value = new int[_valueStr.Length];

            for (int j = 0; j < _value.Length; j++)
                _value[j] = int.Parse(_valueStr[j]);

            return _value;
        }

        /// <summary>
        /// 将字符串转换为浮点型数组
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static float[] SwitchToFloatArray(string _str)
        {
            string[] _valueStr = _str.Split(' ');
            float[] _value = new float[_valueStr.Length];

            for (int j = 0; j < _value.Length; j++)
                _value[j] = float.Parse(_valueStr[j]);

            return _value;
        }
    }

}