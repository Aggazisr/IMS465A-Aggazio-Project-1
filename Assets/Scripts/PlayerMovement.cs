using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private TMP_Text tooWhatever;
    [SerializeField] private Image barOne;
    [SerializeField] private Image barTwo;
    [SerializeField] private Image barThree;
    [SerializeField] private Image barFour;
    private Rigidbody rb;
    private float dashCooldown = 2f;
    private bool canDash = true;
    private float dashJumpTimeWindow = 0.25f;
    private float dashJumpTimeWait = 0.3f;
    private bool canDashJump = false;
    private float dashJumpTimer = 0f;
    private float textChangeTimer = 0f;
    private float barCount = 4f;
    private float barUpiesTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dashJumpTimer = dashJumpTimeWindow + dashJumpTimeWait;
        tooWhatever.SetText(" ");
        barCount = 4f;
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
        textChangeTimer += Time.deltaTime;
        if (textChangeTimer >= 2f)
        {
            tooWhatever.SetText(" ");
        }
        barUpiesTimer += Time.deltaTime;
        if (barUpiesTimer >= 2f && barCount < 4f)
        {
            barCount += 1f;
            barUpiesTimer = 0f;
        }
        if (barCount == 4f)
        {
            barOne.enabled = true;
            barTwo.enabled = true;
            barThree.enabled = true;
            barFour.enabled = true;
        }
        else if (barCount == 3f)
        {
            barOne.enabled = true;
            barTwo.enabled = true;
            barThree.enabled = true;
            barFour.enabled = false;
        }
        else if (barCount == 2f)
        {
            barOne.enabled = true;
            barTwo.enabled = true;
            barThree.enabled = false;
            barFour.enabled = false;
        }
        else if (barCount == 1f)
        {
            barOne.enabled = true;
            barTwo.enabled = false;
            barThree.enabled = false;
            barFour.enabled = false;
        }
        else if (barCount <= 0f)
        {
            barOne.enabled = false;
            barTwo.enabled = false;
            barThree.enabled = false;
            barFour.enabled = false;
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
            if (barCount > 0f)
            {
                barCount -= 1f;
                barUpiesTimer = 0f;
                if (canDashJump && dashJumpTimer <= fullDashJumpTime - dashJumpTimeWait)
                {
                    canDashJump = false;
                    rb.AddForce(Vector3.up * (jumpForce + 5f), ForceMode.Impulse);
                    rb.AddForce(transform.forward * moveSpeed * 2f, ForceMode.Impulse);
                    Debug.Log("Dash Jump");
                }
                else
                {
                tooWhatever.SetText(" ");
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                if (dashJumpTimer >= dashJumpTimeWait)
                {
                tooWhatever.SetText("Too Early");
                textChangeTimer = 0f;
                }
                else if (dashJumpTimer < 0f && canDash == false)
                {
                tooWhatever.SetText("Too Late");
                textChangeTimer = 0f;
                }
            }
            
        }
       
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (barCount > 0f)
            {
                if (canDash)
                {
                    barCount -= 1f;
                    barUpiesTimer = 0f;
                    rb.AddForce(transform.forward * moveSpeed * 2f, ForceMode.Impulse);
                    canDash = false;
                    dashCooldown = 2f;
                    dashJumpTimer = dashJumpTimeWindow + dashJumpTimeWait;
                    canDashJump = true;
                }
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
