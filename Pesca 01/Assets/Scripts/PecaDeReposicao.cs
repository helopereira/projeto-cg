using UnityEngine;

[RequireComponent(typeof(InventoryItem))]
public class PecaDeReposicao : MonoBehaviour
{
    [Header("Identificação")]
    public string ID = "Peca_1"; 

    [Header("Configuração")]
    public GameObject Destino; 

    private void Start()
    {
        if (Destino != null) Destino.SetActive(false);
    }
    private void OnEnable()
    {
        if (Destino != null)
        {
            Destino.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (SelectedObject.Instance == null) return;
        SelectedObject.Instance.SetSelectedTool(this.transform);
        gameObject.SetActive(false); 
        if (Destino != null)
        {
            Destino.SetActive(true);
        }

        Debug.Log($"Peça {ID} coletada.");
    }
}