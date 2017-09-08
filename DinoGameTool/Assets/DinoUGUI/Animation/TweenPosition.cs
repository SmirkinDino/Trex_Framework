using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Dino_Core.DinoUGUI
{
    public class TweenPosition : DinoTweener
    {

        public Vector3 ShowPosition;

        public Vector3 HidePosition;

        public override void PlayBack_Func()
        {
            transform.DOMove(HidePosition, During).SetEase(Ease.OutQuad);
        }

        public override void PlayForward_Func()
        {
            transform.DOMove(ShowPosition, During).SetEase(Ease.OutQuad);
        }
    }
}