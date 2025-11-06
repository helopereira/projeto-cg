using UnityEngine;
using System.Collections.Generic; // Necessário para List

public class InfoPanelController : MonoBehaviour
{
    // Padrão Singleton
    public static InfoPanelController Instance { get; private set; }

    [Header("Componentes de UI")]
    [Tooltip("Lista de todos os painéis de informação. O índice [0] corresponde ao ID 1.")]
    public List<GameObject> infoPanels;
    
    // Variável para rastrear o painel ativo atualmente
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
        
        // Garante que todos os painéis estejam desativados no início
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
        // Verifica se o painel está ativo e se a tecla ENTER foi pressionada
        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            HidePanel();
        }
    }

    /// <summary>
    /// Chamado quando uma placa 3D é clicada.
    /// </summary>
    /// <param name="id">O ID da placa clicada (de 1 a N).</param>
    public void ShowPanel(int id)
    {
        // Garante que o ID é válido (maior que 0 e dentro do limite da lista)
        int index = id - 1;
        if (index >= 0 && index < infoPanels.Count && infoPanels[index] != null)
        {
            // Esconde o painel ativo anterior (se houver)
            if (currentActivePanel != null && currentActivePanel != infoPanels[index])
            {
                currentActivePanel.SetActive(false);
            }
            
            // Ativa o novo painel
            currentActivePanel = infoPanels[index];
            currentActivePanel.SetActive(true);
            isPanelActive = true;
            Time.timeScale = 0f; // Pausa o jogo
        }
        else
        {
            Debug.LogWarning($"InfoPanelController: ID de painel inválido ({id}) ou painel não atribuído.");
        }
    }

    /// <summary>
    /// Chamado para fechar o painel (pelo Enter).
    /// </summary>
    public void HidePanel()
    {
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            currentActivePanel = null;
            isPanelActive = false;
            Time.timeScale = 1f; // Despausa o jogo
        }
    }
}