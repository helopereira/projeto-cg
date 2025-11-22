using UnityEngine;

public class SocketDeEncaixe : MonoBehaviour
{
    [Header("Configuração")]
    public string ID_Esperado = "Peca_1";
    
    // Não precisamos mais da variável "PecaFinalConsertada"!
    // O script vai usar a própria peça que está na sua mão.

    private bool jaFoiEncaixado = false;

    void OnMouseDown()
    {
        if (jaFoiEncaixado) return;
        if (SelectedObject.Instance == null) return;

        Transform ferramentaNaMao = SelectedObject.Instance.SelectedTool;

        if (ferramentaNaMao != null)
        {
            PecaDeReposicao pecaScript = ferramentaNaMao.GetComponent<PecaDeReposicao>();

            if (pecaScript != null && pecaScript.ID == ID_Esperado)
            {
                // Passamos o objeto que está na mão para ser fixado
                FixarPecaNoBanco(ferramentaNaMao.gameObject);
            }
            else
            {
                GameProgressManager.Instance?.DisplayMessage("Peça errada!");
            }
        }
    }

    void FixarPecaNoBanco(GameObject peca)
    {
        jaFoiEncaixado = true;

        // 1. Tira a peça do sistema de seleção (Inventário fica vazio)
        SelectedObject.Instance.SetSelectedTool(null);

        // 2. Teleporta a peça para o lugar e rotação exatos do socket
        peca.transform.position = transform.position;
        peca.transform.rotation = transform.rotation;

        // 3. Reativa a peça (ela estava escondida/na mão)
        peca.SetActive(true);

        // 4. "Blinda" a peça: Remove os componentes para ela virar um objeto comum
        // Assim você não consegue clicar nela de novo.
        Destroy(peca.GetComponent<PecaDeReposicao>()); // Remove o script de pegar
        Destroy(peca.GetComponent<InventoryItem>());   // Remove o ícone
        Destroy(peca.GetComponent<BoxCollider>());     // Remove o colisor (opcional, mas bom)
        
        // Se quiser manter colisão física (pro personagem não atravessar), 
        // tire apenas a linha do Destroy(BoxCollider).

        // 5. Avisa o gerente
        if (ReconstrucaoManager.Instance != null)
            ReconstrucaoManager.Instance.ContarEncaixe();

        // 6. Some com o socket transparente
        gameObject.SetActive(false);

        Debug.Log("Peça movida e fixada com sucesso!");
    }
}