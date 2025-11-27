using UnityEngine;

public class InteracaoFonte : MonoBehaviour
{
    [Header("Configuração")]
    public string nomeDoProdutoLimpeza = "Cloro"; 

    void OnMouseDown()
    {
        if (SelectedObject.Instance == null) return;

        if (!FonteManager.Instance.fonteProntaParaLimpar) 
        {
            GameProgressManager.Instance?.DisplayMessage("A fonte ainda não tem água para limpar.");
            return;
        }

        Transform itemNaMao = SelectedObject.Instance.SelectedTool;

        if (itemNaMao != null)
        {
            if (itemNaMao.name.Contains(nomeDoProdutoLimpeza))
            {
                SelectedObject.Instance.SetSelectedTool(null);

                Destroy(itemNaMao.gameObject);

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