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
        if (FonteManager.Instance.valvulaLiberada && !jaAtivada)
        {
            jaAtivada = true;

            if (meuAnimator != null)
            {
                meuAnimator.SetTrigger("Girar");
            }

            StartCoroutine(EsperarAguaSair());
        }
        else if (!FonteManager.Instance.valvulaLiberada)
        {
            GameProgressManager.Instance?.DisplayMessage("Conserte os canos primeiro!");
        }
    }

    IEnumerator EsperarAguaSair()
    {
        yield return new WaitForSeconds(2.0f);

        FonteManager.Instance.AtivarAguaSuja();
    }
}