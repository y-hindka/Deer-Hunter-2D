using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class totalScoreScript : MonoBehaviour
{

    private TMPro.TextMeshProUGUI totalScoreText;

    private Vector2 hideVector = new Vector2(25f, 2.5f);

    private Vector2 showVector = new Vector2(2.25f, 2.5f);

    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        totalScoreText = GameObject.Find("TotalScore").GetComponent<TMPro.TextMeshProUGUI>();
        totalScoreText.text = "Total Score: " + scoreCounter.totalScore;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void Update()
    {
        totalScoreText.text = "Total Score: " + scoreCounter.totalScore;
        if (!TimerScript.keepTiming)
        {
            totalScoreText.transform.position = showVector;
        }

        else
        {
            totalScoreText.transform.position = hideVector;
        }
    }
}
