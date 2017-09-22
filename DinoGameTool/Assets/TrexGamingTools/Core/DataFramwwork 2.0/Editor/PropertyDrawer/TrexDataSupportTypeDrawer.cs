using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TrexDataSupportTypeDrawer {

    public delegate object PropertyHandler(object _value, Rect _rect);

    public static Dictionary<Dino_Core.Core.TrexObjectType, PropertyHandler> AdaptTable = new Dictionary<Dino_Core.Core.TrexObjectType, PropertyHandler>()
    {
        {Dino_Core.Core.TrexObjectType.INT, IntDrawer},
        {Dino_Core.Core.TrexObjectType.FLOAT, FloatDrawer},
        {Dino_Core.Core.TrexObjectType.BOOL, BoolDrawer},
        {Dino_Core.Core.TrexObjectType.TRANSFORM, TransformDrawer},
        {Dino_Core.Core.TrexObjectType.VECTOR2, Vector2Drawer},
        {Dino_Core.Core.TrexObjectType.VECTOR3, Vector3Drawer},
        {Dino_Core.Core.TrexObjectType.VECTOR4, Vector4Drawer},
        {Dino_Core.Core.TrexObjectType.COLOR, ColorDrawer},
    };

    public static object IntDrawer(object _value, Rect _rect) { return EditorGUI.IntField(_rect, (int)_value); }
    public static object FloatDrawer(object _value, Rect _rect) { return EditorGUI.FloatField(_rect, (float)_value); }
    public static object BoolDrawer(object _value, Rect _rect) { return EditorGUI.Toggle(_rect, (bool)_value); }
    public static object TransformDrawer(object _value, Rect _rect) { return EditorGUI.ObjectField(_rect, (Transform)_value, typeof(Transform), false); }
    public static object Vector2Drawer(object _value, Rect _rect) { return EditorGUI.Vector2Field(_rect, "", (Vector2)_value); }
    public static object Vector3Drawer(object _value, Rect _rect) { return EditorGUI.Vector3Field(_rect, "", (Vector3)_value); }
    public static object Vector4Drawer(object _value, Rect _rect) { return EditorGUI.Vector4Field(_rect, "", (Vector4)_value); }
    public static object ColorDrawer(object _value, Rect _rect) { return EditorGUI.ColorField(_rect, (Color)_value); }
}
