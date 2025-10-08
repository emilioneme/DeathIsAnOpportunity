using UnityEngine;

public class PlayerCameraLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHub inputHub;          // your hub
    [SerializeField] private Transform orientation;         // yaw (used by movement)
    [SerializeField] private Transform cameraPivot;         // pitch (parent of Camera)

    [Header("Sensitivity")]
    [SerializeField] private float mouseSensitivity = 1.5f; // mouse delta scale
    [SerializeField] private float stickSensitivity = 120f; // deg/sec for gamepad
    [SerializeField] private bool invertY = false;

    [Header("Pitch Clamp")]
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    [Header("Quality")]
    [SerializeField] private float yawSmoothing = 0f;       // 0 = raw, >0 = lerp
    [SerializeField] private float pitchSmoothing = 0f;

    // state
    private float targetYaw;
    private float targetPitch;
    private float smoothYaw;
    private float smoothPitch;

    // cursor
    [SerializeField] private bool lockCursorOnStart = true;
    public bool IsCursorLocked { get; private set; }

    void Awake()
    {
        if (!inputHub) inputHub = GetComponent<PlayerInputHub>();
        if (!orientation) orientation = transform;
        if (!cameraPivot) cameraPivot = transform;
    }

    void Start()
    {
        Vector3 euler = orientation.eulerAngles;
        targetYaw = smoothYaw = euler.y;

        euler = cameraPivot.localEulerAngles;
        // convert Unity’s 0..360 to -180..180 for clamping
        targetPitch = smoothPitch = NormalizeAngle(euler.x);

        if (lockCursorOnStart) SetCursorLock(true);
    }

    void Update()
    {
        // Toggle with Esc (simple default). If you later add an action (e.g., Pause),
        // call SetCursorLock(false) there instead.
        if (Input.GetKeyDown(KeyCode.Escape))
            SetCursorLock(!IsCursorLocked);

        // If unlocked, don’t rotate.
        if (!IsCursorLocked) return;

        Vector2 look = inputHub.Look; // mouse delta OR right-stick

        // Heuristic: if magnitude is large → treat as mouse delta; else treat as stick
        bool usingMouseLike = Mathf.Abs(look.x) > 2f || Mathf.Abs(look.y) > 2f;

        float dt = Time.deltaTime;
        float yawDelta, pitchDelta;

        if (usingMouseLike)
        {
            yawDelta   = look.x * mouseSensitivity;
            pitchDelta = look.y * mouseSensitivity * (invertY ? 1f : -1f);
        }
        else
        {
            yawDelta   = look.x * stickSensitivity * dt;
            pitchDelta = look.y * stickSensitivity * dt * (invertY ? -1f : 1f) * -1f;
        }

        targetYaw   += yawDelta;
        targetPitch += pitchDelta;
        targetPitch  = Mathf.Clamp(targetPitch, minPitch, maxPitch);

        // smoothing (optional)
        if (yawSmoothing > 0f)
            smoothYaw = Mathf.LerpAngle(smoothYaw, targetYaw, 1f - Mathf.Exp(-yawSmoothing * dt));
        else
            smoothYaw = targetYaw;

        if (pitchSmoothing > 0f)
            smoothPitch = Mathf.Lerp(smoothPitch, targetPitch, 1f - Mathf.Exp(-pitchSmoothing * dt));
        else
            smoothPitch = targetPitch;

        orientation.rotation = Quaternion.Euler(0f, smoothYaw, 0f);
        cameraPivot.localRotation = Quaternion.Euler(smoothPitch, 0f, 0f);
    }

    public void SetCursorLock(bool locked)
    {
        IsCursorLocked = locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible   = !locked;
    }

    // Handy if you wire an input action event to this from PlayerInput (optional)
    public void OnToggleCursorLock() => SetCursorLock(!IsCursorLocked);

    static float NormalizeAngle(float a)
    {
        a = Mathf.Repeat(a + 180f, 360f) - 180f;
        return a;
    }
}

