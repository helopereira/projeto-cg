using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class ReconstrucaoManager : MonoBehaviour, ICompletable
{
    public float tempoTotal = 0f;
    private bool minigameAtivo = false;
    public int totalDePecas = 5; 
    private int pecasEncaixadas = 0;
    public PlayerInventory inventario; 
    void Update()
    {
        if (minigameAtivo)
        {
            tempoTotal += Time.deltaTime;
        }
    }
    public void IniciarMinigame()
    {
        if (!minigameAtivo)
        {
            minigameAtivo = true;
            tempoTotal = 0f;
            Debug.Log("Minigame de Reconstrução iniciado!");
        }
    }
    
        void Start()
    {
        // ⭐️ CHAMA O INÍCIO AUTOMATICAMENTE PARA TESTE
        IniciarMinigame(); 
    }
    public void ContarEncaixe()
    {
       
        if (!minigameAtivo) return;

        pecasEncaixadas++;

        Debug.Log($"Peça Encaixada! Total: {pecasEncaixadas} / {totalDePecas}"); 

        if (pecasEncaixadas >= totalDePecas)
        {
            Debug.Log("Todas as peças foram encaixadas! Chamando FinalizarMinigame."); 
            FinalizarMinigame();
        }
    }

    // Em ReconstrucaoManager.cs

    public void FinalizarMinigame()
    {
        Debug.Log("Fase de Encaixe: FINALIZAR MINIGAME INICIADO.");
        minigameAtivo = false;

        string mensagem = "PARABÉNS! Você reconstruiu o banco em: " + tempoTotal.ToString("F2") + " segundos!";
        Debug.Log(mensagem);
        
        // Checa uma única vez
        if (GameProgressManager.Instance == null)
        {
            Debug.LogError("ERRO FATAL: GameProgressManager.Instance é nulo. A FASE NÃO PODE SER REGISTRADA!");
            return;
        }

        GameProgressManager.Instance.RegisterGamePhaseCompleted();
        
        // Adiciona um log para confirmar que a notificação ocorreu
        Debug.Log("Fase de Encaixe: Notificação enviada ao GameProgressManager.");
    }
}