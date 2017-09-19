using UnityEngine;
using System.Collections;

namespace Dino_Core.DinoNav2D
{
    public static class Nav2DConst
    {
        public static readonly LayerMask Walkable_Layer = LayerMask.NameToLayer("Nav2DWalk");
        public static readonly LayerMask Non_Walkable_Layer = LayerMask.NameToLayer("Nav2DNonwalk");
        public static readonly int Walkable_LayerID = 14;
        public static readonly int Non_Walkable_LayerID = 13;
    }
}