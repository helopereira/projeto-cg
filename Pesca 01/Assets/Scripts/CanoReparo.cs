using UnityEngine;

public class CanoReparo : MonoBehaviour
{
    private bool jaReparado = false;
    void OnMouseDown()
    {

        if (!jaReparado && FonteManager.Instance != null)
        {
            jaReparado = true;
            FonteManager.Instance.RegistrarReparoCano();
            gameObject.SetActive(false); 
        }
    }
}