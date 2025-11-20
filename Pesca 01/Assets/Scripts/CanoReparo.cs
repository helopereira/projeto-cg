using UnityEngine;

public class CanoReparo : MonoBehaviour
{
    private bool jaReparado = false;

    void OnMouseDown()
    {
        // Verifica se tem ferramenta na mão (Opcional: exigir chave inglesa?)
        // Por enquanto vou deixar clique simples como pediu
        if (!jaReparado && FonteManager.Instance != null)
        {
            jaReparado = true;
            
            // Avisa o gerente
            FonteManager.Instance.RegistrarReparoCano();

            // Visual: Pode sumir com o cano quebrado ou mudar a cor
            gameObject.SetActive(false); 
            
            // Se quiser trocar por um cano "novo", pode instanciar ou ativar um filho aqui
        }
    }
}