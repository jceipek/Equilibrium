using UnityEngine;
using System.Collections;

public class ConnectionMakingAbility : MonoBehaviour {

    public Camera m_camera;

    private static LayerMask m_SELECTABLE_LAYER;
    private AvatarController m_avatar;

    void OnEnable () {
        m_SELECTABLE_LAYER = (1 << LayerMask.NameToLayer("Selectable"));
        m_avatar = GetComponent<AvatarController>();
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        RaycastHit hit;
        Node node = null;
        if (Physics.Raycast(m_camera.transform.position, m_camera.transform.forward, out hit, 100, m_SELECTABLE_LAYER)) {
            node = hit.collider.GetComponent<Node>();
        }
        if (node) {
            node.Highlight();
            if (!m_avatar.m_next_node.DoesShareConnectionWith(node)) {
                GameObject connection_object = (GameObject)Instantiate(Resources.Load("Connection"));
                Connection connection = connection_object.GetComponent<Connection>();
                connection.InitializeWithStartNode(m_avatar.m_next_node);
                connection.FinishConnectionWithEndNode(node);
            }
        }
    }

    void OnDrawGizmos () {
        Gizmos.DrawRay(m_camera.transform.position, m_camera.transform.forward);
    }
}