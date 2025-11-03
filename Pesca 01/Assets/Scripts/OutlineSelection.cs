//MIT License
//Copyright (c) 2023 DA LAB (https://www.youtube.com/@DA-LAB)
//Modified by Gemini for game logic integration and global selected object tracking (2024)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    // Variáveis internas para rastrear destaque e seleção
    private Transform highlight;
    private Transform selection; 
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
        
        // -------------------------
        // 1. HIGHLIGHT (Passar o mouse)
        // -------------------------

        // Remove o highlight anterior
        if (highlight != null)
        {
            // Verifica se o objeto ainda tem o componente Outline e se não é o objeto selecionado
            Outline outlineComponent = highlight.gameObject.GetComponent<Outline>();
            if (outlineComponent != null && highlight != selection)
            {
                outlineComponent.enabled = false;
            }
            highlight = null;
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // Verifica se o ponteiro não está sobre a UI e se acertou um objeto
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            
            // Só faz highlight se tiver a tag "Selectable" e não estiver já selecionado
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                // Configura ou ativa o Outline para o destaque (magenta)
                SetupOrEnableOutline(highlight, Color.magenta, true);
            }
            else
            {
                highlight = null;
            }
        }

        // -------------------------
        // 2. SELECTION (Clique do Mouse)
        // -------------------------
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                // Se um novo objeto é clicado e ele está em destaque
                
                // Desliga o outline do objeto selecionado anteriormente
                if (selection != null)
                {
                    Outline oldOutline = selection.gameObject.GetComponent<Outline>();
                    if (oldOutline != null)
                    {
                        oldOutline.enabled = false;
                    }
                }
                
                // Define a nova seleção
                selection = raycastHit.transform;
                
                // Liga o outline (cor de seleção - amarelo)
                SetupOrEnableOutline(selection, Color.yellow, true);
                
                // Notifica o GERENCIADOR GLOBAL SelectedObject sobre a nova ferramenta selecionada
                SelectedObject.Instance?.SetSelectedTool(selection);

                highlight = null; // O objeto selecionado não precisa de highlight magenta
            }
        }

        // -------------------------
        // 3. DESSELEÇÃO (Pressionar a Barra de Espaço)
        // -------------------------
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (selection != null)
            {
                // Desliga o outline do objeto selecionado
                Outline outline = selection.gameObject.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
                
                // Limpa a seleção
                selection = null;
                
                // Notifica o GERENCIADOR GLOBAL SelectedObject que não há ferramenta selecionada
                SelectedObject.Instance?.SetSelectedTool(null);
            }
        }
    }

    // Função auxiliar para garantir que o componente Outline exista
    private void SetupOrEnableOutline(Transform target, Color color, bool enable)
    {
        Outline outline = target.gameObject.GetComponent<Outline>();
        if (outline == null)
        {
            outline = target.gameObject.AddComponent<Outline>();
            outline.OutlineWidth = 7.0f;
            // É necessário que o componente 'Outline' que você está usando tenha uma propriedade como 'OutlineMode'
            // Se você estiver usando um componente de terceiros, ajuste esta linha ou use as configurações padrão.
            // outline.OutlineMode = Outline.Mode.OutlineAll; 
        }
        outline.OutlineColor = color;
        outline.enabled = enable;
    }
}
