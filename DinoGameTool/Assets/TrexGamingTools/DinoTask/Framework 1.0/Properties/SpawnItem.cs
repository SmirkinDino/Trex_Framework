using UnityEngine;

namespace Dino_Core.Task
{
    [System.Serializable]
    public class SpawnItem
    {
        public int Number;
        public float Interval;
        public string Args;
        public TaskConst.Enemy_Type Type;
        public Transform MovePosition;
    }
}