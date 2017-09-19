using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorGizmos : MonoBehaviour {
    
    public enum AnchorGizmos_Type
    {
        EskyAnchor,
        BotAnchor
    }

    public AnchorGizmos_Type GizmosType;

    private void OnDrawGizmos()
    {
        switch (GizmosType)
        {
            case AnchorGizmos_Type.EskyAnchor:
                Gizmos.color = new Color(0.2f, 0.2f, 0.9f, 0.7f);
                Gizmos.DrawSphere(transform.position, 1);
                break;
            case AnchorGizmos_Type.BotAnchor:
                Gizmos.color = new Color(0.8f, 0.2f, 0.2f, 0.7f);
                Gizmos.DrawSphere(transform.position, 1);
                break;
            default:
                break;
        }
    }
}
