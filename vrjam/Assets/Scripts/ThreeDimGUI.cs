using UnityEngine;
using System.Collections;

public class ThreeDimGUI : MonoBehaviour {

    // Handle to OVRCameraController
    private OVRCameraController m_camera_controller = null;

    // Rift detection
    private bool   HMDPresent           = false;
    private bool   SensorPresent        = false;
    private float  m_rift_present_timeout   = 0.0f;
    private string strRiftPresent       = "";

    // Device attach / detach
    public enum enDEVICE {HMDSensor, HMD, LatencyTester}
    private float  m_device_detection_timeout   = 0.0f;
    private string strDeviceDetection       = "";

    // Mag yaw-drift correction
    private OVRMagCalibration   m_mag_cal     = new OVRMagCalibration();

    // Replace the GUI with our own texture and 3D plane that
    // is attached to the rendder camera for true 3D placement
    private OVRGUI          m_gui_helper         = new OVRGUI();
    private GameObject      m_gui_render_object  = null;
    private RenderTexture   m_gui_render_texture = null;

    // Crosshair system, rendered onto 3D plane
    public Texture  m_crosshair_image          = null;
    private SelectionCursor m_crosshair          = new SelectionCursor();

    // Create a delegate for update functions
    private delegate void updateFunctions();
    private updateFunctions UpdateFunctions;

    void Awake() {
        // Find camera controller
        OVRCameraController[] camera_controllers;
        camera_controllers = gameObject.GetComponentsInChildren<OVRCameraController>();

        if (camera_controllers.Length == 0) {
            Debug.LogWarning("ThreeDimGUI: No OVRCameraController attached.");
        } else if (camera_controllers.Length > 1)
            Debug.LogWarning("ThreeDimGUI: More then 1 OVRCameraController attached.");
        else {
            m_camera_controller = camera_controllers[0];
        }
    }

    // Use this for initialization
    void Start () {
        // Ensure that camera controller variables have been properly
        // initialized before we start reading them
        if (m_camera_controller != null) {
            m_camera_controller.InitCameraControllerVariables();
            m_gui_helper.SetCameraController(ref m_camera_controller);
        }

        // Set the GUI target
        m_gui_render_object = GameObject.Instantiate(Resources.Load("OVRGUIObjectMain")) as GameObject;

        if (m_gui_render_object != null) {
            if (m_gui_render_texture == null) {
                int w = Screen.width;
                int h = Screen.height;

                if (m_camera_controller.PortraitMode == true) {
                    int t = h;
                    h = w;
                    w = t;
                }

                m_gui_render_texture = new RenderTexture(w, h, 24);
                m_gui_helper.SetPixelResolution(w, h);
                m_gui_helper.SetDisplayResolution(OVRDevice.HResolution, OVRDevice.VResolution);
            }
        }

        // Attach GUI texture to GUI object and GUI object to Camera
        if (m_gui_render_texture != null && m_gui_render_object != null) {
            m_gui_render_object.renderer.material.mainTexture = m_gui_render_texture;

            if (m_camera_controller != null) {
                // Grab transform of GUI object
                Transform t = m_gui_render_object.transform;
                // Attach the GUI object to the camera
                m_camera_controller.AttachGameObjectToCamera(ref m_gui_render_object);
                // Reset the transform values (we will be maintaining state of the GUI object
                // in local state)
                OVRUtils.SetLocalTransform(ref m_gui_render_object, ref t);
                // Deactivate object until we have completed the fade-in
                // Also, we may want to deactive the render object if there is nothing being rendered
                // into the UI
                // we will move the position of everything over to the left, so get
                // IPD / 2 and position camera towards negative X
                Vector3 lp = m_gui_render_object.transform.localPosition;
                float ipd = 0.0f;
                m_camera_controller.GetIPD(ref ipd);
                lp.x -= ipd * 0.5f;
                m_gui_render_object.transform.localPosition = lp;

                m_gui_render_object.SetActive(false);
            }
        }

        // Make sure to hide cursor
        if (Application.isEditor == false) {
            Screen.showCursor = false;
            Screen.lockCursor = true;
        }

        // Device updates
        UpdateFunctions += UpdateDeviceDetection;

        // Mag Yaw-Drift correction
        UpdateFunctions += m_mag_cal.UpdateMagYawDriftCorrection;
        m_mag_cal.SetOVRCameraController(ref m_camera_controller);

        // Crosshair functionality
        m_crosshair.Init();
        m_crosshair.SetCrosshairTexture(ref m_crosshair_image);
        m_crosshair.SetOVRCameraController (ref m_camera_controller);
        UpdateFunctions += m_crosshair.UpdateCrosshair;

        // Check for HMD and sensor
        CheckIfRiftPresent();
    }

    // RIFT DETECTION

    // CheckIfRiftPresent
    // Checks to see if HMD and / or sensor is available, and displays a
    // message if it is not
    void CheckIfRiftPresent() {
        HMDPresent = OVRDevice.IsHMDPresent();
        SensorPresent = OVRDevice.IsSensorPresent(0); // 0 is the main head sensor

        if ((HMDPresent == false) || (SensorPresent == false)) {
            m_rift_present_timeout = 5.0f; // Keep message up for 10 seconds

            if ((HMDPresent == false) && (SensorPresent == false))
                strRiftPresent = "NO HMD AND SENSOR DETECTED";
            else if (HMDPresent == false)
                strRiftPresent = "NO HMD DETECTED";
            else if (SensorPresent == false)
                strRiftPresent = "NO SENSOR DETECTED";
        }
    }

    // Update is called once per frame
    void Update () {
        UpdateFunctions();
    }

    void UpdateDeviceDetection () {
        if (m_rift_present_timeout > 0.0f)
            m_rift_present_timeout -= Time.deltaTime;

        if (m_device_detection_timeout > 0.0f)
            m_device_detection_timeout -= Time.deltaTime;
    }

    // UpdateDeviceDetectionMsgCallback
    void UpdateDeviceDetectionMsgCallback(enDEVICE device, bool attached) {
        if (attached == true) {
            switch (device) {
                case (enDEVICE.HMDSensor):
                    strDeviceDetection = "HMD SENSOR ATTACHED";
                    break;

                case (enDEVICE.HMD):
                    strDeviceDetection = "HMD ATTACHED";
                    break;

                case (enDEVICE.LatencyTester):
                    strDeviceDetection = "LATENCY SENSOR ATTACHED";
                    break;
            }
        }
        else {
            switch (device) {
                case (enDEVICE.HMDSensor):
                    strDeviceDetection = "HMD SENSOR DETACHED";
                    break;

                case (enDEVICE.HMD):
                    strDeviceDetection = "HMD DETACHED";
                    break;

                case (enDEVICE.LatencyTester):
                    strDeviceDetection = "LATENCY SENSOR DETACHED";
                    break;
            }
        }
    }

    void OnGUI() {
        // Important to keep from skipping render events
        if (Event.current.type != EventType.Repaint) {
            return;
        }

        /*
        // Fade in screen
        if(AlphaFadeValue > 0.0f) {
            AlphaFadeValue -= Mathf.Clamp01(Time.deltaTime / FadeInTime);
            if(AlphaFadeValue < 0.0f)
            {
                AlphaFadeValue = 0.0f;
            }
            else
            {
                GUI.color = new Color(0, 0, 0, AlphaFadeValue);
                GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), FadeInTexture );
                return;
            }
        }*/


        // We can turn on the render object so we can render the on-screen menu
        if (m_gui_render_object != null) {
            if (m_crosshair.IsCrosshairVisible() ||
                m_rift_present_timeout > 0.0f || m_device_detection_timeout > 0.0f ||
                ((m_mag_cal.Disabled () == false) && (m_mag_cal.Ready () == false))
                ) {
                m_gui_render_object.SetActive(true);
            } else {
                m_gui_render_object.SetActive(false);
            }
        }

        //***
        // Set the GUI matrix to deal with portrait mode
        Vector3 scale = Vector3.one;
        if (m_camera_controller.PortraitMode == true) {
            float h = OVRDevice.HResolution;
            float v = OVRDevice.VResolution;
            scale.x = v / h;                    // calculate hor scale
            scale.y = h / v;                    // calculate vert scale
        }
        Matrix4x4 svMat = GUI.matrix; // save current matrix
        // substitute matrix - only scale is altered from standard
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);

        // Cache current active render texture
        RenderTexture previousActive = RenderTexture.active;

        // if set, we will render to this texture
        if (m_gui_render_texture != null) {
            RenderTexture.active = m_gui_render_texture;
            GL.Clear (false, true, new Color (0.0f, 0.0f, 0.0f, 0.0f));
        }

        // Update OVRGUI functions (will be deprecated eventually when 2D renderingc
        // is removed from GUI)
        // m_gui_helper.SetFontReplace(FontReplace);

        m_crosshair.OnGUICrosshair();

        // Restore active render texture
        RenderTexture.active = previousActive;

        // ***
        // Restore previous GUI matrix
        GUI.matrix = svMat;
    }

}