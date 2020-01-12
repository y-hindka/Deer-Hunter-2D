using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayButtonScript : MonoBehaviour, IPointerDownHandler

{

    public void OnPointerDown(PointerEventData eventData)
    {
        SceneManager.LoadScene("Level 1");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level 1"));
        StartCoroutine(waitLoadLevel());
    }

    IEnumerator waitLoadLevel()
    {
        yield return new WaitForSeconds(3);
    }
}
