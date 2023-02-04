using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GunShopButtonScript : MonoBehaviour, IPointerDownHandler
{

    public GameObject fadeOut;

    public GameObject gunShopErrorText;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (TimerScript.levelPassed || TimerScript.levelFailed)
        {
            StartCoroutine(fadeOutAnimation());
        }
        else
        {
            StartCoroutine(ErrorText());
        }
    }

    IEnumerator ErrorText()
    {
        //text.GetComponent<RectTransform>().anchoredPosition = new Vector2(-60,-180);
        gunShopErrorText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        //text.GetComponent<RectTransform>().anchoredPosition = new Vector2(-850, -135);
        gunShopErrorText.SetActive(false);
    }

    IEnumerator fadeOutAnimation()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("Gun Shop");
        ao.allowSceneActivation = false;
        Animator anim = fadeOut.GetComponent<Animator>();
        anim.enabled = true;
        fadeOut.SetActive(true);
        anim.Play("Start to Level 1 (1) Panel");
        yield return new WaitForSeconds(1);
        anim.enabled = false;
        ao.allowSceneActivation = true;
    }
}
