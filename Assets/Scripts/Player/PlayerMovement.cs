using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float playerSpeed = 5f;
    private Rigidbody2D playerRigidBody;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();        
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
