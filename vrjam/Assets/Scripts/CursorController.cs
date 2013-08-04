using UnityEngine;
using System.Collections;

public class CursorController : MonoBehaviour {

    public float m_speed;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        float t = Time.deltaTime;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 position = transform.position;
        Vector3 delta_position = new Vector3(x * m_speed * t,
                                             y * m_speed * t,
                                             0.0f);
        transform.position = position + delta_position;
    }
}