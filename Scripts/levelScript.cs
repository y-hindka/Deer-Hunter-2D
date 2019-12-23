using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class levelScript : MonoBehaviour
{

    public Text levelText;

    public static int level = 1;
    // Start is called before the first frame update
    void Start()
    {
        levelText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((TimerScript.time > 0 && scoreCounter.score < 100) || (TimerScript.time == 0 && scoreCounter.score >= 100))
        {
            levelText.text = "Level " + level;
        }
        else if (TimerScript.time == 0 && scoreCounter.score < 100)
        {
            levelText.text = "Time's Up!";
        }

        if(level == 2)
        {
            
        }
    }
}
