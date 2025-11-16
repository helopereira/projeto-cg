using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; 
using System.Collections; 

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    [Header("UI Global")]
    public TextMeshProUGUI timeDisplay;
    public TextMeshProUGUI progressDisplay;
    public TextMeshProUGUI messageDisplay;

    [Header("Gestão de Fases")]
    public int totalPhasesCount = 1;
    public float messageDisplayTime = 3.0f;
    
    private float elapsedTime = 0f;
    private int completedPhasesCount = 0;
    private string currentAdditiveScene = "";

    void Awake()
    {
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
        elapsedTime += Time.deltaTime;
        if (timeDisplay != null)
        {
            timeDisplay.text = $"Tempo: {Mathf.FloorToInt(elapsedTime / 60):00}:{Mathf.FloorToInt(elapsedTime % 60):00}";
        }
    }

    /// <summary>
    /// Desativa o controle FPS e carrega o minigame em modo Aditivo.
    /// Chamado por ChangeSceneOnClick.
    /// </summary>
    public void LoadMinigameAdditive(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Nome da cena do minigame está vazio!");
            return;
        }
        
        // 1. DESATIVA o controle FPS e a Câmera FPS (libera o mouse e dá foco à Câmera 2D)
        SetFpsState(false);

        currentAdditiveScene = sceneName;
        // 2. Carrega a cena do minigame
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Descarrega o minigame e reativa o controle FPS.
    /// Chamado pelo script Field.cs ao completar o minigame.
    /// </summary>
    public void ReturnToMainScene()
    {
        if (string.IsNullOrEmpty(currentAdditiveScene))
        {
            Debug.LogError("Nenhuma cena aditiva para descarregar. Reativando FPS.");
            SetFpsState(true); 
            return;
        }

        // 1. Descarrega a cena do minigame
        SceneManager.UnloadSceneAsync(currentAdditiveScene);
        currentAdditiveScene = "";

        // 2. REATIVA o controle FPS e a Câmera FPS (trava o mouse)
        SetFpsState(true);
    }

    /// <summary>
    /// Encontra o script PrimeiraPessoa e define seu estado de ativação.
    /// </summary>
    private void SetFpsState(bool active)
    {
        PrimeiraPessoa fpsController = FindObjectOfType<PrimeiraPessoa>(); 
        
        if (fpsController != null)
        {
            fpsController.SetFpsActive(active);
        }
        else
        {
            Debug.LogWarning("Script PrimeiraPessoa não encontrado! O controle do mouse não será alterado.");
        }
    }

    // ... (Métodos de progresso permanecem inalterados)
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
            StopCoroutine(ShowMessageCoroutine(message)); 
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