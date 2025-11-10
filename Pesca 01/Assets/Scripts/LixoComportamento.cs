using UnityEngine;
using System.Collections;
using System; // Para o atributo Obsolete

public class LixoComportamento : MonoBehaviour
{
    // Posições e Configurações
    public Vector3 posicaoEscondida; 
    public Vector3 posicaoVisivel; 
    
    public float tempoDeAnimacao = 0.2f; 
    public float tempoVisivel = 1.5f;
    
    // Referências
    private PescaManager pescaManager;
    private Collider lixoCollider;
    private bool estaVisivel = false;
    private Coroutine cicloCoroutine;
    
    // --- FUNÇÕES DE INICIALIZAÇÃO ---

    void Awake()
    {
        lixoCollider = GetComponent<Collider>();
        if (lixoCollider == null)
        {
            Debug.LogError("O Prefab do Lixo deve ter um Collider 3D!");
            enabled = false;
            return;
        }
        
        // Configurações iniciais
        transform.localPosition = posicaoEscondida;
        lixoCollider.enabled = false;
        gameObject.SetActive(false);
    }

    // Usando FindObjectOfType que é a forma atual, e removendo o atributo Obsolete
    void Start()
    {
        pescaManager = FindObjectOfType<PescaManager>();
        if (pescaManager == null)
        {
            Debug.LogError("PescaManager não encontrado na cena! A contagem de lixo não funcionará.");
        }
    }
    
    // --- LÓGICA DE ATIVAÇÃO E CICLO ---

    public void AtivarLixo()
    {
        if (lixoCollider == null) return;
        gameObject.SetActive(true);
        estaVisivel = false;
        lixoCollider.enabled = true; 
        cicloCoroutine = StartCoroutine(CicloDeVisibilidade());
    }

    IEnumerator CicloDeVisibilidade()
    {
        yield return StartCoroutine(MoverLixo(posicaoVisivel));
        estaVisivel = true;
        
        yield return new WaitForSeconds(tempoVisivel);
        
        // 3. Desce (se não foi clicado)
        if (estaVisivel) 
        {
            yield return StartCoroutine(MoverLixo(posicaoEscondida));
            DesativarLixo();
        }
        
    }

    IEnumerator MoverLixo(Vector3 alvo)
    {
        float t = 0;
        Vector3 inicio = transform.localPosition;

        while (t < 1)
        {
            t += Time.deltaTime / tempoDeAnimacao;
            transform.localPosition = Vector3.Lerp(inicio, alvo, t);
            yield return null;
        }
        transform.localPosition = alvo;
        
        // ⭐️ CHAMA DESATIVAR APENAS SE ESTIVER DESCENDO
        if (alvo == posicaoEscondida)
        {
            DesativarLixo(); 
        }
    }

    // --- LÓGICA DE INTERAÇÃO ---
    
    void OnMouseDown()
    {
        if (estaVisivel)
        {
            if (cicloCoroutine != null)
            {
                StopCoroutine(cicloCoroutine); 
            }
            estaVisivel = false; 
            lixoCollider.enabled = false; 
            pescaManager?.RegistrarLixoRemovido();
            StartCoroutine(MoverLixo(posicaoEscondida));
        }
    }
    void DesativarLixo()
{
    
    
    if (lixoCollider != null) 
        {
            lixoCollider.enabled = false;
        }
        
        // Usa o Log para rastrear
        Debug.Log($"Lixo {gameObject.name} destruído.");
        Destroy(gameObject);
}
}