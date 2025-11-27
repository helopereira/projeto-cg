using UnityEngine;
using UnityEngine.UI;

public class PrimeiraPessoa : MonoBehaviour
{
    private bool isFpsActive = true; 

    [Header("Referências do Personagem")]
    public Transform characterBody; 
    public Transform characterHead; 

    [Header("Referência da Mira (opcional)")]
    public Image crosshair;

    [Header("Sensibilidade")]
    public float sensitivityX = 800f; 
    public float sensitivityY = 800f; 

    [Header("Limites de Rotação Vertical")]
    public float angleYmin = -90f;
    public float angleYmax = 90f;

    [Header("Offset da Câmera")]
    public Vector3 cameraOffset = new Vector3(0f, 0.5f, 0f);

    private float rotationX = 0f; 
    private float rotationY = 0f; 
    private Camera fpsCamera; 

    void Awake()
    {
        fpsCamera = GetComponent<Camera>();
        if (fpsCamera == null)
        {
            Debug.LogError("O script PrimeiraPessoa requer um componente Camera no mesmo GameObject.");
        }
    }

    void Start()
    {
        SetFpsActive(true); 

        rotationX = (characterBody != null) ? characterBody.eulerAngles.y : transform.eulerAngles.y;
        rotationY = transform.localEulerAngles.x;
        if (rotationY > 180f) rotationY -= 360f;
    }


    public void SetFpsActive(bool active)
    {
        isFpsActive = active;

        if (fpsCamera != null)
        {
            fpsCamera.enabled = active; 
        }

        if (active)
        {
            // Modo FPS
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            if (crosshair) crosshair.enabled = true; 
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            if (crosshair) crosshair.enabled = false;
        }
    }

    void Update()
    {
        if (!isFpsActive) return; 

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