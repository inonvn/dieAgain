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
    [SerializeField] private float pushDecaySpeed = 5f;

    private Vector2 currentInput;
    private float jumpBufferCounter;
    private float jumpBufferTime = 0.2f;
    private bool isDeadProcessed = false;
    private Vector3 externalPushVelocity;

    public void ApplyPush(Vector3 force)
    {
        externalPushVelocity = force;
    }

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (GameManager.instance.playerDie == false && GameManager.instance.isSettingsOpen == false)
        {
            if (GameManager.instance.CheckType == CheckTypeDriver.moblie)
            {
                gameInput1.joystick=GameManager.instance.gameInput1;
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
            if (GameManager.instance.playerDie)
            {
                if (!isDeadProcessed)
                {
                    if (anim != null) 
                    {
                        
                        anim.SetFloat("Speed", 0f);
                        anim.ResetTrigger("Jump");
                        
                        
                        anim.SetBool("isDead", true);
                        
                     
                        anim.Update(0f);
                    }
                    isDeadProcessed = true;
                }
            }
            else
            {
                if (anim != null)
                {
                    anim.SetFloat("Speed", 0f);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (GameManager.instance.playerDie == false && GameManager.instance.isSettingsOpen == false)
        {
            CheckDeadZone();
            if (GameManager.instance.playerDie) return;

            HandleMovement(currentInput);

            if (externalPushVelocity.magnitude > 0.05f)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x + externalPushVelocity.x, rb.linearVelocity.y + externalPushVelocity.y, rb.linearVelocity.z + externalPushVelocity.z);
                externalPushVelocity = Vector3.Lerp(externalPushVelocity, Vector3.zero, Time.fixedDeltaTime * pushDecaySpeed);
            }
            else
            {
                externalPushVelocity = Vector3.zero;
            }

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
        
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, deadZoneLayer))
        {
            if (GameManager.instance.playerDie == false)
            {
                GameManager.instance.PlayerDied();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckDoorCollision(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckDoorCollision(collision.gameObject);
    }

    private void CheckDoorCollision(GameObject obj)
    {
        bool isDoor = obj.CompareTag("Door");
        if (isDoor)
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        if (GameManager.instance != null && !GameManager.instance.playerDie)
        {
            int nextLevelIndex = GameManager.instance.LvNow + 1;
            if (nextLevelIndex < GameManager.instance.SaveLV.Count)
            {
                GameManager.instance.LoadLV(nextLevelIndex);
            }
            else
            {
                
                UI_Manager uiManager = FindObjectOfType<UI_Manager>();
                if (uiManager != null && uiManager.ShowEnd != null)
                {
                    uiManager.ShowEnd.gameObject.SetActive(true);
                    RandomInon.FadeOut(uiManager.ShowEnd);
                }
            }
        }
    }
}

