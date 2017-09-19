using UnityEngine;
using System.Collections;

namespace Dino_Core.DinoUGUI
{
    public class UIComponet : MonoBehaviour
    {
        public T As<T>() where T : UIComponet
        {
            return this as T;
        }

        public virtual void UpdateUI() { }
        public virtual void Init() { }
    }
}
