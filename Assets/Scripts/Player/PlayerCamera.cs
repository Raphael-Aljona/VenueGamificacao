using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //public Transform playerPosition;
    PlayerMovement playerMovement;
    public float smoothSpeed = 0.5f;
    public Vector3 offset;

    public BoxCollider2D boxCollider2D;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    
        if(boxCollider2D != null)
        {
            minX = boxCollider2D.bounds.min.x;
            maxX = boxCollider2D.bounds.max.x;
            minY = boxCollider2D.bounds.min.y;
            maxY = boxCollider2D.bounds.max.y;
        }
    }

    void LateUpdate()
    {
        if (playerMovement.playerPosition == null) return;

        Vector3 desiredPosition = playerMovement.playerPosition.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        float clampX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        float clampY = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        transform.position = new Vector3(clampX, clampY, transform.position.z);
    }
}
