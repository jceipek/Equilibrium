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
            m_Avatar.m_NextNode.AddMutualConnectionTo(node);
        }
    }

    private void CreateConnection (Node startNode, Node endNode)
    {
        GameObject connection_object = (GameObject)Instantiate(Resources.Load("Connection"));
        Connection connection = connection_object.GetComponent<Connection>();
        connection.InitializeConnectionBetween(startNode, endNode);
        //AudioSource.PlayClipAtPoint(m_new_connection_sound, end_node.transform.position);

        //end_node.PlayRandomSound();
    }


    void OnDrawGizmos ()
    {
        Gizmos.DrawRay(gameObject.transform.position, m_LookTarget.forward);
    }
}