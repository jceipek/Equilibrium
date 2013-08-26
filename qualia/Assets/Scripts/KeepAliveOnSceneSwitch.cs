using UnityEngine;
using System.Collections;

public class KeepAliveOnSceneSwitch : MonoBehaviour
{
    void Awake ()
    {
        DontDestroyOnLoad(transform.gameObject);
        foreach (Transform child in transform)
        {
        	// Hack to make scene transitions work
        	if (child.gameObject.tag != "RiftCameraController")
        	{
        		DontDestroyOnLoad(child.gameObject);
        	}
        	else
        	{
        		Debug.Log("Kill ME");
        	}
        }
    }
}