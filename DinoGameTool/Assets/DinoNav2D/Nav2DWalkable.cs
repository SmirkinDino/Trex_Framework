using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dino_Core.DinoNav2D
{

    public class Nav2DWalkable : Nav2DArea
    {
        private static readonly Color PolyColor = new Color(0.3f,0.4f,0.7f,1.0f);

        void OnDrawGizmos()
        {
            Gizmos.color = PolyColor;
            UpdateVertexBuffer();
            DrawVertexBuffer();
        }
    }
}