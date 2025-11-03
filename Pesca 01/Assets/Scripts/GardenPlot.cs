using UnityEngine;

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

    [Header("Materiais/Texturas para os Estados")]
    [Tooltip("Material para o Estado 1 (Inicial).")]
    public Material materialEstado1;
    [Tooltip("Material para o Estado 2.")]
    public Material materialEstado2;
    [Tooltip("Material para o Estado 3.")]
    public Material materialEstado3;
    [Tooltip("Material para o Estado 4 (Final).")]
    public Material materialEstado4;

    [Header("Ferramentas Necessárias")]
    [Tooltip("Nome da ferramenta necessária para ir do Estado 1 -> 2.")]
    public string ferramentaParaEstado2 = "Ferramenta 1";
    [Tooltip("Nome da ferramenta necessária para ir do Estado 2 -> 3.")]
    public string ferramentaParaEstado3 = "Ferramenta 2";
    [Tooltip("Nome da ferramenta necessária para ir do Estado 3 -> 4.")]
    public string ferramentaParaEstado4 = "Ferramenta 3";
    [Tooltip("Nome da ferramenta necessária para ir do Estado 4 -> 5.")]
    public string ferramentaParaEstado5 = "Ferramenta 4";

    // Estados do nosso jardim
    private int estadoAtual = 1;
    private const int ESTADO_MAXIMO = 5;

    // Referência ao nosso Manager Global para notificar a mudança de estado
    private GardenManager gardenManager;

    [System.Obsolete]
    private void Start()
    {
        // Obtém a instância do GardenManager
        gardenManager = FindObjectOfType<GardenManager>();
        if (gardenManager == null)
        {
            Debug.LogError("GardenManager não encontrado na cena.");
        }
        // Verifica se o SelectedObject está presente (requerido para a lógica)
        if (SelectedObject.Instance == null)
        {
            Debug.LogError("SelectedObject não encontrado. Certifique-se de que o script está anexado ao Game Manager.");
        }

        AtualizarVisualDoEstado();
        gardenManager?.RegisterPlot(this);
    }

    // Método chamado quando o usuário clica neste objeto (requer um Collider no objeto!)
    // Método chamado quando o usuário clica neste objeto (requer um Collider no objeto!)
    // Método chamado quando o usuário clica neste objeto (requer um Collider no objeto!)
    private void OnMouseDown()
    {
        // Se o estado já é o máximo, não faz nada
        if (estadoAtual == ESTADO_MAXIMO)
        {
            Debug.Log("Jardim já completo.");
            return;
        }

        string requiredToolName = "";
        if (estadoAtual == 1)
            requiredToolName = ferramentaParaEstado2;
        else if (estadoAtual == 2)
            requiredToolName = ferramentaParaEstado3;
        else if (estadoAtual == 3) // NOVO: Ferramenta para Estado 4
            requiredToolName = ferramentaParaEstado4;
        else if (estadoAtual == 4) // NOVO: Ferramenta para Estado 5
            requiredToolName = ferramentaParaEstado5;
        
        // CORREÇÃO: Verifica se a ferramenta está incorreta ou ausente.
        // Se a verificação falhar, registra o aviso E SAI IMEDIATAMENTE.
        if (SelectedObject.Instance == null || !SelectedObject.Instance.IsToolSelected(requiredToolName))
        {
            // Ferramenta incorreta ou nenhuma ferramenta selecionada
            string currentToolName = SelectedObject.Instance?.SelectedTool?.name ?? "NENHUMA FERRAMENTA";
            
            Debug.LogWarning($"Ferramenta Errada. Você precisa da ferramenta '{requiredToolName}'. Ferramenta atual: {currentToolName}.");
            return; // SAÍDA ANTECIPADA: Impede que o código de avanço de estado seja executado.
        }
        
        // Se chegarmos aqui, a ferramenta está CORRETA.
        // Avança o estado.
        estadoAtual++;
        AtualizarVisualDoEstado();
        
        // Verifica a conclusão da fase após a mudança para o estado 3
        if (estadoAtual == ESTADO_MAXIMO)
        {
            // Chama CheckPhaseCompletion se o manager foi encontrado
            gardenManager?.CheckPhaseCompletion();
        }
    }

    // Aplica as configurações visuais com base no estado atual
    private void AtualizarVisualDoEstado()
    {
        switch (estadoAtual)
        {
            case 1:
                terraRenderer.material = materialEstado1;
                gramaObjeto.SetActive(true);
                floresObjeto.SetActive(false);
                break;

            case 2:
                terraRenderer.material = materialEstado1;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;

            case 3:
                terraRenderer.material = materialEstado2;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;

            case 4:
                terraRenderer.material = materialEstado3;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;

            case 5:
                terraRenderer.material = materialEstado4;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(true);
                break;
        }
    }

    // Propriedade para ser acessada pelo GardenManager
    public bool IsPhaseComplete => estadoAtual == ESTADO_MAXIMO;
}