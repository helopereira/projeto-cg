using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class GardenManager : MonoBehaviour
{

    public static GardenManager Instance { get; private set; }

    [Header("Status da Fase")]
    public bool faseCompleta = false;
        private List<GardenPlot> allGardenPlots = new List<GardenPlot>();
    
    private int phasesCompleted = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        allGardenPlots = FindObjectsByType<GardenPlot>(FindObjectsSortMode.None).ToList();
    }

    private object FindObjectsByType<T>()
    {
        throw new NotImplementedException();
    }
    public void RegisterPlotCompletion()
    {

        int currentCompleted = allGardenPlots.Count(plot => plot.IsPhaseComplete);
        
        if (currentCompleted != phasesCompleted)
        {
            phasesCompleted = currentCompleted;
            
            Debug.Log($"Parcela concluída registrada! Total: {phasesCompleted}/{allGardenPlots.Count}");

            if (phasesCompleted >= allGardenPlots.Count && !faseCompleta)
            {
                faseCompleta = true;

                GameProgressManager.Instance?.RegisterGamePhaseCompleted();
            }
        }
    }
}
