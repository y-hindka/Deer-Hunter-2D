using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class totalScoreScript : MonoBehaviour
{

    private TMPro.TextMeshProUGUI totalScoreText;

    private Vector2 hideVector = new Vector2(850f, 113f);

    private Vector2 showVector = new Vector2(107f, 113f);

    private bool localFlashing = false;

    public static int tempTimeHolder = -1;

    public static int tempRetriesHolder;

    public static int tempLevelHolder = -1;

    // Start is called before the first frame update
    void Start()
    {
        totalScoreText = GameObject.Find("TotalScore").GetComponent<TMPro.TextMeshProUGUI>();
        totalScoreText.text = "Total Score: " + scoreCounter.totalScore;
    }

    private void Update()
    {
        totalScoreText.text = "Total Score: " + scoreCounter.totalScore;
        if (!TimerScript.keepTiming && !TimerScript.flashing)
        {
            totalScoreText.GetComponent<RectTransform>().anchoredPosition = showVector;
        }

        else if (TimerScript.flashing && !localFlashing)
        {
            localFlashing = true;
            flashTotalScoreScript();
        }

        else if (!TimerScript.flashing)
        {
            totalScoreText.GetComponent<RectTransform>().anchoredPosition = hideVector;
        }
    }

    private void flashTotalScoreScript()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < 3; i++)
        {
            totalScoreText.GetComponent<RectTransform>().anchoredPosition = showVector;
            yield return new WaitForSeconds(0.5f);
            totalScoreText.GetComponent<RectTransform>().anchoredPosition = hideVector;
            yield return new WaitForSeconds(0.5f);
        }
        TimerScript.flashing = false;
        localFlashing = false;
    }

}
