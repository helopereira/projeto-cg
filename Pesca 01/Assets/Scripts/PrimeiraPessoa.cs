using UnityEngine;

public class PrimeiraPessoa : MonoBehaviour
{
    [Header("Referências do Personagem")]
    public Transform characterBody; // Transform do corpo
    public Transform characterHead; // Transform da cabeça (ponto base da câmera)

    [Header("Configurações de Sensibilidade")]
    public float sensitivityX = 1.0f;
    public float sensitivityY = 1.0f;

    [Header("Limites de Rotação Vertical")]
    public float angleYmin = -90f;
    public float angleYmax = 90f;

    [Header("Suavização de Movimento")]
    [Range(0.01f, 1f)]
    public float smoothCoefx = 0.05f;
    [Range(0.01f, 1f)]
    public float smoothCoefy = 0.05f;

    [Header("Offset da Câmera")]
    public Vector3 cameraOffset = new Vector3(0, 0.5f, 0); // 0.5 unidades acima da cabeça por padrão

    private float rotationX = 0f;
    private float rotationY = 0f;

    private float smoothRotx = 0f;
    private float smoothRoty = 0f;

    void Start()
    {
        // Trava o cursor e oculta ele
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Captura movimentos do mouse
        float verticalDelta = Input.GetAxisRaw("Mouse Y") * sensitivityY;
        float horizontalDelta = Input.GetAxisRaw("Mouse X") * sensitivityX;

        // Suavização
        smoothRotx = Mathf.Lerp(smoothRotx, horizontalDelta, smoothCoefx);
        smoothRoty = Mathf.Lerp(smoothRoty, verticalDelta, smoothCoefy);

        rotationX += smoothRotx;
        rotationY += smoothRoty;

        // Limita rotação vertical
        rotationY = Mathf.Clamp(rotationY, angleYmin, angleYmax);

        // Rotaciona o corpo horizontalmente
        characterBody.localEulerAngles = new Vector3(0f, rotationX, 0f);

        // Rotaciona a câmera
        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
    }

    void LateUpdate()
    {
        // Atualiza posição da câmera com offset vertical
        transform.position = characterHead.position + cameraOffset;
    }
}
