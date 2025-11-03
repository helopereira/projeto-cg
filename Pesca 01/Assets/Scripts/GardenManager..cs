using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class GardenManager : MonoBehaviour
{
    // Padrão Singleton: Permite que outros scripts acessem o Manager facilmente.
    public static GardenManager Instance { get; private set; }

    [Header("Status da Fase")]
    [Tooltip("Indica se todas as parcelas de terra atingiram o Estado 5 (Final).")]
    public bool faseCompleta = false;
    
    // Lista para manter o controle de todas as parcelas de terra na cena
    private List<GardenPlot> allGardenPlots = new List<GardenPlot>();
    
    private int phasesCompleted = 0; // Usado internamente para rastrear parcelas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Removido DontDestroyOnLoad: Este manager deve ser destruído se a cena do jardim mudar.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Encontra todas as parcelas de jardim na cena para rastreamento inicial
        allGardenPlots = FindObjectsByType<GardenPlot>(FindObjectsSortMode.None).ToList();
    }

    private object FindObjectsByType<T>()
    {
        throw new NotImplementedException();
    }

    // --- MÉTODOS DE RASTREAMENTO DE FASE ---

    /// <summary>
    /// Chamado pelo GardenPlot quando atinge o estado final (Estado 5).
    /// </summary>
    public void RegisterPlotCompletion()
    {
        // Verifica se todas as parcelas estão completas, recontando o estado atual
        int currentCompleted = allGardenPlots.Count(plot => plot.IsPhaseComplete);
        
        // Se houver uma mudança no número de completas
        if (currentCompleted != phasesCompleted)
        {
            phasesCompleted = currentCompleted;
            
            Debug.Log($"Parcela concluída registrada! Total: {phasesCompleted}/{allGardenPlots.Count}");

            // Verifica a conclusão global da FASE DO JARDIM
            if (phasesCompleted >= allGardenPlots.Count && !faseCompleta)
            {
                faseCompleta = true;
                Debug.Log($"🎉 FASE JARDIM CONCLUÍDA!");
                
                // NOTIFICA O GERENCIADOR GLOBAL
                GameProgressManager.Instance?.RegisterGamePhaseCompleted();
            }
        }
    }
}
