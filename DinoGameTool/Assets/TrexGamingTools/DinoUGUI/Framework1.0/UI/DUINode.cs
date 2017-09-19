using UnityEngine;
using System.Collections;

namespace Dino_Core.DinoUGUI
{

    public class DUINode : DUIEntity
    {
        public override void OnInit()
        {
            this.InitChildren<DUIItem>();
        }
    }
}