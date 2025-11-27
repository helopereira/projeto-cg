using UnityEngine;

[RequireComponent(typeof(InventoryItem))] 
public class ProdutoLimpeza : MonoBehaviour
{
    void OnMouseDown()
    {

        if (SelectedObject.Instance == null) return;

        SelectedObject.Instance.SetSelectedTool(this.transform);

        gameObject.SetActive(false);
        
        Debug.Log("Pegou o produto de limpeza: " + gameObject.name);
    }
}