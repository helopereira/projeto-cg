using UnityEngine;

public class SocketDeEncaixe : MonoBehaviour
{
    // O ID da peça que este socket espera
    public ReconstrucaoManager GerenciadorDoJogo;
    public string ID_Esperado = "Peca_1"; 
    
    // Referência ao Player
    // Arraste o GameObject do Player para esta variável
    public PlayerInventory InventarioDoPlayer; 

    // Chamado quando o jogador CLICA no local de encaixe
    void OnMouseDown()
    {
        // 1. Verifica se o player está segurando alguma peça
        if (InventarioDoPlayer.PecaNaMao != null)
        {
            // 2. Verifica se a peça na mão é a peça correta para este socket
            if (InventarioDoPlayer.PecaNaMao.ID == ID_Esperado)
            {
                // ** LÓGICA DE ENCAIXE CORRETO **
                
                // Pega a referência à peça (o objeto que sumiu)
                GameObject pecaReal = InventarioDoPlayer.PecaNaMao.gameObject; 

                // Reposiciona a peça onde o socket está
                pecaReal.transform.position = transform.position;
                pecaReal.transform.rotation = transform.rotation;

                // Faz a peça aparecer no lugar certo
                pecaReal.SetActive(true); 
                
                if (GerenciadorDoJogo != null)
            {
                GerenciadorDoJogo.ContarEncaixe();
            }
                
                // Limpa o inventário do jogador
                InventarioDoPlayer.PecaNaMao = null; 
                
                // Desativa o socket para que não seja clicável novamente
                gameObject.SetActive(false); 

                Debug.Log("Peca " + ID_Esperado + " encaixada com sucesso!");
                // PONTO PARA ADICIONAR PONTUAÇÃO FUTURAMENTE
            }
            else
            {
                Debug.Log("Peça errada. Esperava: " + ID_Esperado + ", mas está segurando: " + InventarioDoPlayer.PecaNaMao.ID);
            }
        }
        else
        {
            Debug.Log("Não estou segurando nenhuma peça!");
        }
    }
}