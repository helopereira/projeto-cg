using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; 
using System.Collections; 

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    [Header("UI Global")]
    public TextMeshProUGUI timeDisplay;
    public TextMeshProUGUI pontuacaoDisplay;
    public TextMeshProUGUI tempofinalDisplay;
    public TextMeshProUGUI progressDisplay;
    public TextMeshProUGUI messageDisplay;
    public GameObject finalDisplay;

    [Header("Gestão de Fases")]
    public int totalPhasesCount = 1;
    public float messageDisplayTime = 3.0f;
    public string nomeCenaMenu = "Menu"; 

    [Header("Configuração de Áudio")] 
    public AudioClip somDeVitoria;    
    private AudioSource meuAudioSource; 
    
    private float elapsedTime = 0f;
    private int completedPhasesCount = 0;
    private string currentAdditiveScene = "";
    
    private bool isGameCompleted = false;

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
        meuAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RetornarAoMenu();
        }
        if (!isGameCompleted)
        {
            elapsedTime += Time.deltaTime;
            
            if (timeDisplay != null)
            {
                timeDisplay.text = $"Tempo: {Mathf.FloorToInt(elapsedTime / 60):00}:{Mathf.FloorToInt(elapsedTime % 60):00}";
            }
        }
    }


    public void LoadMinigameAdditive(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Nome da cena do minigame está vazio!");
            return;
        }
        SetFpsState(false);
        currentAdditiveScene = sceneName;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void ReturnToMainScene()
    {
        if (string.IsNullOrEmpty(currentAdditiveScene))
        {
            SetFpsState(true); 
            return;
        }
        SceneManager.UnloadSceneAsync(currentAdditiveScene);
        currentAdditiveScene = "";
        SetFpsState(true);
    }

    private void SetFpsState(bool active)
    {
        PrimeiraPessoa fpsController = FindObjectOfType<PrimeiraPessoa>(); 
        if (fpsController != null)
        {
            fpsController.SetFpsActive(active);
        }
    }

    // --- LÓGICA DE PROGRESSO E VITÓRIA ---

    public void RegisterGamePhaseCompleted()
    {
        // Só conta se ainda não acabou tudo
        if (completedPhasesCount < totalPhasesCount)
        {
            completedPhasesCount++;
            UpdateProgressDisplay();

            // Toca o som de vitória/avanço
            if (meuAudioSource != null && somDeVitoria != null)
            {
                meuAudioSource.PlayOneShot(somDeVitoria);
            }

            // Verifica se foi a ÚLTIMA fase
            if (completedPhasesCount >= totalPhasesCount)
            {
                FinalizarJogo();
            }
            else
            {
                DisplayMessage($"Fase {completedPhasesCount} / {totalPhasesCount} Concluída!");
            }
        }
    }

    void FinalizarJogo()
    {
        isGameCompleted = true; 
        SetFpsState(false);

        float tempoFinal = Mathf.Max(elapsedTime, 1f);
        int pontuacao = Mathf.RoundToInt(1000000f / tempoFinal);

        if (timeDisplay != null)
        {
            timeDisplay.text = $"Pontuação: {pontuacao}";
            timeDisplay.color = Color.yellow; 
        }
        pontuacaoDisplay.text = $"{pontuacao}";
        tempofinalDisplay.text = $"{Mathf.FloorToInt(elapsedTime / 60):00}:{Mathf.FloorToInt(elapsedTime % 60):00}";
        finalDisplay.gameObject.SetActive(true);
    }

    public void RetornarAoMenu()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Destroy(gameObject); 

        SceneManager.LoadScene(nomeCenaMenu);
    }

    public void VoltarMapa()
    {
        SetFpsState(true);
        finalDisplay.gameObject.SetActive(false);
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