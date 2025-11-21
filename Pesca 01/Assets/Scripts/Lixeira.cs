using UnityEngine;

public class Lixeira : MonoBehaviour
{
    [Header("Configuração da Lixeira")]
    public TiposDeLixo tipoAceito; 
    public int lixosNecessarios = 2; 
    
    [Header("Estado Atual")]
    public int lixosJogados = 0;
    public bool lixeiraConcluida = false;

    // Referência para o sistema de animação
    private Animator meuAnimator;

    void Start()
    {
        // Tenta achar o Animator automaticamente no objeto
        meuAnimator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (SelectedObject.Instance == null) return;
        
        // Se a lixeira já fechou, não aceita mais nada
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
                    // Manda reciclar
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

        // VERIFICA SE ENCHEU
        if (lixosJogados >= lixosNecessarios && !lixeiraConcluida)
        {
            lixeiraConcluida = true;
            Debug.Log($"Lixeira de {tipoAceito} cheia! Fechando tampa...");

            // --- ATIVA A ANIMAÇÃO DE FECHAR ---
            if (meuAnimator != null)
            {
                meuAnimator.SetTrigger("Fechar");
            }
        }

        ReciclagemManager.Instance.VerificarVitoria();
    }
}