using UnityEngine;

public class MinigameUIController : MonoBehaviour
{
    [Header("Referência da UI")]
    public GameObject popUpPanel;
    
    [Header("Referência do Controle FPS")]
    public PrimeiraPessoa fpsController; 
    public void AbrirTutorial()
    {
        if (popUpPanel == null) return;
        
        popUpPanel.SetActive(true);

        if (fpsController != null)
        {
            fpsController.SetFpsActive(false);
        }
        
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;                 
    }

    public void FecharTutorial()
    {
        if (popUpPanel == null) return;

        popUpPanel.SetActive(false);

        if (fpsController != null)
        {
            fpsController.SetFpsActive(true); 
        }
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }
}