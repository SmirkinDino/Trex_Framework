using UnityEngine;

namespace Dino_Core.Task
{
    public class CollisionTrigger : BaseTrigger
    {
        public string TiggerTag;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TiggerTag))
            {
                Conditional();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(.2f,.8f,.2f,.5f);
            Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation, transform.localScale);
        }
    }
}