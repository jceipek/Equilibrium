using UnityEngine;
using System.Collections;

public class FirstNodeTarget : MonoBehaviour {

    private AvatarController m_Avatar;

/*
    void OnEnable ()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");



        if (playerObjects.Length != 1)
        {
            Debug.LogError("There should be exactly one Player-tagged object in the scene, but there are " + playerObjects.Length + ".");
        } else
        {
            m_Avatar = playerObjects[0].GetComponent<AvatarController>();

            if (!m_Avatar.m_NextNode)
            {
                m_Avatar.m_NextNode = gameObject.GetComponent<Node>();
            }
        }
    }
    */

    void Start ()
    {
        CameraFade.StartAlphaFade(Color.white,
                                  isFadeIn: true,
                                  fadeDuration: 2f,
                                  fadeDelay: 0f);

        m_Avatar = AvatarController.g;
        if (!m_Avatar.m_NextNode)
        {
            m_Avatar.m_NextNode = gameObject.GetComponent<Node>();
        }
    }

    void OnDrawGizmos ()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(gameObject.transform.position, Vector3.one);
    }
}
