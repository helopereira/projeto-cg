using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [Header("Alvo e Configurações")]
    [Tooltip("O objeto (Transform) estático que a câmera deve focar.")]
    public Transform target;

    [Tooltip("O deslocamento (offset) da posição do alvo. Ex: (0, 5, -10) para ficar acima e atrás.")]
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    private void Start() // Mudamos de LateUpdate() para Start()
    {
        // Garante que temos um alvo para focar
        if (target == null)
        {
            Debug.LogWarning("O alvo (target) da câmera não está definido. Defina-o no Inspector.");
            // O script pode ser desativado ou removido se o alvo estiver faltando
            enabled = false; 
            return;
        }

        // 1. Calcula a posição desejada: Posição do alvo + Deslocamento
        Vector3 desiredPosition = target.position + offset;

        // 2. Aplica a nova posição imediatamente (apenas uma vez)
        transform.position = desiredPosition;

        // 3. Faz a câmera olhar para o alvo
        transform.LookAt(target);
        
        // 4. O script já completou sua função e não precisa rodar novamente
        Destroy(this); // Remove o script da memória após executar sua função
    }
}
