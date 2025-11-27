using UnityEngine;
using UnityEngine.UI; 

public class InventoryUI : MonoBehaviour
{
    [Header("Referência da UI")]
    public Image displayImage;
    public Sprite defaultEmptySprite;

    private void Start()
    {
        if (SelectedObject.Instance != null)
        {
            SelectedObject.Instance.OnToolSelectionChanged += UpdateInventoryDisplay;
        }
        UpdateInventoryDisplay(SelectedObject.Instance.SelectedTool);
    }

    private void OnDestroy()
    {
        if (SelectedObject.Instance != null)
        {
            SelectedObject.Instance.OnToolSelectionChanged -= UpdateInventoryDisplay;
        }
    }
    private void UpdateInventoryDisplay(Transform selectedTool)
    {
        if (selectedTool == null)
        {
            ClearDisplay();
            return;
        }
        InventoryItem itemData = selectedTool.GetComponent<InventoryItem>();

        if (itemData != null && itemData.icon != null)
        {
            displayImage.sprite = itemData.icon;
            displayImage.enabled = true; 
            displayImage.preserveAspect = true; 
        }
        else
        {
            Debug.LogWarning($"O objeto {selectedTool.name} não tem um componente InventoryItem ou Ícone!");
            ClearDisplay();
        }
    }

    private void ClearDisplay()
    {
        if (defaultEmptySprite != null)
        {
            displayImage.sprite = defaultEmptySprite;
            displayImage.enabled = true;
        }
        else
        {
            displayImage.sprite = null;
            displayImage.enabled = false;
        }
    }
}