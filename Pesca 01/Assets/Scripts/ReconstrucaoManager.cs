using UnityEngine;
// using System.Collections; // Não está sendo usado, pode limpar
// using System.Collections.Generic; 

public class ReconstrucaoManager : MonoBehaviour // Removi ICompletable se você não tiver essa interface no contexto, se tiver, mantenha.
{
    public static ReconstrucaoManager Instance; // Singleton simples para facilitar acesso

    [Header("Status do Minigame")]
    public float tempoTotal = 0f;
    public bool minigameAtivo = false;
    
    [Header("Progresso")]
    public int totalDePecas = 5; 
    private int pecasEncaixadas = 0;

    // Removida a referência ao PlayerInventory antigo
    // public PlayerInventory inventario; 

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Inicia automaticamente para testes
        IniciarMinigame(); 
    }

    void Update()
    {
        if (minigameAtivo)
        {
            tempoTotal += Time.deltaTime;
        }
    }

    public void IniciarMinigame()
    {
        if (!minigameAtivo)
        {
            minigameAtivo = true;
            tempoTotal = 0f;
            pecasEncaixadas = 0; // Reseta contagem ao iniciar
            Debug.Log("Minigame de Reconstrução iniciado!");
        }
    }
    
    // Este método será chamado pelo script do Socket (Destino)
    public void ContarEncaixe()
    {
        if (!minigameAtivo) return;

        pecasEncaixadas++;
        Debug.Log($"Peça Encaixada! Total: {pecasEncaixadas} / {totalDePecas}"); 

        if (pecasEncaixadas >= totalDePecas)
        {
            Debug.Log("Todas as peças foram encaixadas!"); 
            FinalizarMinigame();
        }
    }

    public void FinalizarMinigame()
    {
        Debug.Log("Fase de Encaixe: FINALIZAR MINIGAME INICIADO.");
        minigameAtivo = false;

        string mensagem = $"PARABÉNS! Você reconstruiu o banco em: {tempoTotal:F2} segundos!";
        Debug.Log(mensagem);
        
        // Exibe mensagem na tela se o GameProgressManager existir
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.DisplayMessage(mensagem); // Supondo que tenha esse método
            GameProgressManager.Instance.RegisterGamePhaseCompleted();
            Debug.Log("Fase de Encaixe: Notificação enviada ao GameProgressManager.");
        }
        else
        {
            Debug.LogWarning("GameProgressManager não encontrado, mas a fase terminou.");
        }
        
        // Limpa a seleção (tira a ferramenta da mão se sobrou algo)
        SelectedObject.Instance.SetSelectedTool(null);
    }
}