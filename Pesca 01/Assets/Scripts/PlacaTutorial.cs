using UnityEngine;

public class PlacaTutorial : MonoBehaviour
{
    [Header("Referência da UI")]
    public MinigameUIController uiController; 

    void Update()
    {
        if (uiController.popUpPanel.activeSelf) return;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 2f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                
                if (Input.GetMouseButtonDown(0)) 
                {
                    uiController.AbrirTutorial();
                }
            }
        }
    }
}