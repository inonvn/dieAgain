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

    [Header("Death Zone (Vùng gây chết)")]
    public bool isDeathZone; // Nếu tích vào đây, khi chạm vào Player sẽ chết ngay
    public Effect trapEffect = Effect.None; // Hiệu ứng đặc biệt nếu có
    
    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là Player không
        if (other.CompareTag("Player") && !isActivated)
        {
            if (isDeathZone)
            {
                // Xử lý khi Player chạm vùng chết
                Debug.Log("Player đụng bẫy và chết!");
                // Bạn có thể gọi GameManager để reload lại scene ở đây:
                // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
            else
            {
                // Dịch chuyển người chơi nếu đây là bẫy dịch chuyển
                if (isTeleportTrap && teleportPoint != null)
                {
                    // Tạm tắt CharacterController (nếu có) để tránh lỗi không nhận position mới
                    CharacterController cc = other.GetComponent<CharacterController>();
                    if (cc != null) cc.enabled = false;
                    
                    other.transform.position = teleportPoint.position;
                    
                    if (cc != null) cc.enabled = true;
                }

                // Nếu là vùng kích hoạt bẫy (Trigger)
                isActivated = true;
                ActivateTrap();
            }
        }
    }

    private void ActivateTrap()
    {
        // 1. Kích hoạt bẫy rơi (Dùng Rigidbody)
        if (isFallingTrap && trapObject != null)
        {
            Rigidbody rb = trapObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }

        // 2. Kích hoạt bẫy Animation (Ví dụ: Cây ngả đổ xuống)
        if (isAnimationTrap && trapObject != null)
        {
            Animator anim = trapObject.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger(animationTriggerName);
            }
        }

        // 4. Kích hoạt bẫy biến mất
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
        // 3. Xử lý bẫy di chuyển
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
