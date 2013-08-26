using UnityEngine;
using System.Collections;

public class PlayGameNode : MonoBehaviour
{

    public string m_StartSceneName;

    private AvatarController m_Avatar;

    private bool m_Started = false;

    /*
    void OnEnable ()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        if (playerObjects.Length != 1)
        {
            Debug.LogError("There should be exactly one Player-tagged object in the scene, but there are " + playerObjects.Length + ".");
        }
        else
        {
            m_Avatar = playerObjects[0];
        }
    }
    */

    void Start ()
    {
        m_Avatar = AvatarController.g;
    }

    void Update ()
    {
        if (Utils.AreGameObjectsColliding(gameObject, m_Avatar.gameObject) && !m_Started)
        {
            //Debug.Log("START");
            m_Started = true;
            m_Avatar.TransitionToLevel(m_StartSceneName);
        }
    }
}

