using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuManager : MonoBehaviour
{
    public GameObject painelInicial;
    public GameObject painelTutorial;
    public GameObject painelConfiguracoes;

    void Awake() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (painelInicial != null) painelInicial.SetActive(true);
        if (painelTutorial != null) painelTutorial.SetActive(false);
        if (painelConfiguracoes != null) painelConfiguracoes.SetActive(false);
    }
    
    public void AbrirTutorial()
    {
        painelInicial.SetActive(false);
        painelTutorial.SetActive(true);
    }

    public void AbrirConfiguracoes()
    {
        painelInicial.SetActive(false);
        painelConfiguracoes.SetActive(true);
    }

    public void VoltarParaPrincipal()
    {
        painelTutorial.SetActive(false);
        painelConfiguracoes.SetActive(false);
        painelInicial.SetActive(true);
    }

    public void MudarVolume(float valor)
    {
        AudioListener.volume = valor;
    }

    public void DesligarSom()
    {
        MudarVolume(0f);
    }

    public void LigarSom()
    {
        MudarVolume(1f); 
    }

    public void IniciarNovoJogo()
    {
        SceneManager.LoadScene("Cena Principal"); 
    }
}