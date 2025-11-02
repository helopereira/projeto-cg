using UnityEngine;

public class PrimeiraPessoa : MonoBehaviour
{
    [Header("Referências do Personagem")]
    public Transform characterBody; 
    public Transform characterHead; 

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
    public Vector3 cameraOffset = new Vector3(0, 0.5f, 0); 

    private float rotationX = 0f;
    private float rotationY = 0f;
    private float smoothRotx = 0f;
    private float smoothRoty = 0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float verticalDelta = Input.GetAxisRaw("Mouse Y") * sensitivityY;
        float horizontalDelta = Input.GetAxisRaw("Mouse X") * sensitivityX;

        smoothRotx = Mathf.Lerp(smoothRotx, horizontalDelta, smoothCoefx);
        smoothRoty = Mathf.Lerp(smoothRoty, verticalDelta, smoothCoefy);

        rotationX += smoothRotx;
        rotationY += smoothRoty;

        rotationY = Mathf.Clamp(rotationY, angleYmin, angleYmax);

        characterBody.localEulerAngles = new Vector3(0f, rotationX, 0f);
        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
    }

    void LateUpdate()
    {
        transform.position = characterHead.position + cameraOffset;
    }
}