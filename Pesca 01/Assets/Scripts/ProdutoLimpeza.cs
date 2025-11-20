using UnityEngine;

[RequireComponent(typeof(InventoryItem))] // Obriga a ter o script de ícone
public class ProdutoLimpeza : MonoBehaviour
{
    void OnMouseDown()
    {
        // Verifica se o sistema de seleção existe
        if (SelectedObject.Instance == null) return;

        // Coloca a garrafa na mão do jogador (Inventário)
        SelectedObject.Instance.SetSelectedTool(this.transform);

        // Desativa a garrafa da cena (ela agora está "na mão")
        gameObject.SetActive(false);
        
        Debug.Log("Pegou o produto de limpeza: " + gameObject.name);
    }
}