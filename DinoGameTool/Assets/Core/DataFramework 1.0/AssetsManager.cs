using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace Dino_Core.Core
{
    /// <summary>
    /// SmirkinDino 2017 06 01
    /// </summary>
    public class AssetsManager : MonoSingleton<AssetsManager>
    {
        protected static Dictionary<string, Configuration<ConfigurationItem>> m_Router = new Dictionary<string, Configuration<ConfigurationItem>>();        // the router

        protected static readonly string ROOT_PATH = "ConfigurationAeeet";                                                                                  // under Resources folder . And

        private Configuration<ConfigurationItem>[] m_HandledObjects;                                                                                        // Save gc
        private IDictionaryEnumerator m_DicEnumerator;                                                                                                      // Save gc

        public void Init()
        {
            m_Router.Clear();

            LoadAssetAndStore();
        }

        public T GetAsset<T>(string _cfgKey,string _itemKey) where T : ConfigurationItem
        {
            try
            {
                if (m_Router.ContainsKey(_cfgKey))
                {
                    // Shallow clone a new one and return
                    return m_Router[_cfgKey].GetConfigurationItem(_itemKey).Clone() as T;
                }
            }
            catch (Exception)
            {
#if DINO_DEBUG
                this.DLog("Get Assets error.");
#endif
            }

#if DINO_DEBUG
                this.DLog(string.Format("Not Find Item {0}-{1}", _cfgKey, _itemKey));
#endif
            return null;
        }

        public List<string> GetKeys(string _cfgKey)
        {
            List<string> _rlt = new List<string>();

            if (m_Router.ContainsKey(_cfgKey))
            {
                for (int i = 0; i < m_Router[_cfgKey].ConfigurationList.Count; i++)
                {
                    _rlt.Add(m_Router[_cfgKey].ConfigurationList[i].ItemKey);
                }
            }

            return _rlt;
        }

        public List<string> GetKeys()
        {
            List<string> _rlt = new List<string>();

            m_DicEnumerator = m_Router.GetEnumerator();

            while (m_DicEnumerator.MoveNext())
            {
                _rlt.Add(m_DicEnumerator.Key.ToString());
            }

            return _rlt;
        }

        protected void LoadAssetAndStore()
        {
            string[] _splitName = null;

            try
            {
                m_HandledObjects = Resources.LoadAll<Configuration<ConfigurationItem>>("ConfigurationAeeet");
            }
            catch (Exception)
            {
#if DINO_DEBUG
                this.DLog(string.Format("Wrong file"));
#endif
            }

            for (int i = 0;i < m_HandledObjects.Length; i++)
            {
                // 去掉名字前面的域名等
                _splitName = m_HandledObjects[i].name.Split('.');
                m_Router.AddEntity(_splitName[_splitName.Length - 1], m_HandledObjects[i]);
            }
        } 
    }
}