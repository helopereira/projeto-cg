using UnityEngine;
public class ChangeSceneOnClick : MonoBehaviour
{
    [Header("Configuração da Cena")]
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
            GameProgressManager.Instance.LoadMinigameAdditive(sceneName);
        }
        else
        {
            Debug.LogError("GameProgressManager.Instance não encontrado!");
        }
    }
}