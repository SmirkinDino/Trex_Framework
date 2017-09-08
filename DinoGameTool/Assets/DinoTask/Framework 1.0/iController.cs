using UnityEngine;

namespace Dino_Core.Task
{
    public interface iController
    {
        void InitController();
        void ResetController();
    }

    [System.Serializable]
    public class InterfaceHelper
    {
        public Component target;

        public T GetInterface<T>() where T : class
        {
            return target as T;
        }
    }
}