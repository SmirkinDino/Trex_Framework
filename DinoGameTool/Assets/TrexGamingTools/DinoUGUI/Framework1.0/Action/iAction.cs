using UnityEngine;
using System.Collections;

namespace Dino_Core.DinoUGUI
{
    public enum Action_Request_Type
    {
        ACTION_SHOW = 1,
        ACTION_HIDE = 0,
    }

    public interface iAction 
    {
        DUIEntity Owner
        {
            get; set;
        }
        void Excute(DUIEntity _owner);
        void OnFinish();
    }

    public delegate void UIActionCallback();
}

