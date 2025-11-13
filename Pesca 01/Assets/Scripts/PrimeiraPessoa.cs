using UnityEngine;
using UnityEngine.UI;

public class PrimeiraPessoa : MonoBehaviour
{
    [Header("Referências do Personagem")]
    public Transform characterBody;   // objeto que gira em yaw (horizontal)
    public Transform characterHead;   // posição onde a câmera ficará (applied in LateUpdate)

    [Header("Referência da Mira (opcional)")]
    public Image crosshair;

    [Header("Sensibilidade")]
    public float sensitivityX = 800f; // yaw
    public float sensitivityY = 800f; // pitch

    [Header("Limites de Rotação Vertical")]
    public float angleYmin = -90f;
    public float angleYmax = 90f;

    [Header("Suavização (opcional)")]
    public bool useSmoothing = false;
    [Range(0f, 1f)]
    public float smoothCoefx = 0.3f;
    [Range(0f, 1f)]
    public float smoothCoefy = 0.3f;

    [Header("Offset da Câmera")]
    public Vector3 cameraOffset = new Vector3(0f, 0.5f, 0f);

    private float rotationX = 0f; // yaw acumulado (gira o corpo)
    private float rotationY = 0f; // pitch acumulado (gira a câmera)
    private float smoothRotx = 0f;
    private float smoothRoty = 0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (crosshair) crosshair.enabled = true;

        // Inicializa rotationX/Y com as rotações atuais para evitar "jump" no começo
        rotationX = (characterBody != null) ? characterBody.eulerAngles.y : transform.eulerAngles.y;
        rotationY = transform.localEulerAngles.x;
        if (rotationY > 180f) rotationY -= 360f; // normalize to -180..180
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            return;
        }

        // 👇 Sensibilidade corrigida (sem deltaTime)
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        rotationX += mouseX;
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, angleYmin, angleYmax);

        if (characterBody)
            characterBody.localRotation = Quaternion.Euler(0f, rotationX, 0f);

        transform.localRotation = Quaternion.Euler(rotationY, 0f, 0f);
    }


    void LateUpdate()
    {
        if (characterHead != null)
            transform.position = characterHead.position + cameraOffset;
    }
}
