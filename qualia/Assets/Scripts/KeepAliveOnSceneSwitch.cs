using UnityEngine;
using System.Collections;

public class KeepAliveOnSceneSwitch : MonoBehaviour
{
    void Awake ()
    {
        DontDestroyOnLoad(transform.gameObject);
        foreach (Transform child in transform)
        {
            DontDestroyOnLoad(child.gameObject);
        }
    }
}
