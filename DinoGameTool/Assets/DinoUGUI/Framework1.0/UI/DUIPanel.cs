using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Dino_Core.DinoUGUI
{
    public class DUIPanel : DUIEntity
    {
        protected string m_panelNmae = "";

        public override void OnInit()
        {
            this.InitChildren<DUINode>();

            // 如果是spawn出来的取消掉后面的(clone)字样
            this.m_panelNmae = name.Split('(')[0];
            name = this.m_panelNmae;
        }

        public string getName()
        {
            return this.m_panelNmae;
        }
    }
}