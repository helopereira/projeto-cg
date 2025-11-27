using UnityEngine;
using TMPro; 
using System;

public class ToolDisplayUI : MonoBehaviour
{
    private TextMeshProUGUI toolText;
    
    private const string NO_TOOL_MESSAGE = "Nenhuma Ferramenta Selecionada";

    void Start()
    {
        toolText = GetComponent<TextMeshProUGUI>();
        if (toolText == null)
        {
            Debug.LogError("ToolDisplayUI requer um componente TextMeshProUGUI anexado.");
            enabled = false;
            return;
        }

        if (SelectedObject.Instance != null)
        {

            SelectedObject.Instance.OnToolSelectionChanged += OnSelectionChanged;
            
            OnSelectionChanged(SelectedObject.Instance.SelectedTool);
        }
        else
        {
            toolText.text = "ERRO: SelectedObject não encontrado!";
        }
    }

    private void OnDestroy()
    {
        if (SelectedObject.Instance != null)
        {
            SelectedObject.Instance.OnToolSelectionChanged -= OnSelectionChanged;
        }
    }

    private void OnSelectionChanged(Transform newTool)
    {
        if (newTool != null)
        {
            toolText.text = $"Ferramenta Ativa: {newTool.name}";
            toolText.color = Color.yellow; 
        }
        else
        {
            toolText.text = NO_TOOL_MESSAGE;
            toolText.color = Color.white;
        }
    }
}
