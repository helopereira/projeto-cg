using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;

    Vector3 forward;
    Vector3 strafe;
    Vector3 vertical;

    [Header("Configurações de Velocidade e Pulo")]
    public float forwardSpeed = 8f; 
    public float strafeSpeed = 8f;   

    float gravity; 
    float verticalSpeed = 0;
    float JumpSpeed;
    float maxJumpHeight = 2f;
    float timeToMaxHeight = 0.5f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        gravity=(-2*maxJumpHeight)/(timeToMaxHeight*timeToMaxHeight);
        JumpSpeed=(2*maxJumpHeight)/timeToMaxHeight;
    }

void Update()
{
    float forwardInput = Input.GetAxisRaw("Vertical");
    float strafeInput = Input.GetAxisRaw("Horizontal");

    forward = forwardInput * forwardSpeed * transform.forward;
    strafe = strafeInput * strafeSpeed * transform.right;

    if (controller.isGrounded)
    {
        verticalSpeed = -1f;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            verticalSpeed = JumpSpeed;
        }
    }
    verticalSpeed += gravity * Time.deltaTime;

    vertical = verticalSpeed * Vector3.up;
    Vector3 finalVelocity = forward + strafe + vertical;
        controller.Move(finalVelocity * Time.deltaTime);
    
}
}