using UnityEngine;

public class Lixeira : MonoBehaviour
{
    [Header("Configuração da Lixeira")]
    public TiposDeLixo tipoAceito; 
    public int lixosNecessarios = 2; 
    
    [Header("Estado Atual")]
    public int lixosJogados = 0;
    public bool lixeiraConcluida = false;
    private Animator meuAnimator;

    void Start()
    {
        meuAnimator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (SelectedObject.Instance == null) return;
        
        if (lixeiraConcluida) 
        {
            GameProgressManager.Instance?.DisplayMessage("Esta lixeira já está cheia!");
            return;
        }

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

    void ReciclarItem(GameObject lixoObjeto, int valor)
    {
        Destroy(lixoObjeto);
        SelectedObject.Instance.SetSelectedTool(null);

        lixosJogados += valor;
        Debug.Log($"{tipoAceito}: {lixosJogados}/{lixosNecessarios}");

        if (lixosJogados >= lixosNecessarios && !lixeiraConcluida)
        {
            lixeiraConcluida = true;
            Debug.Log($"Lixeira de {tipoAceito} cheia! Fechando tampa...");

            if (meuAnimator != null)
            {
                meuAnimator.SetTrigger("Fechar");
            }
        }

        ReciclagemManager.Instance.VerificarVitoria();
    }
}