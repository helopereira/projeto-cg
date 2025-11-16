using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoaderUI : MonoBehaviour
{
    // Constante para o nome da cena principal/mundo 3D
    // Útil para retornar após o minigame.
    private const string WorldSceneName = "Cena Principal"; 

    /// <summary>
    /// Carrega uma cena com base no nome fornecido (uso geral).
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("O nome da cena está vazio!");
        }
    }

    /// <summary>
    /// NOVO: Desativa o controle FPS e carrega o minigame em modo Aditivo.
    /// </summary>
    /// <param name="Minigame - Iluminação">O nome da cena do minigame 2D.</param>
    public void LoadMinigameScene(string minigameSceneName)
    {
        if (string.IsNullOrEmpty(minigameSceneName))
        {
            Debug.LogError("O nome da cena do minigame está vazio!");
            return;
        }

        // 1. Encontra e DESATIVA o controle FPS (torna o mouse visível/destravado)
        PrimeiraPessoa fpsController = FindObjectOfType<PrimeiraPessoa>(); 
        if (fpsController != null)
        {
            fpsController.SetFpsActive(false); 
        }
        
        // 2. Carrega a cena do minigame (Aditiva para manter o Player 3D e o World ativos)
        SceneManager.LoadScene(minigameSceneName, LoadSceneMode.Additive); 
    }

    /// <summary>
    /// NOVO: Descarrega o minigame e reativa o controle FPS.
    /// </summary>
    /// <param name="Minigame - Iluminação">O nome da cena do minigame 2D a ser descarregada.</param>
    public void CloseMinigame(string minigameSceneName)
    {
        if (string.IsNullOrEmpty(minigameSceneName))
        {
            Debug.LogError("O nome da cena do minigame a ser descarregada está vazio!");
            return;
        }

        // 1. Descarrega a cena do minigame
        SceneManager.UnloadSceneAsync(minigameSceneName);
        
        // 2. Encontra e REATIVA o controle FPS (trava o mouse/esconde)
        PrimeiraPessoa fpsController = FindObjectOfType<PrimeiraPessoa>();
        if (fpsController != null)
        {
            fpsController.SetFpsActive(true);
        }
    }

    /// <summary>
    /// Fecha o jogo (somente em builds).
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Opcional: para parar no editor
        #endif
    }
}