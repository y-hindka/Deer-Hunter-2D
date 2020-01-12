using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{

    private TMPro.TextMeshProUGUI timerText;

    public static float time;

    public GameObject levelUpImage;

    private Vector2 levelUpImageVector = new Vector2(0f, 0f);

    private float deltaTime;

    public static bool keepTiming = true;

    // Start is called before the first frame update
    void Start()
    {
        levelUpImage.SetActive(false);
        levelUpImage.transform.position = levelUpImageVector;
        timerText = GetComponent<TMPro.TextMeshProUGUI>();
        time = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelUpImage.activeSelf)
        {
            if (Input.touchCount > 0 || Input.GetKeyDown("space"))
            {
                levelUpImage.SetActive(false);
                loadNextLevel();
            }
        }
       
        if (time == 0 && scoreCounter.score < 100)
        {
            stopTimer();
            timerText.text = "Time: " + stopTimer();
        }

        else if (time >= 0 && scoreCounter.score >= 100)
        {
            timerText.text = "Time: " + stopTimer();
            levelUp();
            
            
        }

        else if (keepTiming)
        {
            updateTime();
        }


    }

    IEnumerator WaitStopLevel()
    {
        levelUpImage.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator WaitResumeLevel()
    {
        yield return new WaitForSeconds(1);
        FireBullet.allowShoot = 0;
        keepTiming = true;
    }

 

    float stopTimer()
    {
        keepTiming = false;
        return time;
    }

    void updateTime()
    {
        deltaTime += Time.deltaTime;
        time = Mathf.Round(20f - deltaTime);
        timerText.text = "Time: " + time;
    }

    void levelUp()
    {
        timerText.text = "Time: " + time;
        levelScript.level += 1;
        if (MoveDeer.deerCount >= levelScript.level)
        {
            scoreCounter.totalScore += (scoreCounter.score - 10* MoveDeer.deerCount);
        }
        else
        {
            scoreCounter.totalScore += scoreCounter.score;
        }
        scoreCounter.score = 80;
        FireBullet.allowShoot = 1;
        MoveDeer.spawnDelay -= 1f;
        StartCoroutine(WaitStopLevel());

    }

    public void loadNextLevel() 
    {
        time = 60;
        deltaTime = 0;
        StartCoroutine(WaitResumeLevel());
    }
}
