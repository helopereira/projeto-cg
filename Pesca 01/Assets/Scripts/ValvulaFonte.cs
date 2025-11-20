using UnityEngine;
using System.Collections;

public class ValvulaFonte : MonoBehaviour
{
    private Animator meuAnimator;
    private bool jaAtivada = false;

    void Start()
    {
        meuAnimator = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        // 1. Só funciona se o Manager disser que os canos estão prontos
        if (FonteManager.Instance.valvulaLiberada && !jaAtivada)
        {
            jaAtivada = true;

            // 2. Toca a animação
            if (meuAnimator != null)
            {
                meuAnimator.SetTrigger("Girar");
            }

            // 3. Espera a animação rodar e liga a água
            StartCoroutine(EsperarAguaSair());
        }
        else if (!FonteManager.Instance.valvulaLiberada)
        {
            GameProgressManager.Instance?.DisplayMessage("Conserte os canos primeiro!");
        }
    }

    IEnumerator EsperarAguaSair()
    {
        // Espera 2 segundos (tempo da animação da valvula girando)
        yield return new WaitForSeconds(2.0f);

        // Manda a água aparecer
        FonteManager.Instance.AtivarAguaSuja();
    }
}