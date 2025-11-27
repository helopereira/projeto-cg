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
        foreach (Lixeira lixeira in lixeirasDoCenario)
        {
            if (lixeira.lixeiraConcluida)
            {
                lixeirasProntas++;
            }
        }

        if (lixeirasProntas >= lixeirasDoCenario.Count)
        {
            FinalizarMinigame();
        }
    }

    void FinalizarMinigame()
    {
        jogoFinalizado = true;
        
        string msg = "Área Limpa! Reciclagem Concluída.";
        GameProgressManager.Instance?.DisplayMessage(msg);
        

        GameProgressManager.Instance?.RegisterGamePhaseCompleted();
    }
}