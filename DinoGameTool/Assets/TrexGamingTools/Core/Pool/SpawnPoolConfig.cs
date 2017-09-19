using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dino_Core.Core
{
    [System.Serializable]
    public class SpawnPoolConfig : ScriptableObject
    {
        public List<SpawnPool> Pools;
    }
}


