using UnityEngine;
using System.Collections.Generic; 

public class InfoPanelController : MonoBehaviour
{

    public static InfoPanelController Instance { get; private set; }

    [Header("Componentes de UI")]
    [Tooltip("Lista de todos os painéis de informação. O índice [0] corresponde ao ID 1.")]
    public List<GameObject> infoPanels;
    
    private GameObject currentActivePanel = null;
    private bool isPanelActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        foreach (var panel in infoPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            HidePanel();
        }
    }

    public void ShowPanel(int id)
    {
        int index = id - 1;
        if (index >= 0 && index < infoPanels.Count && infoPanels[index] != null)
        {
            if (currentActivePanel != null && currentActivePanel != infoPanels[index])
            {
                currentActivePanel.SetActive(false);
            }
            
            currentActivePanel = infoPanels[index];
            currentActivePanel.SetActive(true);
            isPanelActive = true;
            Time.timeScale = 0f;
        }
        else
        {
            Debug.LogWarning($"InfoPanelController: ID de painel inválido ({id}) ou painel não atribuído.");
        }
    }

    public void HidePanel()
    {
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            currentActivePanel = null;
            isPanelActive = false;
            Time.timeScale = 1f; 
        }
    }
}