using UnityEngine;
using System.Collections;
using System;

namespace Dino_Core.DinoUGUI
{
    public class ActionShow : iAction
    {
        public DUIEntity Owner
        {
            get; set;
        }

        public void Excute(DUIEntity _owner)
        {
            Owner = _owner;
            Owner.gameObject.SetActive(true);
            Owner.Show();
        }

        public void OnFinish()
        {
        }
    }
}


