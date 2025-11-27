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

        if (SelectedTool != null && SelectedTool != newTool)
        {

            SelectedTool.gameObject.SetActive(true);
        }

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