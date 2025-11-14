using UnityEngine;
// using UnityEngine.SceneManagement; // Não precisamos mais disto aqui

/// <summary>
/// Script simples que chama o GameProgressManager para carregar 
/// uma cena de minigame ADITIVAMENTE quando o objeto é clicado.
/// REQUER que o objeto tenha um Collider 3D.
/// </summary>
public class ChangeSceneOnClick : MonoBehaviour
{
    [Header("Configuração da Cena")]
    [Tooltip("O nome EXATO da cena do minigame (deve estar nas Build Settings)")]
    public string sceneName;

    private void OnMouseDown()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("O nome da cena (sceneName) não foi definido no Inspector!");
            return;
        }

        if (GameProgressManager.Instance != null)
        {
            // Chama o gestor global para carregar a cena por cima
            GameProgressManager.Instance.LoadMinigameAdditive(sceneName);
        }
        else
        {
            Debug.LogError("GameProgressManager.Instance não encontrado!");
        }
    }
}