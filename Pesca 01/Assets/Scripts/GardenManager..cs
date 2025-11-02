using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GardenManager : MonoBehaviour
{
    // Variável global para checar se todas as terras estão no estado final
    [Tooltip("Indica se todas as parcelas de terra atingiram o Estado 3 (Final).")]
    public bool faseCompleta = false;
    
    // Lista para manter o controle de todas as parcelas de terra na cena
    private List<GardenPlot> allGardenPlots = new List<GardenPlot>();

    // Registra uma nova parcela de terra
    public void RegisterPlot(GardenPlot plot)
    {
        if (!allGardenPlots.Contains(plot))
        {
            allGardenPlots.Add(plot);
        }
    }

    // Método chamado para verificar se a fase está completa
    public void CheckPhaseCompletion()
    {
        if (allGardenPlots.Count == 0)
        {
            // Não há parcelas de terra para verificar.
            faseCompleta = false;
            return;
        }
        
        // Checa se TODAS as parcelas de terra estão no estado final (IsPhaseComplete retorna true)
        bool allComplete = allGardenPlots.All(plot => plot.IsPhaseComplete);
        
        if (allComplete && !faseCompleta)
        {
            faseCompleta = true;
            Debug.Log("🎉 FASE COMPLETA! Todas as terras atingiram o Estado 3.");
            // Aqui você pode adicionar lógica de transição de cena, HUD, etc.
        }
    }


}