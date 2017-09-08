using UnityEngine;
using System.Collections;



namespace Dino_Core.DinoUGUI
{
    public interface iAnimator
    {
        void PlayForward();
        void PlayBack();

        void PlayBack_Func();
        void PlayForward_Func();
    }
}


