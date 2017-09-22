using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dino_Core.Core
{
    [System.Serializable]
    public class TrexSeriliazableItem
    {
        public object Value;
        public TrexObjectType type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;

                if (_type == TrexObjectType.INT)
                {
                    Value = 0;
                }
                else if (_type == TrexObjectType.FLOAT)
                {
                    Value = 0.0f;
                }
                else if (_type == TrexObjectType.BOOL)
                {
                    Value = false;
                }
                else if (_type == TrexObjectType.COLOR)
                {
                    Value = new Color();
                }
                else if (_type == TrexObjectType.VECTOR2)
                {
                    Value = new Vector2();
                }
                else if (_type == TrexObjectType.VECTOR3)
                {
                    Value = new Vector3();
                }
                else if (_type == TrexObjectType.VECTOR4)
                {
                    Value = new Vector4();
                }
            }
        }
        private TrexObjectType _type;
    }

    public enum TrexObjectType
    {
        INT,
        FLOAT,
        BOOL,
        VECTOR2,
        VECTOR3,
        VECTOR4,
        COLOR,
        TRANSFORM
    }
}