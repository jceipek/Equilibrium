using UnityEngine;
using System.Collections;

public class ThreeDimGUI : MonoBehaviour
{

    public GameObject m_Target;

    // Handle to OVRCameraController
    private OVRCameraController m_CameraController = null;

    // Rift detection
    private bool   HMDPresent           = false;
    private bool   SensorPresent        = false;
    private float  m_RiftPresentTimeout   = 0.0f;
    private string strRiftPresent       = "";

    // Device attach / detach
    public enum DEVICE {HMDSensor, HMD, LatencyTester}
    private float  m_DeviceDetectionTimeout   = 0.0f;
    private string strDeviceDetection       = "";

    // Mag yaw-drift correction
    private OVRMagCalibration   m_MagCal     = new OVRMagCalibration();

    // Replace the GUI with our own texture and 3D plane that
    // is attached to the rendder camera for true 3D placement
    private OVRGUI          m_GuiHelper         = new OVRGUI();
    private GameObject      m_GUIRenderObject  = null;
    private RenderTexture   m_GUIRenderTexture = null;

    // Crosshair system, rendered onto 3D plane
    public Texture  m_CrosshairImage          = null;
    private SelectionCursor m_Crosshair          = new SelectionCursor();

    // Create a delegate for update functions
    private delegate void updateFunctions();
    private updateFunctions UpdateFunctions;

    void Awake()
    {
        // Find camera controller
        OVRCameraController[] camera_controllers;
        camera_controllers = gameObject.GetComponentsInChildren<OVRCameraController>();

        if (camera_controllers.Length == 0)
            Debug.LogWarning("ThreeDimGUI: No OVRCameraController attached.");
        else if (camera_controllers.Length > 1)
            Debug.LogWarning("ThreeDimGUI: More then 1 OVRCameraController attached.");
        else
            m_CameraController = camera_controllers[0];
    }

    // Use this for initialization
    void Start ()
    {

        // Ensure that camera controller variables have been properly
        // initialized before we start reading them
        if (m_CameraController != null)
        {
            m_CameraController.InitCameraControllerVariables();
            m_GuiHelper.SetCameraController(ref m_CameraController);
        }

        // Set the GUI target
        m_GUIRenderObject = GameObject.Instantiate(Resources.Load("OVRGUIObjectMain")) as GameObject;

        if (m_GUIRenderObject != null)
        {
            if (m_GUIRenderTexture == null)
            {
                int w = Screen.width;
                int h = Screen.height;

                if (m_CameraController.PortraitMode == true)
                {
                    int t = h;
                    h = w;
                    w = t;
                }

                m_GUIRenderTexture = new RenderTexture(w, h, 24);
                m_GuiHelper.SetPixelResolution(w, h);
                m_GuiHelper.SetDisplayResolution(OVRDevice.HResolution, OVRDevice.VResolution);
            }
        }

        // Attach GUI texture to GUI object and GUI object to Camera
        if (m_GUIRenderTexture != null && m_GUIRenderObject != null)
        {
            m_GUIRenderObject.renderer.material.mainTexture = m_GUIRenderTexture;

            if (m_CameraController != null)
            {
                // Grab transform of GUI object
                Transform t = m_GUIRenderObject.transform;
                // Attach the GUI object to the camera
                m_CameraController.AttachGameObjectToCamera(ref m_GUIRenderObject);
                // Reset the transform values (we will be maintaining state of the GUI object
                // in local state)
                OVRUtils.SetLocalTransform(ref m_GUIRenderObject, ref t);
                // Deactivate object until we have completed the fade-in
                // Also, we may want to deactive the render object if there is nothing being rendered
                // into the UI
                // we will move the position of everything over to the left, so get
                // IPD / 2 and position camera towards negative X
                Vector3 lp = m_GUIRenderObject.transform.localPosition;
                float ipd = 0.0f;
                m_CameraController.GetIPD(ref ipd);
                lp.x -= ipd * 0.5f;
                m_GUIRenderObject.transform.localPosition = lp;

                m_GUIRenderObject.SetActive(false);
            }
        }

        // Make sure to hide cursor
        if (Application.isEditor == false)
        {
            Screen.showCursor = false;
            Screen.lockCursor = true;
        }

        // Device updates
        UpdateFunctions += UpdateDeviceDetection;

        // Mag Yaw-Drift correction
        UpdateFunctions += m_MagCal.UpdateMagYawDriftCorrection;
        m_MagCal.SetOVRCameraController(ref m_CameraController);

        // Crosshair functionality
        m_Crosshair.Init();
        m_Crosshair.SetCrosshairTexture(ref m_CrosshairImage);
        m_Crosshair.SetOVRCameraController (ref m_CameraController);
        m_Crosshair.SetTarget(ref m_Target);
        //m_Crosshair.SetContinuousInputController (ref m_continuous_input_controller);
        UpdateFunctions += m_Crosshair.UpdateCrosshair;

        // Check for HMD and sensor
        CheckIfRiftPresent();
    }

    // RIFT DETECTION

    // CheckIfRiftPresent
    // Checks to see if HMD and / or sensor is available, and displays a
    // message if it is not
    void CheckIfRiftPresent()
    {
        HMDPresent = OVRDevice.IsHMDPresent();
        SensorPresent = OVRDevice.IsSensorPresent(0); // 0 is the main head sensor

        if ((HMDPresent == false) || (SensorPresent == false)) {
            m_RiftPresentTimeout = 5.0f; // Keep message up for 10 seconds

            if ((HMDPresent == false) && (SensorPresent == false))
                strRiftPresent = "NO HMD AND SENSOR DETECTED";
            else if (HMDPresent == false)
                strRiftPresent = "NO HMD DETECTED";
            else if (SensorPresent == false)
                strRiftPresent = "NO SENSOR DETECTED";
        }
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateFunctions();
    }

    void UpdateDeviceDetection ()
    {
        if (m_RiftPresentTimeout > 0.0f)
            m_RiftPresentTimeout -= Time.deltaTime;

        if (m_DeviceDetectionTimeout > 0.0f)
            m_DeviceDetectionTimeout -= Time.deltaTime;
    }

    // UpdateDeviceDetectionMsgCallback
    void UpdateDeviceDetectionMsgCallback(DEVICE device, bool attached)
    {
        if (attached == true)
        {
            switch (device)
            {
                case (DEVICE.HMDSensor):
                    strDeviceDetection = "HMD SENSOR ATTACHED";
                    break;

                case (DEVICE.HMD):
                    strDeviceDetection = "HMD ATTACHED";
                    break;

                case (DEVICE.LatencyTester):
                    strDeviceDetection = "LATENCY SENSOR ATTACHED";
                    break;
            }
        }
        else
        {
            switch (device)
            {
                case (DEVICE.HMDSensor):
                    strDeviceDetection = "HMD SENSOR DETACHED";
                    break;

                case (DEVICE.HMD):
                    strDeviceDetection = "HMD DETACHED";
                    break;

                case (DEVICE.LatencyTester):
                    strDeviceDetection = "LATENCY SENSOR DETACHED";
                    break;
            }
        }
    }

    void OnGUI()
    {
        // Important to keep from skipping render events
        if (Event.current.type != EventType.Repaint)
        {
            return;
        }

        // We can turn on the render object so we can render the on-screen menu
        if (m_GUIRenderObject != null)
        {
            if (m_Crosshair.IsCrosshairVisible() ||
                m_RiftPresentTimeout > 0.0f ||
                m_DeviceDetectionTimeout > 0.0f ||
                ((m_MagCal.Disabled() == false) && (m_MagCal.Ready() == false))
               )
            {
                m_GUIRenderObject.SetActive(true);
            }
            else
            {
                m_GUIRenderObject.SetActive(false);
            }
        }

        //***
        // Set the GUI matrix to deal with portrait mode
        Vector3 scale = Vector3.one;
        if (m_CameraController.PortraitMode == true)
        {
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
        if (m_GUIRenderTexture != null)
        {
            RenderTexture.active = m_GUIRenderTexture;
            GL.Clear (false, true, new Color (0.0f, 0.0f, 0.0f, 0.0f));
        }

        // Update OVRGUI functions (will be deprecated eventually when 2D renderingc
        // is removed from GUI)
        // m_GuiHelper.SetFontReplace(FontReplace);

        m_Crosshair.OnGUICrosshair();

        // Restore active render texture
        RenderTexture.active = previousActive;

        // ***
        // Restore previous GUI matrix
        GUI.matrix = svMat;
    }

}