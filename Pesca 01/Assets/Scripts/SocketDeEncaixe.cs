using UnityEngine;

public class SocketDeEncaixe : MonoBehaviour
{
    [Header("Configuração")]
    public string ID_Esperado = "Peca_1";

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

        SelectedObject.Instance.SetSelectedTool(null);

        peca.transform.position = transform.position;
        peca.transform.rotation = transform.rotation;

        peca.SetActive(true);

        Destroy(peca.GetComponent<PecaDeReposicao>()); 
        Destroy(peca.GetComponent<InventoryItem>()); 
        Destroy(peca.GetComponent<BoxCollider>());     

        if (ReconstrucaoManager.Instance != null)
            ReconstrucaoManager.Instance.ContarEncaixe();


        gameObject.SetActive(false);

    }
}