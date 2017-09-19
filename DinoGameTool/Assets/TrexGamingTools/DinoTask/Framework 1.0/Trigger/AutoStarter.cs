using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dino_Core.Task
{
    public class AutoStarter : BaseTrigger
    {
        public float Delay;

        protected float _time;

        protected override void TReady()
        {
            _time = Time.time + Delay;
        }

        protected override void OnMonitoring()
        {
            if (_time < Time.time)
            {
                Conditional();
            }
        }
    }
}