using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;

    Vector3 forward;
    Vector3 strafe;
    Vector3 vertical;

    [Header("Configurações de Velocidade e Pulo")]
    [Tooltip("Velocidade de movimento para frente e para trás.")]
    public float forwardSpeed = 8f; // AUMENTADO (exemplo)
    [Tooltip("Velocidade de movimento lateral (strafe).")]
    public float strafeSpeed = 8f;   // AUMENTADO (exemplo)

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

    // Update is called once per frame
   // Update is called once per frame
void Update()
{
    // --- MOVIMENTO HORIZONTAL (já estava perfeito) ---
    float forwardInput = Input.GetAxisRaw("Vertical");
    float strafeInput = Input.GetAxisRaw("Horizontal");

    forward = forwardInput * forwardSpeed * transform.forward;
    strafe = strafeInput * strafeSpeed * transform.right;

    // --- LÓGICA VERTICAL UNIFICADA (aqui está a correção) ---

    // 1. Se estivermos no chão, nossa velocidade vertical para de acumular gravidade.
    if (controller.isGrounded)
    {
        verticalSpeed = -1f; // Força pequena para manter no chão

        // 2. Checa o input do pulo SOMENTE se estivermos no chão.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Dá a velocidade inicial para o pulo!
            verticalSpeed = JumpSpeed;
        }
    }

    // 3. Aplica a gravidade à velocidade vertical a cada segundo.
    // Isso faz o personagem começar a cair quando está no ar.
    verticalSpeed += gravity * Time.deltaTime;

    // 4. Cria o vetor de movimento vertical final a partir da nossa velocidade.
    vertical = verticalSpeed * Vector3.up;

    // --- APLICA O MOVIMENTO FINAL ---
    Vector3 finalVelocity = forward + strafe + vertical;
    controller.Move(finalVelocity * Time.deltaTime);
}
}
