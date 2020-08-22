using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreCounter : MonoBehaviour
{

    public static int score = 0;

    public static int totalScore = 0;

    public static int targetScore = 50;

    private TMPro.TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TMPro.TextMeshProUGUI>();
        if (PlayerPrefs.HasKey("Level"))
        {
            int level = 0;
            if (totalScoreScript.tempLevelHolder < 1)
            {
                level = PlayerPrefs.GetInt("Level");
            }
            else
            {
                level = totalScoreScript.tempLevelHolder;
            }
            int dividedFive = level / 5;
            targetScore = 50;
            targetScore = (targetScore + (10 * (level - 1)) + (dividedFive * 10));
        }
        if (PlayerPrefs.HasKey("Total Score"))
        {
            totalScore = PlayerPrefs.GetInt("Total Score");
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score + "/" + targetScore;

        // update score text color
        if (bullet.damageBoosted)
        {
            // change to red
            scoreText.color = new Color32(238,26,17,255);
        }
        else
        {
            // change back to orange
            scoreText.color = new Color32(247,156,17,255);
        }
    }
}
