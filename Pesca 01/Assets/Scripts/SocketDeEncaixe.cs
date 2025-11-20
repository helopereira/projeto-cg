using UnityEngine;

public class SocketDeEncaixe : MonoBehaviour
{
    [Header("Configuração")]
    [Tooltip("O ID deve ser idêntico ao da PecaDeReposicao correspondente.")]
    public string ID_Esperado = "Peca_1"; 
    
    [Tooltip("Opcional se você usar o Singleton no Manager. Pode deixar vazio.")]
    public ReconstrucaoManager GerenciadorDoJogo;

    // Chamado quando o jogador CLICA no local de encaixe (o fantasma transparente)
    void OnMouseDown()
    {
        // Verifica se o sistema de seleção existe
        if (SelectedObject.Instance == null) return;

        // 1. Verifica o que o jogador tem na mão (SelectedTool)
        Transform ferramentaNaMao = SelectedObject.Instance.SelectedTool;

        if (ferramentaNaMao != null)
        {
            // 2. Tenta pegar o script 'PecaDeReposicao' do objeto que está na mão
            PecaDeReposicao pecaScript = ferramentaNaMao.GetComponent<PecaDeReposicao>();

            // 3. Verifica se é uma peça válida E se o ID bate com o esperado
            if (pecaScript != null && pecaScript.ID == ID_Esperado)
            {
                // ** LÓGICA DE ENCAIXE CORRETO **
                RealizarEncaixe(ferramentaNaMao);
            }
            else
            {
                // Feedback de erro
                Debug.Log($"Peça errada. Esperava: {ID_Esperado}, mas está segurando: {(pecaScript ? pecaScript.ID : ferramentaNaMao.name)}");
                
                // Se quiser mostrar na tela:
                GameProgressManager.Instance?.DisplayMessage("Essa peça não encaixa aqui!");
            }
        }
        else
        {
            Debug.Log("Não estou segurando nenhuma peça!");
            GameProgressManager.Instance?.DisplayMessage("Você precisa pegar a peça primeiro.");
        }
    }

    void RealizarEncaixe(Transform pecaReal)
    {
        // Traz a peça para a posição exata deste socket
        pecaReal.position = transform.position;
        pecaReal.rotation = transform.rotation;

        // Faz a peça reaparecer no mundo (ela estava setActive(false) quando foi pega)
        pecaReal.gameObject.SetActive(true);
        
        // Limpa o inventário do jogador e remove o ícone da UI
        SelectedObject.Instance.SetSelectedTool(null);

        // Avisa o Gerenciador (Usa o Singleton se tiver, ou a variável arrastada)
        if (ReconstrucaoManager.Instance != null)
        {
            ReconstrucaoManager.Instance.ContarEncaixe();
        }
        else if (GerenciadorDoJogo != null)
        {
            GerenciadorDoJogo.ContarEncaixe();
        }

        // Desativa este socket (o fantasma some, pois a peça real agora ocupa o lugar)
        gameObject.SetActive(false);

        Debug.Log($"Peça {ID_Esperado} encaixada com sucesso!");
    }
}