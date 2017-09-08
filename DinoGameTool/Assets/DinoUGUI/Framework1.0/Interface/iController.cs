using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Linq.Expressions;

namespace Dino_Core.DinoUGUI
{
    public interface iController
    {
        void OnShow();
        void OnHide();
        void OnUpdate();
        void OnInit();
    }
}
