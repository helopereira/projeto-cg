// Copyright notice e usings omitidos por brevidade

using UnityEngine;
using UnityEngine.EventSystems;

public class Borda : MonoBehaviour
{
    private Transform highlight;
    private RaycastHit raycastHit;

    void Update()
    {
        // -------------------------
        // CORREÇÃO DE ERRO DE FRUSTUM: Checagem de sanidade
        // Garante que a câmera exista e que a posição do mouse seja válida no início.
        // -------------------------
        if (Camera.main == null) { return; }
        
        // Se a posição do mouse tiver valores inválidos (como infinito), ignora o frame.
        Vector3 mousePosition = Input.mousePosition;
        if (float.IsInfinity(mousePosition.x) || float.IsInfinity(mousePosition.y))
        {
            return;
        }
        
        // 1. Desliga o contorno (Outline) do objeto anteriormente destacado (hover out)
        if (highlight != null)
        {
            // Verifica se o componente Outline existe antes de tentar desativá-lo
            Outline outlineComponent = highlight.gameObject.GetComponent<Outline>();
            if (outlineComponent != null)
            {
                outlineComponent.enabled = false;
            }
            highlight = null;
        }

        // 2. Cria um raio na posição do mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Certifica-se de que o clique não está sobre a UI (EventSystem) e acerta um objeto 3D
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;

            // 3. Verifica se o objeto tem a tag "Outlined"
            if (highlight.CompareTag("Outlined"))
            {
                Outline outlineComponent = highlight.gameObject.GetComponent<Outline>();

                if (outlineComponent != null)
                {
                    // Se o Outline já existe, apenas o ativa
                    outlineComponent.enabled = true;
                }
                else
                {
                    // Se o Outline não existe, o adiciona e configura
                    Outline newOutline = highlight.gameObject.AddComponent<Outline>();
                    newOutline.enabled = true;
                    
                    // Configurações padrão, você pode editar
                    newOutline.OutlineColor = Color.magenta;
                    newOutline.OutlineWidth = 7.0f;
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