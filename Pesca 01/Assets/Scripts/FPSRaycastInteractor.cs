using UnityEngine;

public class FPSRaycastInteractor : MonoBehaviour
{
    [Header("Configurações do Raio")]
    public float interactionDistance = 5f; 

    private ClickableObjectFeedback currentTarget = null;

    void Update()
    {

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        ClickableObjectFeedback newTarget = null;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            newTarget = hit.collider.GetComponent<ClickableObjectFeedback>();
        }

        if (newTarget != null && newTarget != currentTarget)
        {
            if (currentTarget != null)
            {
                currentTarget.OnLookExit();
            }

            currentTarget = newTarget;
            currentTarget.OnLookEnter();
        }
        else if (newTarget == null && currentTarget != null)
        {
            currentTarget.OnLookExit();
            currentTarget = null;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (currentTarget != null)
            {
                currentTarget.PerformClickAction();
            }
        }
    }
}