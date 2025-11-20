using UnityEngine;

[RequireComponent(typeof(InventoryItem))]
public class LixoColetavel : MonoBehaviour
{
    [Header("Configuração")]
    public TiposDeLixo tipoDesteLixo;
    
    [Tooltip("Quantos pontos esse lixo vale? (Ex: 1 para lata sozinha, 3 para um monte)")]
    public int quantidadeQueVale = 1; 

    void OnMouseDown()
    {
        if (SelectedObject.Instance == null) return;

        SelectedObject.Instance.SetSelectedTool(this.transform);
        gameObject.SetActive(false);
    }
}