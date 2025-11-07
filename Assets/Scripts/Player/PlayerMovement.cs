using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    private float playerSpeed = 10f;
    private Rigidbody2D playerRigidBody;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    public Transform playerPosition;
    private Vector2 networkPosition;

    private bool isNetworked = false;
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();

        isNetworked = photonView != null && photonView.ViewID != 0;

        if (!isNetworked || photonView.IsMine)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName != "gameTrello" && PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY"))
            {
                float x = PlayerPrefs.GetFloat("PlayerX");
                float y = PlayerPrefs.GetFloat("PlayerY");

                playerPosition.position = new Vector2(x, y);
            }
        }
    }

    void Update()
    {
        // O player pode se mover apenas localmente
        if (isNetworked && !photonView.IsMine) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;
        moveVelocity = playerSpeed * moveInput;
    }

    private void FixedUpdate()
    {
        if (playerRigidBody == null) return;

        if (!isNetworked || photonView.IsMine)
        {
            playerRigidBody.linearVelocity = moveVelocity;
        }
        else
        {
            // Interpolação suave dos outros jogadores
            playerRigidBody.position = Vector2.Lerp(playerRigidBody.position, networkPosition, Time.fixedDeltaTime * 10);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (!isNetworked) return;

        if (stream.IsWriting)
        {
            // Envia a posição do player para o servidor
            stream.SendNext(playerRigidBody.position);
        }
        else
        {
            // Recebe a posição dos outros jogadores
            networkPosition = (Vector2)stream.ReceiveNext();
        }
    }
}
