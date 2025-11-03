using UnityEngine;

public class SelectedObject : MonoBehaviour
{
    // Padrão Singleton: Permite que outros scripts acessem o Manager facilmente.
    public static SelectedObject Instance { get; private set; }

    [Header("Objeto Selecionado")]
    [Tooltip("O Transform do objeto que está atualmente selecionado (a ferramenta).")]
    public Transform SelectedTool { get; private set; }

    private void Awake()
    {
        // Implementação do Singleton
        if (Instance == null)
        {
            Instance = this;
            // Opcional: Não destruir na troca de cena, dependendo da sua arquitetura
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Define qual objeto está atualmente selecionado.
    // Este método deve ser chamado pelo script de detecção de clique/outline (OutlineSelection.cs).
    public void SetSelectedTool(Transform tool)
    {
        SelectedTool = tool;
        if (tool != null)
        {
            Debug.Log($"Ferramenta Selecionada Globalmente: {tool.name}");
        }
        else
        {
            Debug.Log("Nenhuma ferramenta selecionada.");
        }
    }
    
    // Método auxiliar para checar a ferramenta por nome
    public bool IsToolSelected(string toolName)
    {
        return SelectedTool != null && SelectedTool.name.Contains(toolName);
    }
}