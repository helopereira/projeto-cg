using UnityEngine;
using UnityEngine.EventSystems;

public class Borda : MonoBehaviour
{
    private Transform highlight;
    private RaycastHit raycastHit;

    void Update()
    {
        // -------------------------
        // CORREÇÃO DE ERRO DE FRUSTUM E CÂMERA
        // -------------------------
        if (Camera.main == null) { return; }
        
        Vector3 mousePosition = Input.mousePosition;
        if (float.IsInfinity(mousePosition.x) || float.IsInfinity(mousePosition.y))
        {
            return;
        }
        
        // 1. Desliga o contorno (Outline) do objeto anteriormente destacado (hover out)
        if (highlight != null)
        {
            Outline outlineComponent = highlight.gameObject.GetComponent<Outline>();
            if (outlineComponent != null)
            {
                outlineComponent.enabled = false;
            }
            highlight = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Certifica-se de que o clique não está sobre a UI (EventSystem) e acerta um objeto 3D
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            
            // Verifica se o objeto tem uma tag que precisa de highlight ou interação
            bool isOutlinable = highlight.CompareTag("Outlined") || highlight.CompareTag("InfoPanel");

            if (isOutlinable)
            {
                Outline outlineComponent = highlight.gameObject.GetComponent<Outline>();

                // Lógica de Highlight (Hover)
                if (outlineComponent != null)
                {
                    outlineComponent.enabled = true;
                    // Define uma cor diferente para a placa se necessário
                    if (highlight.CompareTag("InfoPanel"))
                    {
                        outlineComponent.OutlineColor = Color.cyan;
                    } 
                    else
                    {
                        outlineComponent.OutlineColor = Color.magenta;
                    }
                }
                else
                {
                    // Adiciona e configura o Outline se não existir
                    Outline newOutline = highlight.gameObject.AddComponent<Outline>();
                    newOutline.enabled = true;
                    newOutline.OutlineWidth = 7.0f;
                    
                    if (highlight.CompareTag("InfoPanel"))
                    {
                        newOutline.OutlineColor = Color.cyan;
                    } 
                    else
                    {
                        newOutline.OutlineColor = Color.magenta;
                    }
                }
                
                // --- LÓGICA DE CLIQUE (INTERAÇÃO COM O INFO PANEL) ---
                if (Input.GetMouseButtonDown(0) && highlight.CompareTag("InfoPanel"))
                {
                    InfoBoardID boardID = highlight.GetComponent<InfoBoardID>();
                    if (boardID != null && InfoPanelController.Instance != null)
                    {
                        // Chama o controlador de painéis com o ID da placa clicada
                        InfoPanelController.Instance.ShowPanel(boardID.panelID);
                    }
                }
            }
            else
            {
                // Se acertou um objeto sem a tag, zera o highlight
                highlight = null;
            }
        }
    }
}