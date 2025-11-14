using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para gerir cenas
using TMPro; // Para o texto da UI
using System.Collections; // Para Coroutines

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    [Header("UI Global")]
    public TextMeshProUGUI timeDisplay;
    public TextMeshProUGUI progressDisplay;
    public TextMeshProUGUI messageDisplay;

    [Header("Gestão de Fases")]
    [Tooltip("Quantas fases o jogo tem no total")]
    public int totalPhasesCount = 1;
    [Tooltip("O tempo, em segundos, que as mensagens de debug aparecem")]
    public float messageDisplayTime = 3.0f;
    
    private float elapsedTime = 0f;
    private int completedPhasesCount = 0;
    private string currentAdditiveScene = ""; // ADICIONADO: Rastreia a cena do minigame

    void Awake()
    {
        // Configuração do Singleton Persistente
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Atualiza o tempo global
        elapsedTime += Time.deltaTime;
        if (timeDisplay != null)
        {
            timeDisplay.text = $"Tempo: {Mathf.FloorToInt(elapsedTime / 60):00}:{Mathf.FloorToInt(elapsedTime % 60):00}";
        }
    }

    // --- MÉTODOS DE GESTÃO DE CENAS ADITIVAS (NOVO) ---

    /// <summary>
    /// Chamado por um objeto na cena principal (ex: ChangeSceneOnClick.cs)
    /// </summary>
    public void LoadMinigameAdditive(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Nome da cena do minigame está vazio!");
            return;
        }

        currentAdditiveScene = sceneName;
        // Carrega a cena "por cima" da cena atual
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        // Opcional: Desativar a cena principal (esconder o jardim)
        // (Requer que todos os objetos do jardim estejam dentro de um objeto pai)
        // GameObject.Find("Garden_Scene_Root").SetActive(false);
    }

    /// <summary>
    /// Chamado pelo script do minigame (ex: Field.cs) quando este termina
    /// </summary>
    public void ReturnToMainScene()
    {
        if (string.IsNullOrEmpty(currentAdditiveScene))
        {
            Debug.LogError("Nenhuma cena aditiva para descarregar.");
            return;
        }

        // Descarrega a cena do minigame
        SceneManager.UnloadSceneAsync(currentAdditiveScene);
        currentAdditiveScene = "";

        // Opcional: Reativar a cena principal
        // GameObject.Find("Garden_Scene_Root").SetActive(true);

        // Mostra o cursor (se o minigame o escondeu)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    // --- MÉTODOS DE GESTÃO DE PROGRESSO (Existentes) ---

    /// <summary>
    /// Chamado pelo Field.cs (ou outros) quando uma fase é ganha
    /// </summary>
    public void RegisterGamePhaseCompleted()
    {
        if (completedPhasesCount < totalPhasesCount)
        {
            completedPhasesCount++;
            UpdateProgressDisplay();
            DisplayMessage($"Fase {completedPhasesCount} / {totalPhasesCount} Concluída!");
        }
    }

    void UpdateProgressDisplay()
    {
        if (progressDisplay != null)
        {
            progressDisplay.text = $"Progresso: {completedPhasesCount} / {totalPhasesCount}";
        }
    }

    public void DisplayMessage(string message)
    {
        if (messageDisplay != null)
        {
            StopCoroutine(ShowMessageCoroutine(message)); // Para a coroutine anterior
            StartCoroutine(ShowMessageCoroutine(message));
        }
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        messageDisplay.text = message;
        messageDisplay.gameObject.SetActive(true);
        yield return new WaitForSeconds(messageDisplayTime);
        messageDisplay.gameObject.SetActive(false);
    }
}