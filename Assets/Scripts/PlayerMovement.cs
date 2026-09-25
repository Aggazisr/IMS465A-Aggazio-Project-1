using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody rb;
    private float dashCooldown = 2f;
    private bool canDash = true;
    private float dashJumpTimeWindow = 0.25f;
    private float dashJumpTimeWait = 0.3f;
    private bool canDashJump = false;
    private float dashJumpTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dashJumpTimer = dashJumpTimeWindow + dashJumpTimeWait;
    }

    // Update is called once per frame
    void Update()
    {
        dashCooldown -= Time.deltaTime;
        if (dashCooldown <= 0f)
        {
            canDash = true;
        }
        dashJumpTimer -= Time.deltaTime;
        if (dashJumpTimer <= 0f)
        {
            canDashJump = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector3>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            float fullDashJumpTime = dashJumpTimeWindow + dashJumpTimeWait;
            if (canDashJump && dashJumpTimer <= fullDashJumpTime - dashJumpTimeWait)
            {
                canDashJump = false;
                rb.AddForce(Vector3.up * (jumpForce + 5f), ForceMode.Impulse);
                rb.AddForce(transform.forward * moveSpeed * 2f, ForceMode.Impulse);
                Debug.Log("Dash Jump");
            }
            else
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (canDash)
            {
                rb.AddForce(transform.forward * moveSpeed * 2f, ForceMode.Impulse);
                canDash = false;
                dashCooldown = 2f;
                dashJumpTimer = dashJumpTimeWindow + dashJumpTimeWait;
                canDashJump = true;
            }
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, lookInput.x / 7f);
    }

    void FixedUpdate()
    {
        transform.Translate(moveInput * Time.fixedDeltaTime * moveSpeed);
    }

}
