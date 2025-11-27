using UnityEngine;

public class ReconstrucaoManager : MonoBehaviour 
{
    public static ReconstrucaoManager Instance; 

    [Header("Status do Minigame")]
    public float tempoTotal = 0f;
    public bool minigameAtivo = false;
    
    [Header("Progresso")]
    public int totalDePecas = 5; 
    private int pecasEncaixadas = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
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
            pecasEncaixadas = 0;
            Debug.Log("Minigame de Reconstrução iniciado!");
        }
    }
    
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
        
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.DisplayMessage(mensagem); 
            GameProgressManager.Instance.RegisterGamePhaseCompleted();
            Debug.Log("Fase de Encaixe: Notificação enviada ao GameProgressManager.");
        }
        else
        {
            Debug.LogWarning("GameProgressManager não encontrado, mas a fase terminou.");
        }
        SelectedObject.Instance.SetSelectedTool(null);
    }
}