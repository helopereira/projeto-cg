using UnityEngine;
using System;

public class SelectedObject : MonoBehaviour
{
    public static SelectedObject Instance { get; private set; }

    public event Action<Transform> OnToolSelectionChanged;

    [Header("Objeto Selecionado")]
    public Transform SelectedTool { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelectedTool(Transform newTool)
    {
        // --- CORREÇÃO AQUI ---
        // Se já existia uma ferramenta na mão E ela é diferente da nova...
        if (SelectedTool != null && SelectedTool != newTool)
        {
            // ...nós "soltamos" a ferramenta antiga (trazemos ela de volta pro jogo)
            // Isso impede que a tábua fique invisível para sempre se você trocar de item.
            SelectedTool.gameObject.SetActive(true);
        }
        // ---------------------

        SelectedTool = newTool;
        
        OnToolSelectionChanged?.Invoke(SelectedTool);
        
        if (newTool != null)
        {
            Debug.Log($"Ferramenta Selecionada: {newTool.name}");
            GameProgressManager.Instance?.DisplayMessage($"Item: {newTool.name}");
        }
        else
        {
            Debug.Log("Mão vazia.");
            GameProgressManager.Instance?.DisplayMessage("");
        }
    }
    
    public bool IsToolSelected(string toolName)
    {
        return SelectedTool != null && SelectedTool.name.Contains(toolName);
    }
}