using UnityEngine;

[RequireComponent(typeof(InventoryItem))]
public class LixoColetavel : MonoBehaviour
{
    [Header("Configuração")]
    public TiposDeLixo tipoDesteLixo;

    public int quantidadeQueVale = 1; 

    void OnMouseDown()
    {
        if (SelectedObject.Instance == null) return;

        SelectedObject.Instance.SetSelectedTool(this.transform);
        gameObject.SetActive(false);
    }
}