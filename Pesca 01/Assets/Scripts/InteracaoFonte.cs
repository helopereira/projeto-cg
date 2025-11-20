using UnityEngine;

public class InteracaoFonte : MonoBehaviour
{
    [Header("Configuração")]
    [Tooltip("O nome deve bater com o nome do objeto/produto no inventário")]
    public string nomeDoProdutoLimpeza = "Cloro"; 

    void OnMouseDown()
    {
        // Verifica se o inventário existe
        if (SelectedObject.Instance == null) return;

        // Verifica se a fonte está pronta (se a água suja já apareceu)
        if (!FonteManager.Instance.fonteProntaParaLimpar) 
        {
            GameProgressManager.Instance?.DisplayMessage("A fonte ainda não tem água para limpar.");
            return;
        }

        // Pega o item que está na mão
        Transform itemNaMao = SelectedObject.Instance.SelectedTool;

        if (itemNaMao != null)
        {
            // Verifica se o nome do item contém "Cloro"
            if (itemNaMao.name.Contains(nomeDoProdutoLimpeza))
            {
                // 1. Esvazia a mão do jogador (o ícone some da UI)
                SelectedObject.Instance.SetSelectedTool(null);

                // 2. Destrói o objeto da garrafa (ela deixa de existir no jogo)
                Destroy(itemNaMao.gameObject);

                // 3. Realiza a limpeza da fonte
                FonteManager.Instance.LimparAgua();
            }
            else
            {
                GameProgressManager.Instance?.DisplayMessage("Isso não serve para limpar água!");
            }
        }
        else
        {
             GameProgressManager.Instance?.DisplayMessage("Você precisa do Cloro para limpar isso.");
        }
    }
}