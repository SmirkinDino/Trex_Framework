using Dino_Core.Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrexSerializableObjectFieldDrawer {

    public delegate void PropertyHandler(TrexSerializableItem _value, Rect _rect);
    public static Dictionary<TrexObjectType, PropertyHandler> AdaptTable = new Dictionary<TrexObjectType, PropertyHandler>()
    {
        {TrexObjectType.INT, IntDrawer},
        {TrexObjectType.FLOAT, FloatDrawer},
        {TrexObjectType.BOOL, BoolDrawer},
        {TrexObjectType.OBJECT, ObjectDrawer},
        {TrexObjectType.TRANSFORM, TransformDrawer},
        {TrexObjectType.VECTOR2, Vector2Drawer},
        {TrexObjectType.VECTOR3, Vector3Drawer},
        {TrexObjectType.VECTOR4, Vector4Drawer},
        {TrexObjectType.COLOR, ColorDrawer},
        {TrexObjectType.LAYER, LayerMaskDrawer},
    };

    public static void IntDrawer(TrexSerializableItem _value, Rect _rect)
    {
        _value.IntValue = EditorGUI.IntField(_rect, _value.IntValue);
    }
    public static void FloatDrawer(TrexSerializableItem _value, Rect _rect)
    {
        _value.FloatValue = EditorGUI.FloatField(_rect, _value.FloatValue);
    }
    public static void BoolDrawer(TrexSerializableItem _value, Rect _rect)
    {
        _value.BoolValue = EditorGUI.Toggle(_rect, _value.BoolValue);
    }
    public static void ObjectDrawer(TrexSerializableItem _value, Rect _rect)
    {
        _value.ObjectValue = EditorGUI.ObjectField(_rect, _value.ObjectValue, typeof(UnityEngine.Object), true);
    }
    public static void TransformDrawer(TrexSerializableItem _value, Rect _rect)
    {
        _value.TransformValue =  EditorGUI.ObjectField(_rect, _value.TransformValue, typeof(Transform), true) as Transform;
    }
    public static void Vector2Drawer(TrexSerializableItem _value, Rect _rect)
    {
        _value.Vector2Value =  EditorGUI.Vector2Field(_rect, "", _value.Vector2Value);
    }
    public static void Vector3Drawer(TrexSerializableItem _value, Rect _rect)
    {
        _value.Vector3Value =  EditorGUI.Vector3Field(_rect, "", _value.Vector3Value);
    }
    public static void Vector4Drawer(TrexSerializableItem _value, Rect _rect)
    {
        _value.Vector4Value =  EditorGUI.Vector4Field(_rect, "", _value.Vector4Value);
    }
    public static void ColorDrawer(TrexSerializableItem _value, Rect _rect)
    {
        _value.ColorValue =  EditorGUI.ColorField(_rect, _value.ColorValue);
    }
    public static void LayerMaskDrawer(TrexSerializableItem _value, Rect _rect)
    {
        _value.LayerValue = EditorGUI.LayerField(_rect, _value.LayerValue);
    }
}
