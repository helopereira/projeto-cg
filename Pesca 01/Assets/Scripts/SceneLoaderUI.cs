using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Collections;

public class SceneLoaderUI : MonoBehaviour
{

    private const string WorldSceneName = "Cena Principal"; 

    private const string MinigameSceneName = "Minigame - Iluminação";
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

    public void LoadMinigameScene(string minigameSceneName)
    {
        if (string.IsNullOrEmpty(minigameSceneName))
        {
            Debug.LogError("O nome da cena do minigame está vazio!");
            return;
        }

        PrimeiraPessoa fpsController = FindObjectOfType<PrimeiraPessoa>(); 
        if (fpsController != null)
        {
            fpsController.SetFpsActive(false); 
        }
        

        SceneManager.LoadScene(minigameSceneName, LoadSceneMode.Additive); 
    }

    public void CloseMinigame(string minigameSceneName)
    {
        if (string.IsNullOrEmpty(minigameSceneName))
        {
            Debug.LogError("O nome da cena do minigame a ser descarregada está vazio!");
            return;
        }

        SceneManager.UnloadSceneAsync(minigameSceneName);
        
        PrimeiraPessoa fpsController = FindObjectOfType<PrimeiraPessoa>();
        if (fpsController != null)
        {
            fpsController.SetFpsActive(true);
        }
    }

    private IEnumerator ReloadMinigameCoroutine()
    {
        AsyncOperation unload = SceneManager.UnloadSceneAsync(MinigameSceneName);

        while (unload != null && !unload.isDone)
        {
            yield return null;
        }

        SceneManager.LoadScene(MinigameSceneName, LoadSceneMode.Additive);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

       public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
        #endif
    }
}