using UnityEngine;
using System.Collections;

public class ConnectionCreationAbility : MonoBehaviour
{
    public Transform m_LookTarget;
    private static LayerMask SELECTABLE_LAYER;
    private AvatarController m_Avatar;

    void OnEnable ()
    {
        SELECTABLE_LAYER = (1 << LayerMask.NameToLayer("Selectable"));
        m_Avatar = GetComponent<AvatarController>();
    }

    void Update ()
    {
        RaycastHit hit;
        Node node = null;
        if (Physics.Raycast(m_LookTarget.position, m_LookTarget.forward, out hit, 100, SELECTABLE_LAYER))
        {
            node = hit.collider.GetComponent<Node>();
        }
        if (node)
        {
            //node.Highlight();
            if (m_Avatar.m_NextNode)
            {
                m_Avatar.m_NextNode.AddMutualConnectionTo(node);
            }
            else
            {
                Debug.LogError("m_NextNode undefined!");
            }
        }
    }

    void OnDrawGizmos ()
    {
        Gizmos.DrawRay(gameObject.transform.position, m_LookTarget.forward);
    }
}