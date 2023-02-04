using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreButtonScript : MonoBehaviour, IPointerDownHandler
{
    public GameObject fadeOut;

    public Camera mainCamera;

    public GameObject credits;

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(highScoreAnimation());
        /*SceneManager.LoadScene("High Scores");
        if (SceneManager.GetSceneByName("High Scores").isLoaded)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("High Scores"));
        }*/
    }

    IEnumerator highScoreAnimation()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("High Scores");
        ao.allowSceneActivation = false;
        Animator anim1 = fadeOut.GetComponent<Animator>();
        Animator anim2 = mainCamera.GetComponent<Animator>();
        fadeOut.SetActive(true);
        anim1.enabled = true;
        anim2.enabled = true;
        // play animations here
        anim1.Play("Start to Level 1 (1) Panel");
        anim2.Play("Start to High Scores");
        yield return new WaitForSeconds(1);
        anim1.enabled = false;
        anim2.enabled = false;
        ao.allowSceneActivation = true;
    }

    public void creditsClicked()
    {
        if (!credits.activeSelf)
        {
            credits.SetActive(true);
        }
        else
        {
            credits.SetActive(false);
        }
    }
}
