using Dino_Core.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(TrexSeriliazableObject))]
public class TrexSeriliazableObjectDrawer : PropertyDrawer {

    protected TrexSeriliazableObject _container;
    protected ReorderableList _reorderList;
    protected bool _isDraw;
    protected Dino_Core.Core.TrexObjectType _addingType;
    protected string _addingKey;

    public void InitIfNot(SerializedProperty property)
    {
        if (_container == null)
        {
            var _target = property.serializedObject.targetObject;
            _container = fieldInfo.GetValue(_target) as TrexSeriliazableObject;

            _reorderList = new ReorderableList(_container.DataTable.GetList(), typeof(KeyValuePair<string, TrexSeriliazableItem>),true, true, true, true);
            _reorderList.onAddCallback = OnAddEventHandler;
            _reorderList.drawElementCallback = OnDrawTrexObjectEventHandler;
            _reorderList.onRemoveCallback = OnRemoveEventHandler;
            _isDraw = true;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        InitIfNot(property);
        return (_container.DataTable.Count + 4) * (EditorGUIUtility.singleLineHeight + 5.0f);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        InitIfNot(property);

        EditorGUI.BeginProperty(position, label, property);
        position.y += 5.0f;

        Rect _typeRect = new Rect(position);
        _typeRect.width = 110.0f;
        _typeRect.height = EditorGUIUtility.singleLineHeight;
        if (GUI.Button(_typeRect, "Save"))
        {
            // save logic
        }

        _typeRect.x += _typeRect.width + 10.0f;
        _typeRect.width = 110.0f;
        _addingType = (Dino_Core.Core.TrexObjectType)EditorGUI.EnumPopup(_typeRect, _addingType);

        _typeRect.x += _typeRect.width + 10.0f;
        _typeRect.width = 30.0f;
        EditorGUI.LabelField(_typeRect, "Key:");

        _typeRect.x += _typeRect.width + 10.0f;
        _typeRect.width = position.width - 332.0f;
        _typeRect.width = 160.0f;
        _addingKey = EditorGUI.TextField(_typeRect, _addingKey);

        position.y += 20.0f;
        _reorderList.DoList(position);

        EditorGUI.EndProperty();
    }
    private void OnDrawTrexObjectEventHandler(Rect rect, int index, bool selected, bool focused)
    {
        KeyValuePair<string, TrexSeriliazableItem> _item = _container.DataTable.ToList()[index];

        rect.height = EditorGUIUtility.singleLineHeight;
        rect.width = 80.0f;
        EditorGUI.LabelField(rect, _item.Value.type.ToString());

        rect.x += 85.0f;
        rect.width = 140.0f;
        EditorGUI.LabelField(rect, _container.DataTable.ToList()[index].Key);

        rect.x += 145.0f;
        rect.y += 1.25f;
        rect.width = 140.0f;
        _container.DataTable[_item.Key].Value = TrexDataSupportTypeDrawer.AdaptTable[_item.Value.type](_item.Value.Value, rect);
    }
    private void OnAddEventHandler(ReorderableList _list)
    {
        if (_container.DataTable.ContainsKey(_addingKey))
        {
            return;
        }

        TrexSeriliazableItem _item = new TrexSeriliazableItem();
        _item.type = _addingType;
        _container.DataTable.Add(_addingKey, _item);
        RefreshAndSaveData();
    }
    private void OnRemoveEventHandler(ReorderableList list)
    {
        KeyValuePair<string, TrexSeriliazableItem> _item = (list.list as List<KeyValuePair<string, TrexSeriliazableItem>>)[_reorderList.index];
        _container.DataTable.Remove(_item.Key);
        RefreshAndSaveData();
    }
    private void RefreshAndSaveData()
    {
        _reorderList.list = _container.DataTable.GetList();
    }
}
