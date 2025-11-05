using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BordaFios : MonoBehaviour
{
    private Transform highlight; // Objeto atualmente sob o mouse (hover)
    private RaycastHit raycastHit;

    [Header("Configurações de Drag and Drop")]
    [Tooltip("A tag que identifica os objetos que podem ser arrastados (ex: Plug).")]
    public string draggableTag = "Plug";

    // Variáveis para o sistema de Arrastar e Soltar (Drag and Drop)
    private Transform dragObject;
    private Rigidbody dragRB;
    private float dragDistance; // Distância da câmera até o objeto no início do drag

    void Update()
    {
        // ---------------------------------------------------------------------
        // 1. CHECAGEM DE SANIDADE (Evita erros de NullReference no início do jogo)
        // ---------------------------------------------------------------------
        if (Camera.main == null) { return; }

        // Verifica se a posição do mouse é válida antes de criar o Raycast
        Vector3 mousePosition = Input.mousePosition;
        if (float.IsInfinity(mousePosition.x) || float.IsInfinity(mousePosition.y))
        {
            return;
        }

        // ---------------------------------------------------------------------
        // 2. HIGHLIGHT (Hover) - Apenas se não estiver arrastando
        // ---------------------------------------------------------------------
        if (dragObject == null)
        {
            // Desliga o contorno do objeto anteriormente destacado
            if (highlight != null)
            {
                Outline oldOutline = highlight.gameObject.GetComponent<Outline>();
                if (oldOutline != null)
                {
                    oldOutline.enabled = false;
                }
                highlight = null;
            }

            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            
            // Verifica se o ponteiro não está sobre a UI e acerta um objeto 3D
            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
            {
                // Se o objeto acertado tem a tag de arrastar, faz o highlight
                if (raycastHit.transform.CompareTag(draggableTag))
                {
                    highlight = raycastHit.transform;
                    SetupOrEnableOutline(highlight, Color.cyan, true);
                }
            }
        }
        
        // ---------------------------------------------------------------------
        // 3. INÍCIO DO DRAG (Botão Esquerdo Pressionado)
        // ---------------------------------------------------------------------
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight != null && highlight.CompareTag(draggableTag))
            {
                dragObject = highlight;
                dragRB = dragObject.GetComponent<Rigidbody>();
                
                // Salva a distância para manter a profundidade do objeto
                dragDistance = Vector3.Distance(transform.position, dragObject.position);

                // Se houver Rigidbody, desativa temporariamente a física (Modo Cinético)
                if (dragRB != null)
                {
                    dragRB.isKinematic = true;
                }
                
                // Remove o highlight, pois ele agora é o objeto de drag
                Outline outlineComponent = dragObject.gameObject.GetComponent<Outline>();
                if (outlineComponent != null)
                {
                    outlineComponent.OutlineColor = Color.yellow; // Cor de arrasto
                }
            }
        }

        // ---------------------------------------------------------------------
        // 4. MOVIMENTO DO DRAG (Objeto sendo arrastado)
        // ---------------------------------------------------------------------
        if (dragObject != null)
        {
            // Converte a posição 2D do mouse de volta para uma posição 3D no mundo
            Vector3 mouseWorldPosition = Camera.main.ScreenPointToRay(mousePosition).GetPoint(dragDistance);
            
            // Move o objeto arrastado diretamente para a nova posição
            dragObject.position = mouseWorldPosition;
        }

        // ---------------------------------------------------------------------
        // 5. FIM DO DRAG (Botão Esquerdo Solto)
        // ---------------------------------------------------------------------
        if (Input.GetMouseButtonUp(0))
        {
            if (dragObject != null)
            {
                // Restaura a física
                if (dragRB != null)
                {
                    dragRB.isKinematic = false;
                    // Opcional: Adicionar um pequeno impulso ao soltar
                    // dragRB.velocity = Vector3.zero; 
                }
                
                // Restaura a cor do Outline
                Outline outlineComponent = dragObject.gameObject.GetComponent<Outline>();
                if (outlineComponent != null)
                {
                    outlineComponent.OutlineColor = Color.cyan; 
                }
                
                // Limpa as referências
                dragObject = null;
                dragRB = null;
            }
        }
    }
    
    // Função auxiliar para garantir que o componente Outline exista e esteja configurado
    private void SetupOrEnableOutline(Transform target, Color color, bool enable)
    {
        Outline outline = target.gameObject.GetComponent<Outline>();
        if (outline == null)
        {
            outline = target.gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineWidth = 7.0f;
        }
        outline.OutlineColor = color;
        outline.enabled = enable;
    }
}
