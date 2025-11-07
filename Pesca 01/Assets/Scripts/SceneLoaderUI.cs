using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para gerenciar cenas

/// <summary>
/// Este script contém funções públicas para serem chamadas por
/// botões de UI (Canvas) para carregar cenas ou sair do jogo.
/// Anexe este script ao seu GameManager ou Canvas.
/// </summary>
public class SceneLoaderUI : MonoBehaviour
{
    /// <summary>
    /// Carrega uma cena com base no nome fornecido.
    /// Esta função é chamada pelo evento OnClick() do botão.
    /// </summary>
    /// <param name="sceneName">O nome exato da cena (definido no Inspector do Botão)</param>
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Opcional: Se você estiver a sair de um menu de pausa, 
            // garanta que o tempo volte ao normal.
            Time.timeScale = 1f; 
            
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("O nome da cena está vazio! Verifique o evento OnClick() do botão.");
        }
    }

    /// <summary>
    /// Função bónus para fechar o jogo (geralmente usada no Menu Principal).
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}