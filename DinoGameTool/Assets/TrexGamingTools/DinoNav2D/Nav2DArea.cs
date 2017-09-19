using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dino_Core.DinoNav2D
{

    [RequireComponent(typeof(PolygonCollider2D))]
    public class Nav2DArea : MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> mVertexBuffer = new List<Vector3>();

        private PolygonCollider2D mPolyCollider;

        public void UpdateVertexBuffer()
        {
            mVertexBuffer.Clear();

            if (mPolyCollider == null)
            {
                mPolyCollider = GetComponent<PolygonCollider2D>();
            }

            Vector2[] pointsBuffer = mPolyCollider.points;
            foreach (Vector2 point in pointsBuffer)
            {
                // translate the mat to vec3
                Vector3 v = new Vector3(point.x, point.y);

                // translate
                v = mPolyCollider.transform.TransformPoint(v);

                mVertexBuffer.Add(v);
            }
        }

        public List<Vector3> GetVertexBuffer()
        {
            return this.mVertexBuffer;
        }

        protected void DrawVertexBuffer()
        {
            for (int i = 0; i < mVertexBuffer.Count; i++)
            {
                if (i != mVertexBuffer.Count - 1)
                    Gizmos.DrawLine(mVertexBuffer[i], mVertexBuffer[i + 1]);
                else
                    Gizmos.DrawLine(mVertexBuffer[i], mVertexBuffer[0]);
            }
        }

    }
}