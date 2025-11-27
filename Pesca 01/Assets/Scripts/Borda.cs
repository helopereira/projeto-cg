using UnityEngine;
using UnityEngine.EventSystems;

public class Borda : MonoBehaviour
{
    private Transform highlight;
    private RaycastHit raycastHit;

    void Update()
    {

        if (Camera.main == null) { return; }
        
        Vector3 mousePosition = Input.mousePosition;
        if (float.IsInfinity(mousePosition.x) || float.IsInfinity(mousePosition.y))
        {
            return;
        }
        
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
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            
            bool isOutlinable = highlight.CompareTag("Outlined") || highlight.CompareTag("InfoPanel");

            if (isOutlinable)
            {
                Outline outlineComponent = highlight.gameObject.GetComponent<Outline>();

                if (outlineComponent != null)
                {
                    outlineComponent.enabled = true;
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
                
                if (Input.GetMouseButtonDown(0) && highlight.CompareTag("InfoPanel"))
                {
                    InfoBoardID boardID = highlight.GetComponent<InfoBoardID>();
                    if (boardID != null && InfoPanelController.Instance != null)
                    {
                        InfoPanelController.Instance.ShowPanel(boardID.panelID);
                    }
                }
            }
            else
            {
                highlight = null;
            }
        }
    }
}