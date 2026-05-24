using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] private GameInput1 gameInput1;
    [SerializeField] private bool isWalking;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rorateSpeed;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask deadZoneLayer;

    private Vector2 currentInput;
    private float jumpBufferCounter;
    private float jumpBufferTime = 0.2f;
    private bool isDeadProcessed = false;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (GameManager.instance.playerDie == false)
        {
            if (GameManager.instance.CheckType == CheckTypeDriver.moblie)
            {
                currentInput = gameInput1.getMovementVectorNormalized();
            }
            else if (GameManager.instance.CheckType == CheckTypeDriver.Pc)
            {
                currentInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if (Input.GetKeyDown(KeyCode.Space)) { jumpBufferCounter = jumpBufferTime; }
            }

            if (jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (anim != null)
            {
                anim.SetFloat("Speed", currentInput.magnitude); 
                anim.SetBool("isDead", false);
            }
        }
        else
        {
            currentInput = Vector2.zero;
            if (!isDeadProcessed)
            {
                if (anim != null) 
                {
                    // Dừng các logic di chuyển/nhảy cũ
                    anim.SetFloat("Speed", 0f);
                    anim.ResetTrigger("Jump");
                    
                    // Kích hoạt animation chết
                    anim.SetBool("isDead", true);
                    
                    // Ép Animator cập nhật ngay lập tức trong frame này để hủy bỏ các animation đang dang dở
                    anim.Update(0f);
                }
                isDeadProcessed = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (GameManager.instance.playerDie == false)
        {
            CheckDeadZone();
            if (GameManager.instance.playerDie) return;

            HandleMovement(currentInput);

            if (jumpBufferCounter > 0)
            {
                if (TryJump())
                {
                    jumpBufferCounter = 0; 
                }
            }
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            rb.angularVelocity = Vector3.zero;
            isWalking = false;
        }
    }

    private void HandleMovement(Vector2 input)
    {
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

        if (moveDir != Vector3.zero)
        {
            rb.linearVelocity = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);
            transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.fixedDeltaTime * rorateSpeed);
            isWalking = true;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            rb.angularVelocity = Vector3.zero;
            isWalking = false;
        }
    }

    public void Jump()
    {
        TryJump();
    }

    private bool TryJump()
    {
       
        bool isGrounded = Physics.SphereCast(transform.position, 0.2f, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer) 
                          || Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded)
        {
          
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (anim != null) anim.SetTrigger("Jump");
            return true;
        }
        return false;
    }

    private void CheckDeadZone()
    {
        // Bắn tia raycast xuống dưới để kiểm tra layer DeadZone
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, deadZoneLayer))
        {
            if (GameManager.instance.playerDie == false)
            {
                GameManager.instance.PlayerDied();
            }
        }
    }
}
