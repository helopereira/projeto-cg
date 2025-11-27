using UnityEngine;
using System.Collections;
using System; 

public class LixoComportamento : MonoBehaviour
{
    public Vector3 posicaoEscondida; 
    public Vector3 posicaoVisivel; 
    
    public float tempoDeAnimacao = 0.2f; 
    public float tempoVisivel = 1.5f;
    
    private PescaManager pescaManager;
    private Collider lixoCollider;
    private bool estaVisivel = false;
    private Coroutine cicloCoroutine;
    
    void Awake()
    {
        lixoCollider = GetComponent<Collider>();
        if (lixoCollider == null)
        {
            enabled = false;
            return;
        }
        
        transform.localPosition = posicaoEscondida;
        lixoCollider.enabled = false;
        gameObject.SetActive(false);
    }

    void Start()
    {
        pescaManager = FindObjectOfType<PescaManager>();
        if (pescaManager == null)
        {
            Debug.LogError("PescaManager não encontrado na cena! A contagem de lixo não funcionará.");
        }
    }

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
        
        if (alvo == posicaoEscondida)
        {
            DesativarLixo(); 
        }
    }
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
        Debug.Log($"Lixo {gameObject.name} destruído.");
        Destroy(gameObject);
}
}