using System.Collections;
using Photon.Pun;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //public Transform playerPosition;
    //PlayerMovement playerMovement;
    public float smoothSpeed = 0.5f;
    public Vector3 offset;

    public BoxCollider2D boxCollider2D;
    private float minX;
    private float maxX; 
    private float minY;
    private float maxY;

    [SerializeField]private Transform target;

    IEnumerator Start()
    {
        if (boxCollider2D != null)
        {
            minX = boxCollider2D.bounds.min.x;
            maxX = boxCollider2D.bounds.max.x;
            minY = boxCollider2D.bounds.min.y;
            maxY = boxCollider2D.bounds.max.y;
        }

        yield return new WaitForSeconds(0.5f);
        TryFindPlayer();
    }

    void Update()
    {
        if (target == null)
            TryFindPlayer();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        float clampX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        float clampY = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        transform.position = new Vector3(clampX, clampY, transform.position.z);
    }

    // Dentro de PlayerCamera.cs

    void TryFindPlayer()
    {
        // 1. Tenta encontrar o objeto do jogador local pela Tag.
        GameObject localPlayerObject = GameObject.FindWithTag("Player");

        if (localPlayerObject != null)
        {
            target = localPlayerObject.transform;
            Debug.Log("Camera agora seguindo o player local (via Tag).");
            return; // Sai da função após encontrar
        }

        // 2. Se não encontrar pela Tag, volta à lógica anterior (útil para multiplayer)
        foreach (var player in FindObjectsOfType<PlayerMovement>())
        {
            PhotonView view = player.GetComponent<PhotonView>();

            if (PhotonNetwork.OfflineMode || view == null || view.IsMine)
            {
                target = player.transform;
                Debug.Log("Camera agora seguindo o player local (via Photon).");
                break;
            }
        }
    }
}
