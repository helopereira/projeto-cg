using UnityEngine;

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

        if (objectRenderer != null && normalMaterial != null)
        {
            objectRenderer.material = normalMaterial;
        }
    }

    public void OnLookEnter()
    {
        if (objectRenderer != null && hoverMaterial != null)
        {
            objectRenderer.material = hoverMaterial;
        }
    }
    public void OnLookExit()
    {
        if (objectRenderer != null && normalMaterial != null)
        {
            objectRenderer.material = normalMaterial;
        }
    }

    public void PerformClickAction()
    {
        if (objectRenderer != null && clickedMaterial != null)
        {
            objectRenderer.material = clickedMaterial; 
        }
        
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(!objectToToggle.activeSelf);
        }

        Invoke("ResetToHoverMaterial", 0.2f);
    }

    void ResetToHoverMaterial()
    {
        if (objectRenderer != null && hoverMaterial != null)
        {
            objectRenderer.material = hoverMaterial;
        }
    }
}