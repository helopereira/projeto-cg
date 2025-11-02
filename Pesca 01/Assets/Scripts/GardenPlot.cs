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

    // Estados do nosso jardim
    private int estadoAtual = 1;
    private const int ESTADO_MAXIMO = 5;

    // Referência ao nosso Manager Global para notificar a mudança de estado
    private GardenManager gardenManager;

    [System.Obsolete]
    private void Start()
    {
        // Encontra o manager da fase no início
        gardenManager = FindObjectOfType<GardenManager>();
        if (gardenManager == null)
        {
            Debug.LogError("GardenManager não encontrado na cena! Certifique-se de ter um.");
        }

        // Garante que o estado inicial esteja configurado corretamente
        AtualizarVisualDoEstado();
        
        // Notifica o Manager que esta parcela de terra existe (para a contagem)
        gardenManager?.RegisterPlot(this);
    }

    // Método chamado quando o usuário clica neste objeto (requer um Collider no objeto!)
    private void OnMouseDown()
    {
        // Só avança o estado se não for o estado final
        if (estadoAtual < ESTADO_MAXIMO)
        {
            estadoAtual++;
            AtualizarVisualDoEstado();
            
            // Notifica o Manager sobre a mudança de estado
            if (estadoAtual == ESTADO_MAXIMO)
            {
                gardenManager?.CheckPhaseCompletion();
            }
        }
    }

    // Aplica as configurações visuais com base no estado atual
    private void AtualizarVisualDoEstado()
    {
        switch (estadoAtual)
        {
            case 1:
                // Estado 1 - Inicial
                terraRenderer.material = materialEstado1;
                gramaObjeto.SetActive(true);
                floresObjeto.SetActive(false);
                break;

            case 2:
                // Estado 2
                terraRenderer.material = materialEstado1;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;

            case 3:
                // Estado 2
                terraRenderer.material = materialEstado2;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;

            case 4:
                // Estado 3 - Final
                terraRenderer.material = materialEstado3;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(false);
                break;

            case 5:
                // Estado 3 - Final
                terraRenderer.material = materialEstado4;
                gramaObjeto.SetActive(false);
                floresObjeto.SetActive(true);
                break;
        }
    }

    // Propriedade para ser acessada pelo GardenManager
    public bool IsPhaseComplete => estadoAtual == ESTADO_MAXIMO;
}