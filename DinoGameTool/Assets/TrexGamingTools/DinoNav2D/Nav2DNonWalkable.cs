using UnityEngine;
using System.Collections;

namespace Dino_Core.DinoNav2D
{
    public class Nav2DNonWalkable : Nav2DArea
    {

        private static readonly Color PolyColor = new Color(0.7f, 0.4f, 0.3f, 1.0f);

        void OnDrawGizmos()
        {
            Gizmos.color = PolyColor;
            UpdateVertexBuffer();
            DrawVertexBuffer();
        }
    }
}