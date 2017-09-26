using UnityEngine;
namespace Dino_Core.Core
{
    [System.Serializable]
    public class TrexSerializableItem
    {
        [SerializeField] private string _key = "non";
        [SerializeField] private TrexObjectType _type;

        public object Value
        {
            get
            {
                switch (_type)
                {
                    case TrexObjectType.INT: return IntValue;
                    case TrexObjectType.FLOAT: return FloatValue;
                    case TrexObjectType.BOOL: return BoolValue;
                    case TrexObjectType.VECTOR2: return Vector2Value;
                    case TrexObjectType.VECTOR3: return Vector3Value;
                    case TrexObjectType.VECTOR4: return Vector4Value;
                    case TrexObjectType.OBJECT: return ObjectValue;
                    case TrexObjectType.TRANSFORM: return TransformValue;
                    case TrexObjectType.COLOR: return ColorValue;
                    case TrexObjectType.LAYER: return LayerValue;
                }

                return default(object);
            }
        }

        public int IntValue;
        public float FloatValue;
        public bool BoolValue;
        public Vector2 Vector2Value;
        public Vector3 Vector3Value;
        public Vector4 Vector4Value;
        public Color ColorValue;
        public Transform TransformValue;
        public LayerMask LayerValue;
        public Object ObjectValue;

        public TrexObjectType type { get { return _type; } }
        public string Key { get { return _key; } }

        public void InitItem(TrexObjectType _type, string _key)
        {
            this._type = _type;
            this._key = _key;
        }
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
        OBJECT,
        TRANSFORM,
        LAYER
    }
}