using UnityEngine;
using System; // Necessário para a Action

public class SelectedObject : MonoBehaviour
{
    // Padrão Singleton
    public static SelectedObject Instance { get; private set; }

    // Evento C# que será disparado sempre que a ferramenta selecionada mudar.
    public event Action<Transform> OnToolSelectionChanged;

    [Header("Objeto Selecionado")]
    [Tooltip("O Transform do objeto que está atualmente selecionado (a ferramenta).")]
    public Transform SelectedTool { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Deixando comentado para controle manual
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Define qual objeto está atualmente selecionado.
    public void SetSelectedTool(Transform tool)
    {
        SelectedTool = tool;
        
        // Dispara o evento, notificando todos os ouvintes (incluindo a UI de ToolDisplayUI)
        OnToolSelectionChanged?.Invoke(SelectedTool);
        
        if (tool != null)
        {
            GameProgressManager.Instance?.DisplayMessage($"Ferramenta Selecionada: {tool.name}");
            Debug.Log($"Ferramenta Selecionada Globalmente: {tool.name}");
        }
        else
        {
            GameProgressManager.Instance?.DisplayMessage("Ferramenta Desselecionada.");
            Debug.Log("Nenhuma ferramenta selecionada.");
        }
    }
    
    // Método auxiliar para checar a ferramenta por nome
    public bool IsToolSelected(string toolName)
    {
        return SelectedTool != null && SelectedTool.name.Contains(toolName);
    }
}
