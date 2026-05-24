using UnityEngine;

public class cameraMove : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
    void Start()
    {
        GameManager.instance.OnLevelLoaded += HandleLevelLoaded;
    }
    void HandleLevelLoaded(int i)
    {
        target = GameManager.instance.currentPlayerObj.transform;
    }
    }
