using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{

    private Text timerText;

    public static float time;

    public GameObject levelUpImage;

    private Vector2 levelUpImageVector = new Vector2(0f, 0f);

    private float deltaTime;

    private bool keepTiming = true;

    // Start is called before the first frame update
    void Start()
    {
        levelUpImage.SetActive(false);
        levelUpImage.transform.position = levelUpImageVector;
        timerText = GetComponent<Text>();
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
        yield return new WaitForSeconds(0.5f);
        levelUpImage.SetActive(true);
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
        scoreCounter.totalScore += scoreCounter.score;
        scoreCounter.score = 0;
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
