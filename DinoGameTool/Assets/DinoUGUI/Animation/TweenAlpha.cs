using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

namespace Dino_Core.DinoUGUI
{
    public class TweenAlpha : DinoTweener
    {

        public float From;

        public float To;

        private Graphic[] m_Images;

        void Awake()
        {
            m_Images = GetComponentsInChildren<Graphic>();
        }

        public override void PlayBack_Func()
        {
            if(m_Images != null)
            {
                foreach (Graphic _img in m_Images)
                {
                    _img.DOFade(From, During);
                }
            }
        }

        public override void PlayForward_Func()
        {
            if (m_Images != null)
            {
                foreach (Graphic _img in m_Images)
                {
                    _img.DOFade(To, During);
                }
            }
        }
    }
}


