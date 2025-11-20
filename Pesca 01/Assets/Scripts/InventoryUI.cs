using UnityEngine;
using UnityEngine.UI; // Necessário para mexer com UI

public class InventoryUI : MonoBehaviour
{
    [Header("Referência da UI")]
    [Tooltip("Arraste aqui o componente Image do seu quadrado de inventário.")]
    public Image displayImage;

    [Tooltip("Sprite padrão para mostrar quando nada estiver selecionado (opcional). Deixe null para ficar transparente.")]
    public Sprite defaultEmptySprite;

    private void Start()
    {
        // Inscreve-se no evento do seu Singleton
        if (SelectedObject.Instance != null)
        {
            SelectedObject.Instance.OnToolSelectionChanged += UpdateInventoryDisplay;
        }
        
        // Garante que começa vazio ou atualizado
        UpdateInventoryDisplay(SelectedObject.Instance.SelectedTool);
    }

    private void OnDestroy()
    {
        // Sempre se desinscreva de eventos estáticos para evitar erros de memória
        if (SelectedObject.Instance != null)
        {
            SelectedObject.Instance.OnToolSelectionChanged -= UpdateInventoryDisplay;
        }
    }

    // Este método é chamado automaticamente sempre que o evento dispara
    private void UpdateInventoryDisplay(Transform selectedTool)
    {
        if (selectedTool == null)
        {
            // Nada selecionado: Esconde a imagem ou mostra a padrão
            ClearDisplay();
            return;
        }

        // Tenta pegar o script que criamos no Passo 1
        InventoryItem itemData = selectedTool.GetComponent<InventoryItem>();

        if (itemData != null && itemData.icon != null)
        {
            // Achou o ícone! Mostra na tela
            displayImage.sprite = itemData.icon;
            displayImage.enabled = true; // Garante que a imagem está visível
            
            // Dica: Isso evita que a imagem fique esticada
            displayImage.preserveAspect = true; 
        }
        else
        {
            // O objeto foi selecionado, mas esquecemos de colocar o script InventoryItem nele
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
            // Se não tiver sprite padrão, deixa a imagem invisível
            displayImage.sprite = null;
            displayImage.enabled = false;
        }
    }
}