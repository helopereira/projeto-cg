using UnityEngine;

public class Lixeira : MonoBehaviour
{
    public TiposDeLixo tipoAceito;
    public int lixosNecessarios = 2; 
    
    public int lixosJogados = 0; // Agora conta pontos, não apenas objetos
    public bool lixeiraConcluida = false;

    void OnMouseDown()
    {
        if (SelectedObject.Instance == null) return;

        Transform itemNaMao = SelectedObject.Instance.SelectedTool;

        if (itemNaMao != null)
        {
            LixoColetavel lixoScript = itemNaMao.GetComponent<LixoColetavel>();

            if (lixoScript != null)
            {
                if (lixoScript.tipoDesteLixo == tipoAceito)
                {
                    ReciclarItem(itemNaMao.gameObject, lixoScript.quantidadeQueVale);
                }
                else
                {
                    GameProgressManager.Instance?.DisplayMessage($"Aqui só aceita {tipoAceito}!");
                }
            }
        }
    }

    // Recebe quantos pontos o item vale
    void ReciclarItem(GameObject lixoObjeto, int valor)
    {
        Destroy(lixoObjeto);
        SelectedObject.Instance.SetSelectedTool(null);

        // Soma o valor do objeto (ex: se for o monte de latas, soma logo 2 ou 3)
        lixosJogados += valor;
        
        Debug.Log($"{tipoAceito}: {lixosJogados}/{lixosNecessarios}");

        if (lixosJogados >= lixosNecessarios && !lixeiraConcluida)
        {
            lixeiraConcluida = true;
            Debug.Log($"Lixeira de {tipoAceito} finalizada!");
        }

        ReciclagemManager.Instance.VerificarVitoria();
    }
}