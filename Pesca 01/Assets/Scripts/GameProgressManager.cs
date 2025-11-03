using UnityEngine;
using TMPro; // Necessário para usar TextMeshPro
using System.Collections;
using System.Collections.Generic;

public class GameProgressManager : MonoBehaviour
{
    // Padrão Singleton
    public static GameProgressManager Instance { get; private set; }

    [Header("Exibição de UI de Progresso")]
    [Tooltip("Objeto de texto para exibir o tempo decorrido.")]
    public TextMeshProUGUI timeDisplay;
    [Tooltip("Objeto de texto para exibir Fases Concluídas / Fases Totais.")]
    public TextMeshProUGUI progressDisplay;
    
    [Header("Configuração de Mensagens de Jogo")]
    [Tooltip("Objeto de texto para exibir mensagens temporárias (Ex: 'Fase Completa', 'Ferramenta Errada').")]
    public TextMeshProUGUI messageDisplay;
    [Tooltip("Quantidade de segundos que a mensagem temporária deve aparecer.")]
    public float messageDisplayDuration = 3.0f; 
    
    // Variáveis de progresso
    [Header("Progresso do Jogo")]
    public int totalPhasesCount = 1; 
    private int completedPhasesCount = 0;
    private float gameTime = 0f;
    private Coroutine messageCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Assumimos que este é o Manager principal e deve ser persistente.
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Inicializa o display
        if (messageDisplay != null)
        {
            messageDisplay.gameObject.SetActive(false);
        }
        UpdateProgressUI();
    }

    private void Update()
    {
        // Contabilização do tempo
        gameTime += Time.deltaTime;
        
        if (timeDisplay != null)
        {
            // Formato simples (Minutos:Segundos)
            timeDisplay.text = string.Format("Tempo: {0:00}:{1:00}", Mathf.FloorToInt(gameTime / 60), Mathf.FloorToInt(gameTime % 60));
        }
    }

    // --- MÉTODOS PÚBLICOS DE MENSAGEM ---

    /// <summary>
    /// Exibe uma mensagem na tela por um período configurável.
    /// </summary>
    public void DisplayMessage(string message)
    {
        if (messageDisplay == null) return;

        // Se uma mensagem antiga estiver sendo exibida, a para e começa a nova.
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }
        
        messageCoroutine = StartCoroutine(ShowMessageForDuration(message));
    }

    private IEnumerator ShowMessageForDuration(string message)
    {
        messageDisplay.text = message;
        messageDisplay.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(messageDisplayDuration);

        // Desativa a mensagem após o tempo
        messageDisplay.gameObject.SetActive(false);
        messageDisplay.text = "";
    }

    // --- MÉTODOS PÚBLICOS DE PROGRESO ---
    
    /// <summary>
    /// Chamado pelo GardenManager quando a fase específica do Jardim é concluída.
    /// </summary>
    public void RegisterGamePhaseCompleted()
    {
        completedPhasesCount++;
        UpdateProgressUI();
        
        DisplayMessage("Fase Concluída! Avance para o próximo desafio.");

        if (completedPhasesCount >= totalPhasesCount)
        {
            DisplayMessage("PARABÉNS! Jogo 100% Completo!");
            // Adicionar lógica de fim de jogo (Time.timeScale = 0f, etc.)
        }
    }
    
    private void UpdateProgressUI()
    {
        if (progressDisplay != null)
        {
            progressDisplay.text = $"Fases: {completedPhasesCount} / {totalPhasesCount}";
        }
    }
}
