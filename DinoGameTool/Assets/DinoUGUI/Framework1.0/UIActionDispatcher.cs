using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dino_Core.DinoUGUI
{
    public class UIActionDispatcher
    {
        public static Dictionary<int, iAction> m_ActionSet;

        public static void InitActionDispatcher()
        {
            if (m_ActionSet != null)
            {
                m_ActionSet.Clear();
            }
            else
            {
                m_ActionSet = new Dictionary<int, iAction>();
            }

            // 注册事件
            RegistAction(Action_Request_Type.ACTION_SHOW,new ActionShow());
            RegistAction(Action_Request_Type.ACTION_HIDE, new ActionHide());
        }

        public static void RegistAction(Action_Request_Type _type,iAction _action)
        {
            m_ActionSet.AddEntity((int)_type,_action);
        }

        public static void UnregistAction(Action_Request_Type _type)
        {
            m_ActionSet.RemoveEntity((int)_type);
        }

        public static UIActionCallback DispatchAction(DUIEntity _owner, Action_Request_Type _type)
        {
            iAction _action = m_ActionSet.GetEntity((int)_type);
            _action.Excute(_owner);
            return _action.OnFinish;
        }
    }
}