using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSphericalLevelGenerator : MonoBehaviour
{

    public int m_NodeCount;
    public int m_InfectedCount;
    public int m_ConnectionCount;

    public float m_SphereRadius;
    public bool m_AllowNodesInsideSphere;

    private List<Node> m_NodeList = new List<Node>();

    // Use this for initialization
    void Start ()
    {
        Generate();
    }

    private Node GenerateNodeAt (Vector3 pos)
    {
        GameObject node = Instantiate(Resources.Load("Node")) as GameObject;
        node.transform.position = pos;
        return node.GetComponent<Node>();
    }

    private void Generate ()
    {
        GenerateNodes();
        GenerateConnections();
    }

    private void GenerateNodes ()
    {
        int infected = 0;
        for (int index = 0; index < m_NodeCount; index++)
        {
            Vector3 random_vector;
            if (m_AllowNodesInsideSphere)
            {
                random_vector = Random.insideUnitSphere * m_SphereRadius;
            }
            else
            {
                random_vector = Random.onUnitSphere * m_SphereRadius;
            }

            Node node = GenerateNodeAt(random_vector);
            if (infected < m_InfectedCount)
            {
                Infected infection = node.GetComponent<Infected>();
                infection.enabled = true;
                infected++;
            }
            m_NodeList.Add(node);
        }
    }

    private void GenerateConnections ()
    {
        EnsureConnectionCountIsPossible();
        for (int index = 0; index < m_ConnectionCount; index++)
        {
            bool success = false;
            do
            {
                int first = index % m_NodeCount;
                int second = Random.Range(0, m_NodeList.Count);
                Node firstNode = m_NodeList[first];
                Node secondNode = m_NodeList[second];

                success = (first != second && !firstNode.DoesShareConnectionWith(secondNode));
                if (success)
                {
                    firstNode.AddMutualConnectionTo(secondNode, audible: false);
                }
            } while (!success);
        }
    }

    private void EnsureConnectionCountIsPossible ()
    {
        int maxConnectionCount = m_NodeCount*(m_NodeCount - 1) / 2;
        if (m_ConnectionCount > maxConnectionCount)
        {
            m_ConnectionCount = maxConnectionCount; // So we don't have an infinite loop!
        }
    }
}
