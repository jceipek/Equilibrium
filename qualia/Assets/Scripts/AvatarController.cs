using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum INPUT_MODE
{KeyToMove, WaitToMove};

public class AvatarController : MonoBehaviour
{

    public static AvatarController g = null;

    public INPUT_MODE m_InputMode = INPUT_MODE.WaitToMove;
    public Transform m_LookTarget;
    public float m_Speed;

    public Node m_NextNode;
    private bool m_IsMoving = false;

    private Node m_CurrentNode;
    private CameraSwitcher m_CameraSwitcher;

    public bool m_LevelTransitioning = false;

    void OnEnable ()
    {
        m_CameraSwitcher = GetComponent<CameraSwitcher>();
        // Ensure uniqueness
        if (AvatarController.g)
        {
            Destroy(gameObject);
        }
        else
        {
            AvatarController.g = this;
        }
    }

    void Start ()
    {
        m_IsMoving = true;
    }

    void Update ()
    {
        if (m_NextNode)
        {
            if (m_IsMoving)
            {
                Vector3 positionDifference = m_NextNode.transform.position - gameObject.transform.position;
                if (Utils.IsDistanceInCollideRange(positionDifference.magnitude))
                {
                    gameObject.SendMessage("ReachedNode", m_NextNode, SendMessageOptions.DontRequireReceiver);
                    m_IsMoving = false;
                } else {
                    Vector3 direction = positionDifference.normalized;
                    gameObject.transform.position += (direction * m_Speed * Time.deltaTime);
                }
            }
            else
            {
                m_CurrentNode = m_NextNode;
                if (m_InputMode == INPUT_MODE.KeyToMove)
                {
                    if (Input.GetButtonDown("Select"))
                    {
                        PickNodeInSight();
                        m_IsMoving = true;
                    }
                }
                else if (m_InputMode == INPUT_MODE.WaitToMove)
                {
                    StartCoroutine(MoveInSeconds(0.3f));
                }
            }
        }
    }

    void OnDrawGizmos ()
    {
        //if (!Application.isPlaying) {
        if (m_NextNode)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(gameObject.transform.position, m_NextNode.transform.position);
        }
        //}
    }

    public void TransitionToLevel (string levelName)
    {
        m_LevelTransitioning = true;
        m_NextNode = null;
        m_CurrentNode = null;
        CameraFade.StartAlphaFade(Color.white,
                              isFadeIn: false,
                              fadeDuration: 2f,
                              fadeDelay: 0f,
                              OnFadeFinish: () => {
                                Application.LoadLevel(levelName);
                                m_LevelTransitioning = false;
                              });
    }

    private IEnumerator MoveInSeconds (float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!m_LevelTransitioning)
        {
            PickNodeInSight();
        }
        m_IsMoving = true;
    }

    private void PickNodeInSight ()
    {
        float largestDot = -0.0f;
        foreach (Node node in m_CurrentNode.GetConnectedNodes())
        {
            Vector3 nodeDirection = (node.transform.position - m_CurrentNode.transform.position).normalized;
            float currentDot = Vector3.Dot(nodeDirection, m_LookTarget.transform.forward);
            if (currentDot > largestDot)
            {
                largestDot = currentDot;
                m_NextNode = node;
            }
        }
    }
}
