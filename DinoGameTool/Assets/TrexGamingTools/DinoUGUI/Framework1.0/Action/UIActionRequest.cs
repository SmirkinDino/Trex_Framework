using UnityEngine;
using System.Collections;


namespace Dino_Core.DinoUGUI
{

    public class UIActionRequest
    {
        [SerializeField]
        public DUIEntity ActionOwner
        {
            get; set;
        }

        [SerializeField]
        public Action_Request_Type ActionType
        {
            get; set;
        }

        [SerializeField]
        public float ActionDuring
        {
            get; set;
        }

        public UIActionRequest(DUIEntity _owner, Action_Request_Type _type)
        {
            ActionOwner = _owner;
            ActionType = _type;
            ActionDuring = _owner.AnimationDuring;
        }

        public UIActionRequest(DUIEntity _owner, Action_Request_Type _type,float _during)
        {
            ActionOwner = _owner;
            ActionType = _type;
            ActionDuring = _during;
        }

        public UIActionRequest()
        {

        }
    }
}

