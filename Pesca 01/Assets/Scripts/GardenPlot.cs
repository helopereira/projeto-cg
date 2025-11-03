using UnityEngine;
using System; // Necessário para a Action

public class GardenPlot : MonoBehaviour
{
    // Variáveis que serão configuradas no Inspector da Unity
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
    public Material materialEstado4; // NOVO: Material para o Estado 4
    public Material materialEstado5; // NOVO: Material para o Estado 5 (Final)

    [Header("Ferramentas Necessárias (Nomes dos Objetos)")]
    [Tooltip("Ferramenta necessária para ir do Estado 1 -> 2 (e.g., Shovel).")]
    public string ferramentaParaEstado2 = "Shovel";
    [Tooltip("Ferramenta necessária para ir do Estado 2 -> 3 (e.g., SeedBag).")]
    public string ferramentaParaEstado3 = "SeedBag";
    [Tooltip("Ferramenta necessária para ir do Estado 3 -> 4 (e.g., WaterCan).")]
    public string ferramentaParaEstado4 = "WaterCan";
    [Tooltip("Ferramenta necessária para ir do Estado 4 -> 5 (e.g., Fertilizer).")]
    public string ferramentaParaEstado5 = "Fertilizer";

    // O estado final do jardim
    private const int ESTADO_MAXIMO = 5;
    
    // O estado atual do jardim
    private int estadoAtual = 1;

    // Referências aos Singletons
    private GardenManager gardenManager;

    // Propriedade para ser acessada pelo GardenManager para contagem
    public bool IsPhaseComplete => estadoAtual == ESTADO_MAXIMO;

    private void Start()
    {
        // Garante que os Singletons estejam acessíveis
        gardenManager = GardenManager.Instance;
        if (gardenManager == null)
        {
            Debug.LogError("GardenManager não encontrado. Verifique se o script está no GameManager e ativo.");
        }
        
        AtualizarVisualDoEstado();
        // Não precisamos mais registrar o Plot no Manager, pois ele usa FindObjectsOfType no Start.
    }

    // Método chamado quando o usuário clica neste objeto (requer um Collider!)
    private void OnMouseDown()
    {
        // Se o estado já é o máximo, não faz nada
        if (estadoAtual == ESTADO_MAXIMO)
        {
            GameProgressManager.Instance?.DisplayMessage("Esta parcela já está completa!");
            Debug.Log("Jardim já completo.");
            return;
        }

        // 1. Determina qual ferramenta é necessária
        string requiredToolName = GetRequiredToolName(estadoAtual);

        // 2. Checa se o SelectedObject tem a ferramenta correta
        if (SelectedObject.Instance != null && SelectedObject.Instance.IsToolSelected(requiredToolName))
        {
            // Ferramenta correta! Avança o estado.
            estadoAtual++;
            AtualizarVisualDoEstado();
        }
        else
        {
            // FERRAMENTA INCORRETA! EXIBE MENSAGEM GLOBALMENTE.
            string selectedName = SelectedObject.Instance?.SelectedTool != null ? SelectedObject.Instance.SelectedTool.name : "Nenhuma";
            string errorMessage = $"Ferramenta Errada! Requer: {requiredToolName}. Selecionado: {selectedName}.";
            
            GameProgressManager.Instance?.DisplayMessage(errorMessage);
            Debug.LogWarning(errorMessage);
            return; // Sai do método se a ferramenta estiver incorreta
        }
    }

    /// <summary>
    /// Retorna o nome da ferramenta necessária para o próximo estado.
    /// </summary>
    private string GetRequiredToolName(int current)
    {
        // Usamos switch para mapeamento claro de estado para ferramenta
        switch (current)
        {
            case 1: return ferramentaParaEstado2;
            case 2: return ferramentaParaEstado3;
            case 3: return ferramentaParaEstado4;
            case 4: return ferramentaParaEstado5;
            default: return ""; // Não deve acontecer
        }
    }

    // Aplica as configurações visuais com base no estado atual
    private void AtualizarVisualDoEstado()
    {
        // Garantindo que só aplicamos materiais se o Renderer existir
        if (terraRenderer == null) return;

        // Se o estado for o final, notifica o GardenManager
        if (estadoAtual == ESTADO_MAXIMO)
        {
            // Notifica o Gerenciador da Fase que esta parcela está completa
            gardenManager?.RegisterPlotCompletion();
        }
        
        switch (estadoAtual)
        {
            case 1:
                // Estado 1 - Inicial: Grama Alta
                terraRenderer.material = materialEstado1;
                gramaObjeto.SetActive(true);
                floresObjeto.SetActive(false);
                break;

            case 2:
                // Estado 2: Terra arada (Grama removida)
                terraRenderer.material = materialEstado2;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;

            case 3:
                // Estado 3: Sementes plantadas (Material da Terra mudado)
                terraRenderer.material = materialEstado3;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;
                
            case 4:
                // Estado 4: Plantas brotando
                terraRenderer.material = materialEstado4;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;

            case ESTADO_MAXIMO: // Estado 5 - Final
                // Estado 5: Flores em plena floração
                terraRenderer.material = materialEstado5;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(true);
                break;
        }
    }
}
