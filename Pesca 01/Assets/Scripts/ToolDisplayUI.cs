using UnityEngine;
using TMPro; // Necessário para usar TextMeshPro
using System; // Importação necessária para o evento

/// <summary>
/// Este script é anexado a um componente TextMeshProUGUI no Canvas
/// para exibir o nome da ferramenta atualmente selecionada.
/// </summary>
public class ToolDisplayUI : MonoBehaviour
{
    private TextMeshProUGUI toolText;
    
    // Texto que aparece quando não há nada selecionado
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

        // 1. Garante que o SelectedObject existe
        if (SelectedObject.Instance != null)
        {
            // 2. Inscreve-se no evento para ser notificado sobre mudanças
            // Esta é a chave da comunicação global -> UI!
            SelectedObject.Instance.OnToolSelectionChanged += OnSelectionChanged;
            
            // 3. Atualiza o display imediatamente com o estado atual
            OnSelectionChanged(SelectedObject.Instance.SelectedTool);
        }
        else
        {
            toolText.text = "ERRO: SelectedObject não encontrado!";
        }
    }

    private void OnDestroy()
    {
        // Desinscreve-se para evitar erros quando o objeto UI for destruído
        if (SelectedObject.Instance != null)
        {
            SelectedObject.Instance.OnToolSelectionChanged -= OnSelectionChanged;
        }
    }

    /// <summary>
    /// Chamado pelo evento OnToolSelectionChanged sempre que a ferramenta muda.
    /// </summary>
    /// <param name="newTool">O Transform da nova ferramenta selecionada, ou null.</param>
    private void OnSelectionChanged(Transform newTool)
    {
        if (newTool != null)
        {
            // Retorna o nome do objeto no Canvas
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
