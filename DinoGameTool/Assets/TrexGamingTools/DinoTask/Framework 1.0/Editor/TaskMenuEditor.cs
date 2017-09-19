using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TaskMenuEditor {

    private static Transform _selection;
    private static int UniqueID = 0;
    private static void AddToCurrentSelection(Transform _child)
    {
        _selection = Selection.activeTransform;

        if (_selection)
        {
            _child.position = _selection.position;
            _child.SetParent(_selection);
        }
    }
    private static Transform LoadPrefab(string _path, string _name)
    {
        Transform _trans = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<Transform>("Assets/TaskPrefabs/" + _path));
        _trans.name = _name + "_#"+ UniqueID;
        UniqueID++;
        return _trans;
    }
    private static Transform LoadPrefab(string _path, string _name,bool _code)
    {
        Transform _trans = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<Transform>("Assets/TaskPrefabs/" + _path));
        if (_code)
        {
            _trans.name = _name + "_#" + UniqueID;
            UniqueID++;
        }
        else
        {
            _trans.name = _name;
        }
        return _trans;
    }

    #region TASK
    //[MenuItem("GameObject/DTaskNodes/Task/Root", false, -1)]
    //public static void AddTaskRoot()
    //{
    //    AddToCurrentSelection(LoadPrefab("Tasks/Root.prefab", "Root"));
    //}
    //[MenuItem("GameObject/DTaskNodes/Task/NormalTask", false, -1)]
    //public static void AddTaskNode()
    //{
    //    AddToCurrentSelection( LoadPrefab("Tasks/NormalTask.prefab", "NormalTask"));
    //}
    #endregion

    #region EVENTS
    //[MenuItem("GameObject/DTaskNodes/Events/AnimatorEvent", false, -1)]
    //public static void AddAnimatorEvent()
    //{
    //    AddToCurrentSelection(LoadPrefab("Events/AnimatorEvent.prefab", "AnimatorEvent"));
    //}
    //[MenuItem("GameObject/DTaskNodes/Events/BotAnchor", false, -1)]
    //public static void AddBotAnchor()
    //{
    //    AddToCurrentSelection(LoadPrefab("Events/BotAnchor.prefab", "BotAnchor", false));
    //}
    //[MenuItem("GameObject/DTaskNodes/Events/EskyAnchor", false, -1)]
    //public static void AddEskyAnchor()
    //{
    //    AddToCurrentSelection(LoadPrefab("Events/EskyAnchor.prefab", "EskyAnchor", false));
    //}

    //[MenuItem("GameObject/DTaskNodes/Events/Spawner/Esky", false, -1)]
    //public static void AddEskySpawner()
    //{
    //    AddToCurrentSelection(LoadPrefab("Events/Spawner/DEskySpawner.prefab", "EskySpawner"));
    //}

    //[MenuItem("GameObject/DTaskNodes/Events/Spawner/Spider", false, -1)]
    //public static void AddSpiderSpawner()
    //{
    //    AddToCurrentSelection(LoadPrefab("Events/Spawner/DSpiderSpawner.prefab", "SpiderSpawner"));
    //}

    //[MenuItem("GameObject/DTaskNodes/Events/Spawner/TFBot", false, -1)]
    //public static void AddTFBotSpawner()
    //{
    //    AddToCurrentSelection(LoadPrefab("Events/Spawner/DTFBotSpawner.prefab", "TFBotSpawner"));
    //}
    #endregion

    #region TRIGGERS
    //[MenuItem("GameObject/DTaskNodes/Triggers/CollisionTrigger", false, -1)]
    //public static void AddCollisionTrigger()
    //{
    //    AddToCurrentSelection(LoadPrefab("Triggers/CollisionTrigger.prefab", "CollisionTrigger"));
    //}

    //[MenuItem("GameObject/DTaskNodes/Triggers/EnemyNumberTrigger", false, -1)]
    //public static void AddEnemyNumberTrigger()
    //{
    //    AddToCurrentSelection(LoadPrefab("Triggers/EnemyNumberTrigger.prefab", "EnemyNumberTrigger"));
    //}

    //[MenuItem("GameObject/DTaskNodes/Triggers/EventEndTrigger", false, -1)]
    //public static void AddEventEndTrigger()
    //{
    //    AddToCurrentSelection(LoadPrefab("Triggers/EventEndTrigger.prefab", "EventEndTrigger"));
    //}

    //[MenuItem("GameObject/DTaskNodes/Triggers/AutoStarter", false, -1)]
    //public static void AddAutoStarter()
    //{
    //    AddToCurrentSelection(LoadPrefab("Triggers/AutoStarter.prefab", "AutoStarter"));
    //}
    #endregion
}
