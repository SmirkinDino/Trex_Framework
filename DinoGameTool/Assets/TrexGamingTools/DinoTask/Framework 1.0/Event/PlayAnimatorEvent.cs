using UnityEngine;

namespace Dino_Core.Task
{
    public class PlayAnimatorEvent : BaseEvent
    {
        public Transform TargetTrans;
        public bool SwitchActive;
        public string AnimatorKey;
        public bool ToState;

        protected Animator _animator;

        protected override void TInit()
        {
            base.InitController();
            _animator = TargetTrans.GetComponent<Animator>();
        }
        protected override void TReset()
        {
            if (_animator && AnimatorKey != "")
            {
                _animator.SetBool(AnimatorKey, !ToState);
            }

            if (SwitchActive)
            {
                TargetTrans.gameObject.SetActive(false);
            }
            
        }
        protected override void TStart()
        {
            if (SwitchActive)
            {
                TargetTrans.gameObject.SetActive(true);
            }

            if (_animator && AnimatorKey != "")
            {
                _animator.SetBool(AnimatorKey, ToState);
            }
        }
    }
}