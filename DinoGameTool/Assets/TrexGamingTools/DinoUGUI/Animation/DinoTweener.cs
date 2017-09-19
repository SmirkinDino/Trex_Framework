using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

namespace Dino_Core.DinoUGUI
{

    public class DinoTweener : MonoBehaviour, iAnimator
    {
        public float During = 0.2f;

        public void PlayBack()
        {
            PlayBack_Func();
        }

        public void PlayForward()
        {
            PlayForward_Func();
        }

        public virtual void PlayBack_Func()
        {
        }

        public virtual void PlayForward_Func()
        {
        }
    }
}

