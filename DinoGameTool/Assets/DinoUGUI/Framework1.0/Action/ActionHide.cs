using UnityEngine;
using System.Collections;
using System;

namespace Dino_Core.DinoUGUI
{
    public class ActionHide : iAction
    {
        public DUIEntity Owner
        {
            get; set;
        }

        public void Excute(DUIEntity _owner)
        {
            Owner = _owner;
            Owner.Hide();
        }

        public void OnFinish()
        {
            Owner.gameObject.SetActive(false);
        }
    }
}