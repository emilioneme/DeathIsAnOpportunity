using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementRB : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHub inputHub;
    [SerializeField] private Transform orientation; // usually the camera rig (yaw only)
    [SerializeField] private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;          // target ground speed
    [SerializeField] private float maxHorizontalSpeed = 10f;
    [SerializeField] private float groundAcceleration = 40f; // how fast to reach target speed
    [SerializeField] private float airAcceleration = 15f;    // weaker air control
    [SerializeField] private float groundLinearDrag = 6f;
    [SerializeField] private float airLinearDrag = 0.5f;

    [Header("Jumping")]
    [SerializeField] private float jumpImpulse = 7.5f; // physics-based jump (Impulse)
    [SerializeField] private int maxAirJumps = 1;      // 1 = double jump
    [SerializeField] private float coyoteTime = 0.12f; // grace after leaving ground
    [SerializeField] private float jumpBuffer = 0.12f; // press slightly before landing
    [SerializeField] private float fallGravityMultiplier = 2.0f;     // faster fall
    [SerializeField] private float lowJumpGravityMultiplier = 2.5f;  // when JumpHeld is false on ascent

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;  // a child at feet
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayers = ~0;

    // state
    private bool grounded;
    private int airJumpsUsed = 0;
    private float lastGroundedTime;
    private float lastJumpPressedTime;

    public void ApplyUpgradeData(PlayerUpgradeData data)
    {
        if (data == null) return;

        moveSpeed = data.MoveSpeed;
        maxHorizontalSpeed = data.MaxHorizontalSpeed;
        groundAcceleration = data.GroundAcceleration;
        airAcceleration = data.AirAcceleration;
        groundLinearDrag = data.GroundLinearDrag;
        airLinearDrag = data.AirLinearDrag;
        jumpImpulse = data.JumpImpulse;
        maxAirJumps = data.MaxAirJumps;
        coyoteTime = data.CoyoteTime;
        jumpBuffer = data.JumpBuffer;
        fallGravityMultiplier = data.FallGravityMultiplier;
        lowJumpGravityMultiplier = data.LowJumpGravityMultiplier;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotation; // prevent tipping
        if (!inputHub) inputHub = GetComponent<PlayerInputHub>();
        if (!orientation) orientation = Camera.main ? Camera.main.transform : transform;
        
    }

    void Update()
    {
        // --- Ground check & timers ---
        grounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayers, QueryTriggerInteraction.Ignore);
        if (grounded)
        {
            lastGroundedTime = Time.time;
            airJumpsUsed = 0;
        }
        
        // Buffer jump input
        if (inputHub.JumpPressed)
            lastJumpPressedTime = Time.time;
    }

    void FixedUpdate()
    {
        // Try to consume buffered jump (this allows the player to press jump slightly before landing)
        if (Time.time - lastJumpPressedTime <= jumpBuffer)
        {
            // if within coyote window -> grounded jump (this allows jumping shortly after leaving a platform)
            if (Time.time - lastGroundedTime <= coyoteTime)
            {
                DoJump();
                lastJumpPressedTime = -999f; // consume
            }
            // else if we have air jumps left
            else if (airJumpsUsed < maxAirJumps)
            {
                DoJump();
                airJumpsUsed++;
                lastJumpPressedTime = -999f; // consume
            }
        }

        // Extra gravity for snappy feel, removed to make jumps feel better
        ApplyBetterJumpGravity();
        Vector3 desired = CameraRelativeMove(inputHub.Move);
        if(desired.sqrMagnitude > 0.01f)
            MoveTowards(desired);
        CapHorizontalSpeed(maxHorizontalSpeed);
        rb.linearDamping = grounded ? groundLinearDrag : airLinearDrag;
    }

    // --- Helpers ---

    Vector3 CameraRelativeMove(Vector2 move)
    {
        // Project camera forward/right onto plane (y=0)
        Vector3 forward = orientation.forward;  forward.y = 0f; forward.Normalize();
        Vector3 right = orientation.right;  right.y = 0f; right.Normalize();
        Vector3 direction = forward * move.y + right * move.x;
        return direction.normalized; // direction only; speed controlled in MoveTowards
    }

    void MoveTowards(Vector3 desiredDir)
    {
        // Current horizontal velocity
        Vector3 v = rb.linearVelocity;
        Vector3 vHoriz = new Vector3(v.x, 0f, v.z);

        // Target horizontal velocity at moveSpeed
        Vector3 target = desiredDir * moveSpeed;

        // Compute desired change and apply accel
        Vector3 delta = target - vHoriz;
        float accel = grounded ? groundAcceleration : airAcceleration;

        // Acceleration as ForceMode.Acceleration (mass-independent)
        rb.AddForce(delta.normalized * accel, ForceMode.Acceleration);

        // Gentle braking when no input on ground
        if (grounded && desiredDir.sqrMagnitude < 0.0001f)
        {
            // counter force to stop sliding
            rb.AddForce(-vHoriz * groundAcceleration, ForceMode.Acceleration);
        }
    }

    void DoJump()
    {
        // Reset downward velocity so short hops feel responsive
        Vector3 velocity = rb.linearVelocity;
        if (velocity.y < 0f) velocity.y = 0f;
        rb.linearVelocity = new Vector3(velocity.x, velocity.y, velocity.z);

        rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
    }

    void ApplyBetterJumpGravity()
    {
        if (rb.linearVelocity.y < 0f)
        {
            // Falling
            rb.AddForce(Physics.gravity * (fallGravityMultiplier - 1f), ForceMode.Acceleration);
        }
        else if (rb.linearVelocity.y > 0f && !inputHub.JumpHeld)
        {
            // Rising but jump released -> low jump
            rb.AddForce(Physics.gravity * (lowJumpGravityMultiplier - 1f), ForceMode.Acceleration);
        }
        // else: default gravity already applied by Unity
    }

    void CapHorizontalSpeed(float maxSpeed)
    {
        Vector3 v = rb.linearVelocity;
        Vector3 horiz = new Vector3(v.x, 0f, v.z);
        float mag = horiz.magnitude;
        if (mag > maxSpeed)
        {
            Vector3 clamped = horiz.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(clamped.x, v.y, clamped.z);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (groundCheck)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
#endif
}
