using UnityEngine;
using System.Collections;

public class ConnectionCreationAbility : MonoBehaviour
{
    private int novelNodesConnected = 0;

    public Transform m_LookTarget;
    private static LayerMask SELECTABLE_LAYER;
    private AvatarController m_Avatar;
    private SoundComplexityManager m_SoundComplexityManager;

    public bool canCreateConnection = false;

    public void CloseEnoughToMakeConnection (Node node)
    {
        canCreateConnection = true;
    }

    public void NotCloseEnoughToMakeConnection (Node node)
    {
        canCreateConnection = false;
    }

    void OnEnable ()
    {
        SELECTABLE_LAYER = (1 << LayerMask.NameToLayer("Selectable"));
        m_Avatar = GetComponent<AvatarController>();
        m_SoundComplexityManager = GetComponent<SoundComplexityManager>();
    }

    void Update ()
    {
        if (canCreateConnection) {
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
                    if (node.GetConnectionCount() == 0)
                    {
                        novelNodesConnected++;
                    }
                    bool createdConnection = m_Avatar.m_NextNode.AddMutualConnectionTo(node);
                    if (createdConnection)
                    {
                        canCreateConnection = false;
                    }
                    if (createdConnection && (!node.GetComponent<Infected>().enabled && !m_Avatar.m_NextNode.GetComponent<Infected>().enabled))
                    {
                        m_SoundComplexityManager.IncreaseComplexity();
                    }
                }
                else
                {
                    //Debug.LogError("m_NextNode undefined!");
                }
            }
        }
    }

    void OnDrawGizmos ()
    {
        Gizmos.DrawRay(gameObject.transform.position, m_LookTarget.forward);
    }
}