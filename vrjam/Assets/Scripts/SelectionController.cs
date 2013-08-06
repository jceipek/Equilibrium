using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour {

    // TODO (Julian): Generalize these (Rift doesn't even work)
    public Camera m_standard_camera;

    public Connection m_connection = null;

    public bool IsANodeSelected () {
        return (m_connection != null);
    }

    public void SelectNode (Node node) {
        GameObject connection = Instantiate(Resources.Load("Connection")) as GameObject;
        Connection connection_component = connection.GetComponent<Connection>();
        connection_component.InitializeWithStartNode(node);
        m_connection = connection_component;
    }

    public bool TryToConnectTo (Node node) {
        bool success = m_connection.TryToFinishConnectionWithEndNode(node);
        if (!success) m_connection.DestroyConnection(); // TODO (Julian): Maybe move this into TryToFinishConnectionWithEndNode?
        m_connection = null;
        return success;
    }

    public bool TryToDragConnectionTo (Vector3 position) {
        return m_connection.TryToReachPoint(position);
    }

    public void AbortConnection () {
        m_connection.DestroyConnection();
        m_connection = null;
    }
}
