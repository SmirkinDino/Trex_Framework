using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dino_Core.Core
{
    [System.Serializable]
    public class ConfigurationItem : ICloneable
    {
        public string ItemKey = "";

        public GameObject Resources;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public T As<T>() where T : ConfigurationItem
        {
            return this as T;
        }
    }
}