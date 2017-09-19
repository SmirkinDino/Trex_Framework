using System.Collections.Generic;
using UnityEngine;
namespace Dino_Core.Task
{
    [System.Serializable]
    public class DEditorNodes : ScriptableObject
    {
        public int AutoID = 0;
        public List<DBaseNodeEditor> Router = new List<DBaseNodeEditor>();

        public DBaseNodeEditor this[int i]
        {
            get
            {
                if (i >= 0 && i < Router.Count)
                {
                    return Router[i];
                }

                return null;
            }
            set
            {
                if (i >= 0 && i < Router.Count)
                {
                    Router[i] = value;
                }
            }
        }
        public DBaseNodeEditor GetNodeByID(int _id)
        {
            for (int i = 0; i < Router.Count; i++)
            {
                if (Router[i].NodeID == _id)
                {
                    return Router[i];
                }
            }
            return null;
        }
        public int Count
        {
            get
            {
                return Router.Count;
            }
        }
        public void Add(DBaseNodeEditor _node)
        {
            AutoID++;
            Router.Add(_node);
        }
        public void Remove(DBaseNodeEditor _node)
        {
            if (Router.Contains(_node))
            {
                Router.Remove(_node);
            }
        }
        public void Clear()
        {
            Router.Clear();
        }
        private void OnEnable()
        {
            if (Router == null)
            {
                Router = new List<DBaseNodeEditor>();
            }
        }
    }
}