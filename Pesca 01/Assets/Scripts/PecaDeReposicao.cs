using UnityEngine;

[RequireComponent(typeof(InventoryItem))]
public class PecaDeReposicao : MonoBehaviour
{
    [Header("Identificação")]
    public string ID = "Peca_1"; 

    [Header("Configuração")]
    [Tooltip("O GameObject transparente/fantasma onde esta peça deve ser encaixada.")]
    public GameObject Destino; 

    private void Start()
    {
        // Garante que o destino comece escondido
        if (Destino != null) Destino.SetActive(false);
    }

    // Essa função roda automaticamente quando o objeto reaparece na cena (quando você troca de item)
    private void OnEnable()
    {
        // Se a tábua caiu no chão (reapareceu), escondemos o fantasma no banco
        // para não confundir o jogador.
        if (Destino != null)
        {
            Destino.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (SelectedObject.Instance == null) return;

        // --- MUDANÇA PRINCIPAL AQUI ---
        // Removemos o 'if (SelectedTool == null)'
        // Agora, se você clicar na tábua, você pega ela, não importa o que tem na mão.
        // O SelectedObject.Instance.SetSelectedTool já cuida de soltar o item anterior.

        // 1. Define esta tábua como a ferramenta atual
        SelectedObject.Instance.SetSelectedTool(this.transform);
        
        // 2. Esconde a tábua da cena (vai para o inventário)
        gameObject.SetActive(false); 

        // 3. Mostra o fantasma no banco para saber onde encaixar
        if (Destino != null)
        {
            Destino.SetActive(true);
        }

        Debug.Log($"Peça {ID} coletada.");
    }
}