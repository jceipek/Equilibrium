using UnityEngine;
using System.Collections;

public class AvatarTurnController : MonoBehaviour {

    public float m_turn_speed;

    // Initial direction of controller (passed down into m_camera_controller)
    private Quaternion m_orientation_offset = Quaternion.identity;
    // Rotation amount from inputs (passed down into m_camera_controller)
    private float m_y_rotation = 0.0f;
    private float m_x_rotation = 0.0f;
    private Quaternion m_controller_rotation = Quaternion.identity;

    public QCameraController m_camera_controller;

    void Start () {
        // Get our start direction
        m_orientation_offset = transform.rotation;
        // Make sure to set y rotation to 0 degrees
        m_y_rotation = 0.0f;
        m_x_rotation = 0.0f;
        SetCameras();
    }

    // Update is called once per frame
    void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = -1*Input.GetAxis("Vertical");
        //if (horizontal != 0.0f) {
        //    Debug.Log(horizontal);
        //}
        /*m_y_rotation += horizontal * Time.deltaTime * m_turn_speed;
        m_x_rotation += vertical * Time.deltaTime * m_turn_speed;
        /*m_controller_rotation *= Quaternion.AngleAxis(vertical * Time.deltaTime * m_turn_speed, transform.up) *
                                 Quaternion.AngleAxis(horizontal * Time.deltaTime * m_turn_speed, transform.right);*/

        /*
        if (m_camera_controller != null) {
            Quaternion rotation = Quaternion.identity;
            m_camera_controller.GetCameraOrientation(ref rotation);
            transform.rotation = rotation;
        }

        Vector3 displacement = new Vector3(vertical * Time.deltaTime * m_turn_speed,
                                           horizontal * Time.deltaTime * m_turn_speed);
        /*m_controller_rotation *= Quaternion.FromToRotation(transform.forward, transform.forward + displacement);*/


        /*
        transform.Rotate(displacement);

        if (m_camera_controller != null) {
            Quaternion rotation = Quaternion.identity;
            m_camera_controller.GetCameraOrientation(ref rotation);

            m_orientation_offset = Quaternion.RotateTowards(rotation, transform.rotation, 1.0f);
        }*/

        //m_orientation_offset = transform.rotation;

        SetCameras();
        m_camera_controller.SetYRotationOffset(horizontal * Time.deltaTime * m_turn_speed);
        m_camera_controller.SetXRotationOffset(vertical * Time.deltaTime * m_turn_speed);
    }

    /*void OnDrawGizmos () {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10.0f);
    }*/

    public void SetCameras () {
        if(m_camera_controller != null) {
            // Make sure to set the initial direction of the camera
            // to match the game player direction
            m_camera_controller.SetOrientationOffset(m_orientation_offset);
            //m_camera_controller.SetYRotation(m_y_rotation);
            //m_camera_controller.SetXRotation(m_x_rotation);
            //m_camera_controller.SetControllerRotation(m_controller_rotation);
        }
    }
}
