using UnityEngine;

public class FPSRaycastInteractor : MonoBehaviour
{
    [Header("Configurações do Raio")]
    public float interactionDistance = 5f; // Distância máxima para interagir/hover

    private ClickableObjectFeedback currentTarget = null;

    void Update()
    {
        // Cria um raio a partir do centro da tela (onde a mira estaria)
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        // Variável para rastrear o alvo atingido neste frame
        ClickableObjectFeedback newTarget = null;

        // --- LÓGICA DE RAYCAST ---
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Tenta obter o script ClickableObjectFeedback do objeto atingido
            newTarget = hit.collider.GetComponent<ClickableObjectFeedback>();
        }

        // --- LÓGICA DE HOVER (Mudar o Visual) ---

        // A. Se mudamos para um novo objeto (e não estamos olhando para nada)
        if (newTarget != null && newTarget != currentTarget)
        {
            // Se já tínhamos um alvo anterior, chama o OnLookExit nele
            if (currentTarget != null)
            {
                currentTarget.OnLookExit();
            }
            
            // Define o novo alvo e chama o OnLookEnter (o efeito de borda/hover)
            currentTarget = newTarget;
            currentTarget.OnLookEnter();
        }
        // B. Se saímos do objeto atual (estamos olhando para o nada)
        else if (newTarget == null && currentTarget != null)
        {
            // Chama o OnLookExit no alvo anterior e o define como nulo
            currentTarget.OnLookExit();
            currentTarget = null;
        }


        // --- LÓGICA DE CLIQUE (Interação) ---
        if (Input.GetMouseButtonDown(0)) // Se o botão esquerdo do mouse foi clicado
        {
            // Se o objeto atual (currentTarget) não é nulo, é o objeto que estamos mirando e clicando
            if (currentTarget != null)
            {
                // Chama a função de clique no script do objeto
                currentTarget.PerformClickAction();
            }
        }
    }
}