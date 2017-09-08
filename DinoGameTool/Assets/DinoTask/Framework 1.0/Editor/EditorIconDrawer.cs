using Dino_Core.Task;
using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EditorIconDrawer {

    private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;

    private static Texture2D _taskIcon;
    private static Texture2D TaskIcon
    {
        get
        {
            if (EditorIconDrawer._taskIcon == null)
            {
                _taskIcon = (Texture2D)Resources.Load("TaskIcon");
            }

            return EditorIconDrawer._taskIcon;
        }
    }

    private static Texture2D _eventIcon;
    private static Texture2D EventIcon
    {
        get
        {
            if (EditorIconDrawer._eventIcon == null)
            {
                _eventIcon = (Texture2D)Resources.Load("EventIcon");
            }

            return EditorIconDrawer._eventIcon;
        }
    }

    private static Texture2D _triggerIcon;
    private static Texture2D TriggerIcon
    {
        get
        {
            if (EditorIconDrawer._triggerIcon == null)
            {
                _triggerIcon = (Texture2D)Resources.Load("TriggerIcon");
            }

            return EditorIconDrawer._triggerIcon;
        }
    }

    private static Texture2D _rootIcon;
    private static Texture2D RootIcon
    {
        get
        {
            if (EditorIconDrawer._rootIcon == null)
            {
                _rootIcon = (Texture2D)Resources.Load("RootIcon");
            }

            return EditorIconDrawer._rootIcon;
        }
    }

    private static Texture2D _anchorIcon;
    private static Texture2D AnchorIcon
    {
        get
        {
            if (EditorIconDrawer._anchorIcon == null)
            {
                _anchorIcon = (Texture2D)Resources.Load("AnchorIcon");
            }

            return EditorIconDrawer._anchorIcon;
        }
    }

    static EditorIconDrawer()
    {
        EditorIconDrawer.hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(EditorIconDrawer.DrawHierarchyIcon);
        EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(
            EditorApplication.hierarchyWindowItemOnGUI,
            EditorIconDrawer.hiearchyItemCallback);
    }

    // 绘制icon方法
    private static void DrawHierarchyIcon(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject == null)
        {
            return;
        }

        Rect rect = new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f);

        if (gameObject.GetComponent<BaseTrigger>())
        {
            // 画icon
            GUI.DrawTexture(rect, EditorIconDrawer.TriggerIcon);
        }
        else if (gameObject.GetComponent<BaseTask>())
        {
            // 画icon
            GUI.DrawTexture(rect, EditorIconDrawer.TaskIcon);
        }
        else if (gameObject.GetComponent<BaseEvent>())
        {
            // 画icon
            GUI.DrawTexture(rect, EditorIconDrawer.EventIcon);
        }
        else if (gameObject.GetComponent<DTasksManager>())
        {
            // 画icon
            GUI.DrawTexture(rect, EditorIconDrawer.RootIcon);
        }
        else if (gameObject.GetComponent<AnchorGizmos>())
        {
            // 画icon
            GUI.DrawTexture(rect, EditorIconDrawer.AnchorIcon);
        }
    }
}
