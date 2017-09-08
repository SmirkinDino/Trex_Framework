using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dino_Core.AssetsUtils
{
    [System.Serializable]
    public class SpawnPrefab
    {
        public int SpawnedCount { get; set; }

        [SerializeField]
        public Transform Resouces;

        [SerializeField]
        public int Preload;

        [SerializeField]
        public int Limit;

        [SerializeField]
        public string LogMessage;
    }
}