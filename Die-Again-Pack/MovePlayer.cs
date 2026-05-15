using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameInput1 gameInput1;
    [SerializeField] private bool isWalking;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rorateSpeed;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

 
         private void HandleMovement()
    {
       
            Vector2 inputVector = gameInput1.getMovementVectorNormalized();
       
        Vector3 moveDir = new Vector3(inputVector.x, transform.position.y, inputVector.y);


        if (moveDir != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);

            transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rorateSpeed);

            isWalking = true;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            isWalking = false;
        }
    

}
    public void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }    
    private void PCMove()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveDir != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
            transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * rorateSpeed);
            isWalking = true;
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isWalking = false;
        }
    }
void Update()
    {
        if (GameManager.instance.playerDie == false)
        {
            if (GameManager.instance.CheckType == CheckTypeDriver.moblie)
                HandleMovement();
            else if (GameManager.instance.CheckType == CheckTypeDriver.Pc)
            {
                PCMove();
                if (Input.GetKey(KeyCode.Space)) { Jump(); }
            }
        }
        else
        {

        }

    }
}
