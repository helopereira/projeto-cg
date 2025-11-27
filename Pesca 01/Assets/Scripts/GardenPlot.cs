using UnityEngine;
using System; 

public class GardenPlot : MonoBehaviour
{
    [Header("Componentes")]
    [Tooltip("Renderizador do objeto principal (Terra) para trocar o material.")]
    public Renderer terraRenderer;
    [Tooltip("Objeto da Grama que será ligado/desligado.")]
    public GameObject gramaObjeto;
    [Tooltip("Objeto das Flores que será ligado/desligado.")]
    public GameObject floresObjeto;

    [Header("Materiais para os Estados")]
    public Material materialEstado1;
    public Material materialEstado2;
    public Material materialEstado3;
    public Material materialEstado4; 
    public Material materialEstado5; 

    [Header("Ferramentas Necessárias (Nomes dos Objetos)")]
    [Tooltip("Ferramenta necessária para ir do Estado 1 -> 2 (e.g., Shovel).")]
    public string ferramentaParaEstado2 = "Shovel";
    [Tooltip("Ferramenta necessária para ir do Estado 2 -> 3 (e.g., SeedBag).")]
    public string ferramentaParaEstado3 = "SeedBag";
    [Tooltip("Ferramenta necessária para ir do Estado 3 -> 4 (e.g., WaterCan).")]
    public string ferramentaParaEstado4 = "WaterCan";
    [Tooltip("Ferramenta necessária para ir do Estado 4 -> 5 (e.g., Fertilizer).")]
    public string ferramentaParaEstado5 = "Fertilizer";
    private const int ESTADO_MAXIMO = 5;
    
    private int estadoAtual = 1;

    private GardenManager gardenManager;

    public bool IsPhaseComplete => estadoAtual == ESTADO_MAXIMO;

    private void Start()
    {
        gardenManager = GardenManager.Instance;
        if (gardenManager == null)
        {
            Debug.LogError("GardenManager não encontrado. Verifique se o script está no GameManager e ativo.");
        }
        
        AtualizarVisualDoEstado();
    }

    private void OnMouseDown()
    {
        // Se o estado já é o máximo, não faz nada
        if (estadoAtual == ESTADO_MAXIMO)
        {
            GameProgressManager.Instance?.DisplayMessage("Esta parcela já está completa!");
            return;
        }

        string requiredToolName = GetRequiredToolName(estadoAtual);

        if (SelectedObject.Instance != null && SelectedObject.Instance.IsToolSelected(requiredToolName))
        {
            estadoAtual++;
            AtualizarVisualDoEstado();
        }
        else
        {
            string selectedName = SelectedObject.Instance?.SelectedTool != null ? SelectedObject.Instance.SelectedTool.name : "Nenhuma";
            string errorMessage = $"Ferramenta Errada! Requer: {requiredToolName}. Selecionado: {selectedName}.";
            
            GameProgressManager.Instance?.DisplayMessage(errorMessage);
            Debug.LogWarning(errorMessage);
            return; 
        }
    }

    private string GetRequiredToolName(int current)
    {

        switch (current)
        {
            case 1: return ferramentaParaEstado2;
            case 2: return ferramentaParaEstado3;
            case 3: return ferramentaParaEstado4;
            case 4: return ferramentaParaEstado5;
            default: return ""; 
        }
    }

    private void AtualizarVisualDoEstado()
    {
        if (terraRenderer == null) return;

        if (estadoAtual == ESTADO_MAXIMO)
        {
            gardenManager?.RegisterPlotCompletion();
        }
        
        switch (estadoAtual)
        {
            case 1:
                terraRenderer.material = materialEstado1;
                gramaObjeto.SetActive(true);
                floresObjeto.SetActive(false);
                break;

            case 2:
                terraRenderer.material = materialEstado2;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;

            case 3:
                terraRenderer.material = materialEstado3;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;
                
            case 4:
                terraRenderer.material = materialEstado4;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;

            case ESTADO_MAXIMO:
                terraRenderer.material = materialEstado5;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(true);
                break;
        }
    }
}
