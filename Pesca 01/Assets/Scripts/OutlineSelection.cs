using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection; 
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
            if (outlineComponent != null && highlight != selection)
            {
                outlineComponent.enabled = false;
            }
            highlight = null;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
        
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                SetupOrEnableOutline(highlight, Color.magenta, true);
            }
            else
            {
                highlight = null;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                if (selection != null)
                {
                    Outline oldOutline = selection.gameObject.GetComponent<Outline>();
                    if (oldOutline != null)
                    {
                        oldOutline.enabled = false;
                    }
                }
                selection = raycastHit.transform;
                SetupOrEnableOutline(selection, Color.yellow, true);
                SelectedObject.Instance?.SetSelectedTool(selection);
                highlight = null; 
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (selection != null)
            {
                Outline outline = selection.gameObject.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
                selection = null;
                SelectedObject.Instance?.SetSelectedTool(null);
            }
        }
    }
    private void SetupOrEnableOutline(Transform target, Color color, bool enable)
    {
        Outline outline = target.gameObject.GetComponent<Outline>();
        if (outline == null)
        {
            outline = target.gameObject.AddComponent<Outline>();
            outline.OutlineWidth = 7.0f;
        }
        outline.OutlineColor = color;
        outline.enabled = enable;
    }
}
