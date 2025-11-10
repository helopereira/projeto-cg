using UnityEngine;

public class PecaDeReposicao : MonoBehaviour
{
    // Apenas um ID para identificar qual peça é
    public string ID = "Peca_1"; 

    // O GameObject vazio que marca o lugar certo (o Socket)
    // Arraste o seu GameObject "Socket_..." para esta variável no Inspector
    public GameObject Destino; 

    // Referência ao script do Player para saber se ele tem algo na mão
    // Arraste o GameObject do Player para esta variável
    public PlayerInventory InventarioDoPlayer; 

    private Collider meuCollider;

    void Start()
    {
        meuCollider = GetComponent<Collider>();
        // Garante que o Destino (Socket) esteja invisível e sem interação no início
        if (Destino != null)
        {
            Destino.SetActive(false); 
        }
    }

    // Chamado quando o jogador CLICA na tábua
    void OnMouseDown()
    {
        // 1. Verifica se o player já está segurando alguma peça
        if (InventarioDoPlayer.PecaNaMao == null)
        {
            // Pega a peça!
            InventarioDoPlayer.PecaNaMao = this;
            
            // Faz a tábua sumir do mapa
            gameObject.SetActive(false); 
            
            // Ativa o destino para que o jogador possa clicar nele
            if (Destino != null)
            {
                Destino.SetActive(true);
            }
            Debug.Log("Peca " + ID + " coletada.");
        }
        else
        {
            Debug.Log("Já estou segurando a peca: " + InventarioDoPlayer.PecaNaMao.ID);
        }
    }
}