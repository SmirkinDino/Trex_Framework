using Dino_Core.Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(TrexSerializableObject))]
public class TrexSerializableObjectDrawer : PropertyDrawer {

    protected TrexSerializableObject _container;
    protected TrexSerializableItem _targetObject;

    protected ReorderableList _reorderList;

    protected TrexObjectType _addingType;
    protected string _addingKey;
    protected bool _isDraw;

    protected static readonly float LABEL_WIDTH_KEY = 80.0f;
    protected static readonly float LABEL_WIDTH_TYPE = 120.0f;
    protected static readonly float LABEL_WIDTH_VALUE = 208.0f;
    protected static readonly float LABEL_WIDTH_BUTTON = 20.0f;

    protected static readonly float EMPTY_PADDING = 64.0f;
    protected static readonly float EMPTY_PADDING_HEIGHT = 4.0f;
    protected static readonly float EMPTY_MARGIN_HEIGHT = 1.0f;
    protected static readonly float BUTTON_MARGIN_HEIGHT = 1.0f;
    public void InitIfNot(SerializedProperty property)
    {
        if (_container == null)
        {
            _container = fieldInfo.GetValue(property.serializedObject.targetObject) as TrexSerializableObject;

            _reorderList = new ReorderableList(_container.Values, typeof(TrexSerializableItem),true, true, true, true);

            _reorderList.drawHeaderCallback = (Rect rect) => 
            {
                EditorGUI.LabelField(rect, "ContainerTable");
            };

            _reorderList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                try
                {
                    _targetObject = _reorderList.list[index] as TrexSerializableItem;
                }
                catch (Exception)
                {
                    return;
                }

                Rect _origin = new Rect(rect);

                rect.height -= EMPTY_PADDING_HEIGHT;
                rect.y += EMPTY_MARGIN_HEIGHT;

                // type
                EditorGUI.LabelField(rect, _targetObject.type.ToString());
                rect.x += LABEL_WIDTH_TYPE;

                // key
                EditorGUI.LabelField(rect, _targetObject.Key);
                rect.x += LABEL_WIDTH_KEY;
                rect.width -= LABEL_WIDTH_TYPE + LABEL_WIDTH_KEY + LABEL_WIDTH_BUTTON;

                // value
                TrexSerializableObjectFieldDrawer.AdaptTable[_targetObject.type](_targetObject, rect);
                rect.x = _origin.width + 16;
                rect.height -= BUTTON_MARGIN_HEIGHT;
                rect.width = LABEL_WIDTH_BUTTON;

                // apply
                _container[_targetObject.Key] = _targetObject;

                // button
                if (GUI.Button(rect, "x", EditorStyles.miniButtonMid))
                {
                    if (_container.DataTable.ContainsKey(_targetObject.Key))
                    {
                        _container.DataTable.Remove(_targetObject.Key);
                    }
                    _reorderList.list.Remove(_targetObject);
                }
            };

            _reorderList.drawFooterCallback = (Rect rect) =>
            {
                // key
                rect.height = EditorGUIUtility.singleLineHeight + 2.0f;
                rect.width = rect.width / 3 - 3.8f;
                _addingKey = EditorGUI.TextField(rect, _addingKey);

                // type
                rect.x += rect.width + 4.0f;
                _addingType = (TrexObjectType)EditorGUI.EnumPopup(rect, _addingType, EditorStyles.toolbarPopup);

                // button
                rect.x += rect.width + 4.0f;
                if (GUI.Button(rect, "ADD", EditorStyles.toolbarButton))
                {
                    if (string.IsNullOrEmpty(_addingKey) || _container.DataTable.ContainsKey(_addingKey))
                    {
                        return;
                    }

                    TrexSerializableItem _item = new TrexSerializableItem();
                    _item.InitItem(_addingType, _addingKey);

                    _container.DataTable.Add(_addingKey, _item);
                    _reorderList.list.Add(_item);
                }
            };
            _isDraw = true;
        }
    }

    public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
    {
        InitIfNot(_property);
        return Mathf.Max(0, (_reorderList.list.Count - 1)) * (EditorGUIUtility.singleLineHeight + 5.0f) + EMPTY_PADDING;
    }
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        InitIfNot(_property);

        EditorGUI.BeginProperty(_position, _label, _property);
        {
            _reorderList.DoList(_position);
        }
        EditorGUI.EndProperty();
    }
}