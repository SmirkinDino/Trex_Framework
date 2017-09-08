using Dino_Core.AssetsUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dino_Core.Task
{
    public class TBWindowToolbar : ITBComponent
    {
        protected Dictionary<string, Action> _menuContentTable;
        protected Dictionary<string, Action>.Enumerator _handlingEnumerator;
        public TBWindowToolbar(TBWindow _window) : base(_window)
        {
            _menuContentTable = new Dictionary<string, Action>();
            _menuContentTable.Add("Save", OnSaveEventhandler);
            _menuContentTable.Add("Generate", GenerateEventhandler);
        }
        public override void PaintComponent()
        {
            GUILayout.BeginHorizontal();
            {
                _handlingEnumerator = _menuContentTable.GetEnumerator();

                while (_handlingEnumerator.MoveNext())
                {
                    if (GUILayout.Button(_handlingEnumerator.Current.Key, EditorStyles.toolbarButton, GUILayout.Width(80)))
                    {
                        if (_handlingEnumerator.Current.Value != null)
                        {
                            _handlingEnumerator.Current.Value();
                        }
                    }
                }
                GUILayout.Box("", EditorStyles.toolbarButton, GUILayout.ExpandWidth(true));
            }
            GUILayout.EndHorizontal();
        }
        private void OnSaveEventhandler()
        {
            try
            {
                AssetsProjectEditor.GenerateConfig(DTaskEditorConst.Level_Path_Editor + SceneManager.GetActiveScene().name == "" ? "UnnamedScene" : SceneManager.GetActiveScene().name, TBWindow.NodesRouter);
            }
            catch (Exception)
            {
                this.DLog("You have already saved");
            }
            //DXMLSerializer.SerializeObjectToXml(
            //        // Path
            //        SceneManager.GetActiveScene().name == "" ?
            //        Application.dataPath + "/Resources/Levels/" + "UnnamedScene.xml" :
            //        Application.dataPath + "/Resources/Levels/" + SceneManager.GetActiveScene().name + ".xml",
            //        // object
            //        TBWindow.NodesRouter,
            //        _drivedClasses
            //    );
        }
        private void GenerateEventhandler()
        {
            try
            {
                if (AssetDatabase.DeleteAsset(DTaskEditorConst.Level_Path_Relative + SceneManager.GetActiveScene().name == "" ? "UnnamedScene" : SceneManager.GetActiveScene().name))
                {
                    this.DLog("Remove old level file!");
                }

                DTaskRouter _router = ExcuteGenerate(TBWindow.NodesRouter);

                if (_router != null)
                {
                    AssetsProjectEditor.GenerateConfig(DTaskEditorConst.Level_Path + SceneManager.GetActiveScene().name == "" ? "UnnamedScene" : SceneManager.GetActiveScene().name, _router);
                }
                else
                {
                    this.DLog("cannot generate router!");
                }

            }
            catch (Exception _error)
            {
                this.DLog("Generate failed");
                this.DLog(_error);
                // Do nothing
            }
        }
        private DTaskRouter ExcuteGenerate(DEditorNodes _nodes)
        {
            return null;
        }
    }
}