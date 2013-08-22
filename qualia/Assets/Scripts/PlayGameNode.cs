using UnityEngine;
using System.Collections;

public class PlayGameNode : MonoBehaviour
{

    public string m_StartSceneName;

    private GameObject m_Avatar;

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

    void Update ()
    {
        if (Utils.AreGameObjectsColliding(gameObject, m_Avatar))
        {
            Debug.Log("START");
            Application.LoadLevel(m_StartSceneName);
        }
    }
}
