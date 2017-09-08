using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Dino_Core.AssetsUtils
{
    [ExecuteInEditMode]
    public class AssetsProjectEditor
    {
        [MenuItem("Assets/Dino_Core/Generate")]
        public static void GenerateConfig()
        {
            MonoScript _configClass = Selection.activeObject as MonoScript;

            ScriptableObject _configEntity = Activator.CreateInstance(_configClass.GetClass()) as ScriptableObject;

            if (!_configEntity)
            {
                ExtendLib.DLog("Configuration Generator", "Could not find class!");
                return;
            }

            if (!Directory.Exists(Application.dataPath + "/Resources/ConfigurationAsset"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Resources/ConfigurationAsset");
            }

            AssetDatabase.CreateAsset(_configEntity, string.Format("Assets/Resources/ConfigurationAsset/{0}.asset", (_configEntity.GetType().ToString())));
        }

        public static void GenerateConfig(string _name, ScriptableObject _configEntity)
        {
            if (!_configEntity)
            {
                ExtendLib.DLog("Configuration Generator", "Could not find class!");
                return;
            }

            if (!Directory.Exists(Application.dataPath + "/Resources/ConfigurationAsset"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Resources/ConfigurationAsset");
            }

            try
            {
                AssetDatabase.CreateAsset(_configEntity, string.Format("Assets/Resources/ConfigurationAsset/{0}.asset", _name));
            }
            catch (Exception)
            {

            }

        }
    }
}