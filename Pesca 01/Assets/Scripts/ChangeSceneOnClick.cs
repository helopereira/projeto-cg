using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para gerenciar cenas



/// <summary>
/// Script simples que carrega uma nova cena quando o objeto é clicado.
/// REQUER que o objeto tenha um Collider (3D) ou Collider2D (2D).
/// </summary>



public class ChangeSceneOnClick : MonoBehaviour
{

    [Header("Configuração da Cena")]
    [Tooltip("O nome EXATO da cena para carregar (deve estar nas Build Settings)")]
    public string sceneName;
    

    // OnMouseDown é chamado automaticamente pela Unity quando o mouse
    // clica sobre qualquer Collider (2D ou 3D) neste objeto.
    private void OnMouseDown()
    {
        // Verifica se o nome da cena foi preenchido no Inspector
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Carrega a cena
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("O nome da cena (sceneName) não foi definido no Inspector deste objeto!");
        }
    }
}