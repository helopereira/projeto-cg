using UnityEngine;

public class FonteManager : MonoBehaviour
{
    public static FonteManager Instance;

    [Header("Configuração Visual")]
    public GameObject grupoAgua; 
    public Renderer[] partesDaAgua; 

    [Header("Texturas (Imagens)")]
    public Texture texturaSuja; 
    public Texture texturaLimpa;
    [Header("Efeitos e Progresso")]
    public ParticleSystem bolhasParticulas;
    public int canosTotais = 5;
    private int canosReparados = 0;
    public bool valvulaLiberada = false;
    public bool fonteProntaParaLimpar = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (grupoAgua != null) grupoAgua.SetActive(false);
        if (bolhasParticulas != null) bolhasParticulas.Stop();
    }

    public void RegistrarReparoCano()
    {
        canosReparados++;
        Debug.Log($"Canos: {canosReparados}/{canosTotais}");

        if (canosReparados >= canosTotais)
        {
            valvulaLiberada = true;
            GameProgressManager.Instance?.DisplayMessage("Canos prontos! Gire a válvula.");
        }
    }

    public void AtivarAguaSuja()
    {
        if (grupoAgua != null) grupoAgua.SetActive(true);

        TrocarTextura(texturaSuja);

        fonteProntaParaLimpar = true;
        GameProgressManager.Instance?.DisplayMessage("Água suja! Use o Cloro.");
    }

    public void LimparAgua()
    {
        if (bolhasParticulas != null) bolhasParticulas.Play();

        TrocarTextura(texturaLimpa);

        Debug.Log("Fonte Restaurada!");
        GameProgressManager.Instance?.DisplayMessage("Fonte Restaurada!");
        GameProgressManager.Instance?.RegisterGamePhaseCompleted();
    }

    void TrocarTextura(Texture novaTextura)
    {
        if (partesDaAgua != null && novaTextura != null)
        {
            foreach (Renderer r in partesDaAgua)
            {

                r.material.mainTexture = novaTextura;

                r.material.SetTexture("_BaseMap", novaTextura); 
            }
        }
    }
}