using UnityEngine;

// Remove as interfaces IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
public class ClickableObjectFeedback : MonoBehaviour 
{
    public GameObject objectToToggle;

    [Header("Feedback Visual")]
    public Material normalMaterial; 
    public Material hoverMaterial;  
    public Material clickedMaterial; 

    private Renderer objectRenderer; 

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();

        // (Seus fallbacks de material omitidos para brevidade, mas devem ser mantidos)

        if (objectRenderer != null && normalMaterial != null)
        {
            objectRenderer.material = normalMaterial;
        }
    }

    // Método PÚBLICO chamado pelo script da Câmera quando o mouse entra na mira
    public void OnLookEnter()
    {
        if (objectRenderer != null && hoverMaterial != null)
        {
            objectRenderer.material = hoverMaterial;
        }
    }

    // Método PÚBLICO chamado pelo script da Câmera quando o mouse sai da mira
    public void OnLookExit()
    {
        if (objectRenderer != null && normalMaterial != null)
        {
            objectRenderer.material = normalMaterial;
        }
    }

    // Método PÚBLICO chamado pelo script da Câmera quando o objeto é clicado
    public void PerformClickAction()
    {
        // 1. Lógica de Feedback Visual (Cor de Clique)
        if (objectRenderer != null && clickedMaterial != null)
        {
            objectRenderer.material = clickedMaterial; 
        }
        
        // 2. Lógica de Esconder/Mostrar
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }

        // 3. Reseta o material para o Hover após o clique
        Invoke("ResetToHoverMaterial", 0.2f);
    }

    void ResetToHoverMaterial()
    {
        if (objectRenderer != null && hoverMaterial != null)
        {
            // Volta para a cor de hover, já que o jogador ainda está olhando para ele.
            objectRenderer.material = hoverMaterial;
        }
    }
}