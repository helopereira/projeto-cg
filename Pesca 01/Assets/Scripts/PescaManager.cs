using UnityEngine;
using System.Collections;

public class PescaManager : MonoBehaviour, ICompletable
{
    public GameObject lixoPrefab;     
    public Transform[] spawnPoints;
    public float tempoEntreSpawns = 1.0f;
    private int ultimoPontoIndex = -1;
    
    public int totalLixosParaRemover = 10;
    private int lixosRemovidos = 0;
    private bool minigameCompleto = false;

    [Header("Troca de Cenário")]
    public GameObject cenarioSujo;
    public GameObject cenarioLimpo;
    
    [Header("Efeito de Transição")]
public ParticleSystem transicaoParticulasSystem; 

    void Start()
    {
        if (cenarioSujo != null)
        {
            cenarioSujo.SetActive(true);
        }
        if (cenarioLimpo != null)
        {
            cenarioLimpo.SetActive(false);
        }
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (minigameCompleto == false)
        {
            yield return new WaitForSeconds(tempoEntreSpawns);
            int novoPontoIndex = ultimoPontoIndex;
            while (novoPontoIndex == ultimoPontoIndex)
            {
                novoPontoIndex = Random.Range(0, spawnPoints.Length);
            }
            ultimoPontoIndex = novoPontoIndex;
            Transform spawnLoc = spawnPoints[novoPontoIndex];

            GameObject novoLixoObj = Instantiate(lixoPrefab, spawnLoc.position, Quaternion.identity, spawnLoc);

            LixoComportamento lixoComportamento = novoLixoObj.GetComponent<LixoComportamento>();
            lixoComportamento.posicaoEscondida = Vector3.zero;
            lixoComportamento.posicaoVisivel = new Vector3(0, 0.5f, 0);
            lixoComportamento.AtivarLixo();
        }
        Debug.Log("SpawnRoutine encerrada. Fim do Minigame de Pesca.");
    }


public void RegistrarLixoRemovido()
{
    if (minigameCompleto) return; 
    
    lixosRemovidos++;
    
    if (lixosRemovidos >= totalLixosParaRemover)
    {
        minigameCompleto = true;
                LixoComportamento[] lixosRestantes = FindObjectsByType<LixoComportamento>(FindObjectsSortMode.None);
        foreach (var lixo in lixosRestantes) 
        { 
            Destroy(lixo.gameObject); 
        }
        
        TrocarCenarioParaLimpo(); 
        GameProgressManager.Instance?.RegisterGamePhaseCompleted();
    }
}
    

private void TrocarCenarioParaLimpo()
{
    StartCoroutine(IniciarTransicaoComFX());
}

IEnumerator IniciarTransicaoComFX()
{
    
    if (transicaoParticulasSystem != null)
    {
        // Garante que o objeto pai está ativo
        transicaoParticulasSystem.gameObject.SetActive(true); 
        transicaoParticulasSystem.Play();
        Debug.Log("2. FX DISPARADO. Aguardando 0.1s para a visualização.");
    }
    else
    {
        Debug.LogError("ERRO: O componente ParticleSystem não está atribuído no Inspector!");
    }
    yield return new WaitForSeconds(0.1f); 
    if (cenarioSujo != null)
    {
        cenarioSujo.SetActive(false);
        Debug.Log("3. Cenário Sujo Desativado.");
    }
    
    if (cenarioLimpo != null)
    {
        cenarioLimpo.SetActive(true);
        Debug.Log("4. Cenário Limpo Ativado.");
    }
    if (transicaoParticulasSystem != null)
    {
        float fxDuration = transicaoParticulasSystem.main.duration;
        Debug.Log($"5. Aguardando {fxDuration:F2} segundos para o FX terminar.");
        yield return new WaitForSeconds(fxDuration); 
        
        transicaoParticulasSystem.gameObject.SetActive(false);
        Debug.Log("6. FX Desativado.");
    }
    
    Debug.Log("--- 7. FIM DA TRANSIÇÃO ---");
}
IEnumerator DesativarFXAposDuracao()
{
    if (transicaoParticulasSystem != null)
    {
        yield return new WaitForSeconds(transicaoParticulasSystem.main.duration); 
        
        transicaoParticulasSystem.gameObject.SetActive(false);
    }
}


}