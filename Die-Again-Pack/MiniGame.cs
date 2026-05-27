using UnityEngine;

public class MiniGame : MonoBehaviour
{
    [Header("Trap Settings (Vùng kích hoạt)")]
    public GameObject trapObject; 
    public bool isFallingTrap; 
    public bool isAnimationTrap; 
    public string animationTriggerName = "Activate";
    
    [Header("Moving Trap (Bẫy di chuyển)")]
    public bool isMovingTrap; 
    public Transform targetMovePoint; 
    public float moveSpeed = 15f;

    [Header("Disappear Trap (Bẫy biến mất)")]
    public bool isDisappearTrap; // Đánh dấu nếu muốn trapObject tự động biến mất
    public float delayDisappear = 0f; // Thời gian chờ trước khi biến mất (giây)

    [Header("Teleport Trap (Bẫy dịch chuyển)")]
    public bool isTeleportTrap; // Đánh dấu nếu muốn dịch chuyển người chơi
    public Transform teleportPoint; // Vị trí đích đến

    [Header("Push Trap (Bẫy đẩy)")]
    public bool isPushTrap; // Đánh dấu nếu là bẫy đẩy người chơi
    public float pushForce = 25f; // Lực đẩy
    public bool useTrapForward = true; // Sử dụng hướng transform.forward của bẫy
    public Vector3 customPushDirection = Vector3.forward; // Hướng đẩy tùy chỉnh nếu không dùng forward

    [Header("Death Zone (Vùng gây chết)")]
    public bool isDeathZone; // Nếu tích vào đây, khi chạm vào Player sẽ chết ngay
    public Effect trapEffect = Effect.None; // Hiệu ứng đặc biệt nếu có
    
    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Player") && !isActivated)
        {
            if (isDeathZone)
            {
                Debug.Log("Player đụng bẫy và chết!");
                if (GameManager.instance != null)
                {
                    GameManager.instance.PlayerDied();
                }
            }
            else
            {
                
                if (isTeleportTrap && teleportPoint != null)
                {
                    
                    CharacterController cc = other.GetComponent<CharacterController>();
                    if (cc != null) cc.enabled = false;
                    
                    other.transform.position = teleportPoint.position;
                    
                    if (cc != null) cc.enabled = true;
                }

                if (isPushTrap)
                {
                    MovePlayer movePlayer = other.GetComponent<MovePlayer>();
                    if (movePlayer != null)
                    {
                        Vector3 pushDir = useTrapForward ? transform.forward : customPushDirection.normalized;
                        movePlayer.ApplyPush(pushDir * pushForce);
                    }
                }

         
                isActivated = true;
                ActivateTrap();
            }
        }
    }

    private void ActivateTrap()
    {
       
        if (isFallingTrap && trapObject != null)
        {
            Rigidbody rb = trapObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }

       
        if (isAnimationTrap && trapObject != null)
        {
            Animator anim = trapObject.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger(animationTriggerName);
            }
        }

      
        if (isDisappearTrap && trapObject != null)
        {
            if (delayDisappear > 0)
            {
                Destroy(trapObject, delayDisappear);
            }
            else
            {
                trapObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
       
        if (isActivated && isMovingTrap && trapObject != null && targetMovePoint != null)
        {
            trapObject.transform.position = Vector3.MoveTowards(
                trapObject.transform.position, 
                targetMovePoint.position, 
                moveSpeed * Time.deltaTime
            );
        }
    }
}

public enum Effect
{
    None,
    changeKey,
}
