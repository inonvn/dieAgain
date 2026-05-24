using UnityEngine;
using DG.Tweening;

public class dooropen : MonoBehaviour
{
    public Transform player;
    public Transform doorChild;
    public float detectionRadius = 3f;
    public float openAngle = 90f;
    public float animDuration = 0.5f;

    private bool isOpen = false;
    private void Start()
    {
        player = GameManager.instance.currentPlayerObj.transform;
    }
    void Update()
    {
        if (player == null || doorChild == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius && !isOpen)
        {
            isOpen = true;
            doorChild.DORotate(new Vector3(0, openAngle, 0), animDuration);
        }
        else if (distance > detectionRadius && isOpen)
        {
            isOpen = false;
            doorChild.DORotate(new Vector3(0, 0, 0), animDuration);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
