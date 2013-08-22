using UnityEngine;
using System.Collections;

public class DieOnHitNode : MonoBehaviour
{
    void ReachedNode (Node node)
    {
        Infected infection = node.GetComponent<Infected>();
        if (infection && infection.enabled)
        {
            Debug.Log("DIE!");
        }
    }
}
