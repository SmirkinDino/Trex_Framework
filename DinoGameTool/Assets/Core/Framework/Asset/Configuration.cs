using System.Collections.Generic;
using UnityEngine;

namespace Dino_Core.AssetsUtils
{
    /// <summary>
    /// 配置列表基类
    /// </summary>
    [System.Serializable]
    public class Configuration<T> : ScriptableObject where T : ConfigurationItem
    {
        [SerializeField]
        public string CongigurationKey;

        [SerializeField]
        public List<T> ConfigurationList = new List<T>();

        public T GetConfigurationItem(string _itemKey)
        {
            for (int i = 0; i < ConfigurationList.Count; i++)
            {
                if (ConfigurationList[i].ItemKey.Equals(_itemKey))
                {
                    return ConfigurationList[i];
                }
            }
            return null;
        }
    }
}


