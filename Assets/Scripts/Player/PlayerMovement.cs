using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private float playerSpeed = 10f;
    private Rigidbody2D playerRigidBody;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    public Transform playerPosition;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "teste" && PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");

            playerPosition.position = new Vector2(x, y);
        }
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;
        moveVelocity = playerSpeed * moveInput;
    }

    private void FixedUpdate()
    {
        if (playerRigidBody == null) return;

        playerRigidBody.linearVelocity = moveVelocity;
        //playerRigidBody.MovePosition(playerRigidBody.position + moveInput * playerSpeed * Time.fixedDeltaTime);


    }
}
