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
    [Tooltip("Arraste o objeto que representa o estado Sujo.")]
    public GameObject cenarioSujo; 
    [Tooltip("Arraste o objeto que representa o estado Limpo.")]
    public GameObject cenarioLimpo;

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

// EM PescaManager.cs

// EM PescaManager.cs

public void RegistrarLixoRemovido()
{
    if (minigameCompleto) return; 
    
    lixosRemovidos++;
    
    if (lixosRemovidos >= totalLixosParaRemover)
    {
        minigameCompleto = true;
        
        // ⭐️ CORREÇÃO: Destruir lixos remanescentes para garantir a limpeza do cenário
        LixoComportamento[] lixosRestantes = FindObjectsByType<LixoComportamento>(FindObjectsSortMode.None);
        foreach (var lixo in lixosRestantes) 
        { 
            // Usa-se Destroy para garantir que nenhuma Coroutine continue rodando
            Destroy(lixo.gameObject); 
        }
        
        TrocarCenarioParaLimpo(); 
        
        Debug.Log("🎉 FASE PESCA CONCLUÍDA!");
        GameProgressManager.Instance?.RegisterGamePhaseCompleted();
    }
}
    
    private void TrocarCenarioParaLimpo()
    {
        if (cenarioSujo != null)
        {
            cenarioSujo.SetActive(false);
        }
        
        if (cenarioLimpo != null)
        {
            cenarioLimpo.SetActive(true);
        }
    }
}