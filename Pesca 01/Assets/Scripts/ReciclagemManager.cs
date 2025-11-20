using UnityEngine;
using System.Collections.Generic; // Necessário para Listas

public class ReciclagemManager : MonoBehaviour
{
    public static ReciclagemManager Instance;

    [Header("Arrastar as 4 lixeiras aqui")]
    public List<Lixeira> lixeirasDoCenario; 

    private bool jogoFinalizado = false;

    private void Awake()
    {
        Instance = this;
    }

    public void VerificarVitoria()
    {
        if (jogoFinalizado) return;

        int lixeirasProntas = 0;

        // Passa por todas as lixeiras da lista
        foreach (Lixeira lixeira in lixeirasDoCenario)
        {
            if (lixeira.lixeiraConcluida)
            {
                lixeirasProntas++;
            }
        }

        // Se todas as lixeiras (ex: 4) estiverem prontas
        if (lixeirasProntas >= lixeirasDoCenario.Count)
        {
            FinalizarMinigame();
        }
    }

    void FinalizarMinigame()
    {
        jogoFinalizado = true;
        Debug.Log("PARABÉNS! Toda a reciclagem foi feita.");
        
        string msg = "Área Limpa! Reciclagem Concluída.";
        GameProgressManager.Instance?.DisplayMessage(msg);
        
        // Registra no seu gerenciador de progresso global
        GameProgressManager.Instance?.RegisterGamePhaseCompleted();
    }
}