using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class TimerScript : MonoBehaviour, IUnityAdsListener
{

    private TMPro.TextMeshProUGUI timerText;

    public static float time;

    public GameObject levelUpImage;

    public GameObject menuButton;

    public GameObject continueButton;

    public GameObject retryButton;

    public GameObject videoAdButton;

    public GameObject startOverButton;

    public GameObject newHighScore;

    public GameObject fadeIn;

    public GameObject fadeOut;

    public static int retriesLeft;

    private Vector2 levelUpImageVector = new Vector2(0f, -35f);

    private float deltaTime;

    public static bool keepTiming = true;

    public static bool flashing = false;

    public static bool levelPassed = true;

    public static bool levelFailed = false;

    private bool decreaseScore = false;

    private float timeForLevel;

    public static bool paused = false;

    private bool playingAnimation = false;

    private string videoID = "video";

    private string rewardedVideoID = "rewardedVideo";

    public static bool newHighScoreActive = false;

    private Vector2 leftButtonVector = new Vector2(-260f, -65f);
    private Vector2 rightButtonVector = new Vector2(260f, -65f);

    string appID = "ca-app-pub-3186669794523908~9166474271"; // iOS appID

    private static bool rewardedAdUsed = false;

    public static int bigDeerRandom;


    // Start is called before the first frame update
    void Start()
    { 
        StartCoroutine(fadeInAnimation());

        // get total coin count from memory
        if (CoinScript.totalCoinCount == 0 && PlayerPrefs.HasKey("Coins"))
        {
            CoinScript.totalCoinCount = PlayerPrefs.GetInt("Coins");
        }
        // get time for level from memory
        if (PlayerPrefs.HasKey("Time For Level"))
        {
            timeForLevel = PlayerPrefs.GetFloat("Time For Level");
        }
        else
        {
            timeForLevel = 25f;
        }

        // get delta time from memory
        if (PlayerPrefs.HasKey("Delta Time"))
        {
            deltaTime = PlayerPrefs.GetFloat("Delta Time");
        }

        // returning to game after visiting menu while paused
        if (totalScoreScript.tempTimeHolder > 0)
        {
            initialize();

            paused = true;
            keepTiming = false;
            time = totalScoreScript.tempTimeHolder;
            deltaTime = (timeForLevel - time);

            levelUpImage.SetActive(false);
            timerText.text = "Time: " + time;

            menuButton.SetActive(true);
            continueButton.SetActive(true);
            retryButton.SetActive(false);
            startOverButton.SetActive(false);
        }
        // initial start of game
        else if (scoreCounter.totalScore == 0 && scoreCounter.score == 0 && keepTiming)
        {
            initialize();

            time = timeForLevel;
            levelPassed = false;

            levelUpImage.SetActive(false);
            menuButton.SetActive(false);
            continueButton.SetActive(false);
            startOverButton.SetActive(false);
            retryButton.SetActive(false);
            retriesLeft = 3;
            if (PlayerPrefs.HasKey("Retries"))
            {
                retriesLeft = PlayerPrefs.GetInt("Retries");
            }
            if (PlayerPrefs.HasKey("Last Level Failed"))
            {
                retriesLeft--;
            }
        }
        // returning to game after visiting menu while level was passed
        else if (scoreCounter.score >= scoreCounter.targetScore)
        {
            initialize();

            print(levelScript.level);
            levelScript.level = totalScoreScript.tempLevelHolder;
            levelUpImage.SetActive(true);
            menuButton.SetActive(true);
            continueButton.SetActive(true);
            retryButton.SetActive(false);
            startOverButton.SetActive(false);
            keepTiming = false;


        }
        // returning to game after visiting menu while level was failed
        else
        {
            initialize();

            levelUpImage.SetActive(false);
            menuButton.SetActive(true);
            retriesLeft = totalScoreScript.tempRetriesHolder;
            if (retriesLeft > 0)
            {
                retryButton.SetActive(true);
                retryButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Retry (" + retriesLeft + " left)";
                startOverButton.SetActive(false);
                continueButton.SetActive(false);
            }
            else
            {
                retryButton.SetActive(false);
                menuButton.SetActive(true);
                startOverButton.SetActive(true);
                continueButton.SetActive(false);
            }

            keepTiming = false;
        }
        newHighScore.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -850f);
        newHighScore.GetComponent<RectTransform>().Find("Save Button").GetComponent<Button>().enabled = false;

        bigDeerRandom = Random.Range(1, 6);
    }

    private void initialize()
    {
        timerText = GetComponent<TMPro.TextMeshProUGUI>();
        menuButton.GetComponent<RectTransform>().anchoredPosition = leftButtonVector;
        continueButton.GetComponent<RectTransform>().anchoredPosition = rightButtonVector;
        retryButton.GetComponent<RectTransform>().anchoredPosition = rightButtonVector;
        startOverButton.GetComponent<RectTransform>().anchoredPosition = rightButtonVector;
        levelUpImage.GetComponent<RectTransform>().anchoredPosition = levelUpImageVector;
        levelUpImage.GetComponent<SpriteRenderer>().sortingOrder = 4;
    }

    // Update is called once per frame
    void Update()
    {
        // level failed
        if (time == 0 && scoreCounter.score < scoreCounter.targetScore)
        {
            MoveSight.computedLogistic = false;
            PlayerPrefs.DeleteKey("Delta Time");
            levelFailed = true;
            levelPassed = false;
            timerText.text = "Time: " + stopTimer();
            if (!decreaseScore)
            {
                StartCoroutine(WaitDecreaseScore());
            }
            continueButton.SetActive(false);
            retryButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Retry (" + retriesLeft + " left)";
            if (retriesLeft > 0)
            {
                retryButton.SetActive(true);
                PlayerPrefs.SetInt("Last Level Failed", 1);
            }
            // no retries left
            else
            {
                // check if new high score
                if (!flashing && HighScoresTable.checkNewHighScore(scoreCounter.totalScore))
                {
                    newHighScore.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -20f);
                    newHighScore.GetComponent<RectTransform>().Find("Score Text").GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + scoreCounter.totalScore;
                    newHighScoreActive = true;
                    menuButton.GetComponent<Button>().enabled = false;
                    startOverButton.GetComponent<Button>().enabled = false;
                }
                PlayerPrefs.DeleteKey("Total Score");
                PlayerPrefs.DeleteKey("Level");
                PlayerPrefs.DeleteKey("Time For Level");
                PlayerPrefs.DeleteKey("Last Level Failed");
                startOverButton.SetActive(true);
                if (!rewardedAdUsed)
                {
                    videoAdButton.SetActive(true);
                }
            }
        }

        // level passed
        else if (time == 0 && scoreCounter.score >= scoreCounter.targetScore)
        {
            MoveSight.computedLogistic = false;
            PlayerPrefs.DeleteKey("Delta Time");
            timerText.text = "Time: " + stopTimer();
            if (!levelPassed)
            {
                WaitStopLevel();
            }
            if ((levelScript.level + 1) % 5 == 0)
            {
                PlayerPrefs.SetFloat("Time For Level", timeForLevel + 2.5f);
            }
            PlayerPrefs.SetInt("Coins", CoinScript.totalCoinCount);
            PlayerPrefs.SetInt("Level", levelScript.level + 1);
            PlayerPrefs.SetInt("Total Score", scoreCounter.totalScore);
            levelPassed = true;
            levelFailed = false;
        }

        // in middle of level
        else if (keepTiming)
        {
            updateTime();
        }

        // update timer text color
        if (MoveSight.speedBoosted)
        {
            timerText.color = new Color32(30, 149, 19, 255);
        }
        else
        {
            timerText.color = new Color32(247, 156, 17, 255);
        }


    }

    public void saveHighScore()
    {
        string name = newHighScore.GetComponent<RectTransform>().Find("Name Input").GetComponent<TMPro.TMP_InputField>().text;
        HighScoresTable.addEntry(scoreCounter.totalScore, name);
        newHighScore.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -850f);
        newHighScore.GetComponent<RectTransform>().Find("Save Button").GetComponent<Button>().enabled = false;
        newHighScoreActive = false;
        menuButton.GetComponent<Button>().enabled = true;
        startOverButton.GetComponent<Button>().enabled = true;
    }

    public void inputTextChanged()
    {
        if (newHighScore.GetComponent<RectTransform>().Find("Name Input").GetComponent<TMPro.TMP_InputField>().text.Length > 0)
        {
            newHighScore.GetComponent<RectTransform>().Find("Save Button").GetComponent<Button>().enabled = true;
        }
        else
        {
            newHighScore.GetComponent<RectTransform>().Find("Save Button").GetComponent<Button>().enabled = false;
        }
    }

    public void startOver()
    {
        levelScript.level = 1;
        resetLevel(true);
        startOverButton.SetActive(false);
        videoAdButton.SetActive(false);
        retriesLeft = 3;
        rewardedAdUsed = false;
        scoreCounter.targetScore = 50;
    }

    public void resetLevel(bool startOver)
    {
        deltaTime = 0;
        time = timeForLevel;
        scoreCounter.score = 0;
        CoinScript.coinCount = 0;
        StartCoroutine(WaitResumeLevel(0));
        retryButton.SetActive(false);
        startOverButton.SetActive(false);
        videoAdButton.SetActive(false);
        retriesLeft -= 1;
        if (!startOver)
        {
            PlayerPrefs.SetInt("Retries", retriesLeft);
            PlayerPrefs.SetInt("Total Score", scoreCounter.totalScore);
            PlayerPrefs.SetInt("Level", levelScript.level);
            PlayerPrefs.SetFloat("Time For Level", timeForLevel);
        }
        else
        {
            PlayerPrefs.DeleteKey("Total Score");
            PlayerPrefs.DeleteKey("Level");
            PlayerPrefs.DeleteKey("Time For Level");
        }
        menuButton.SetActive(false);
        decreaseScore = false;
        levelFailed = false;
        newHighScore.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -850f);
        newHighScoreActive = false;
    }

    public void pauseLevel()
    {
        keepTiming = false;
        paused = true;
        FireBullet.allowShoot = 1;
        menuButton.SetActive(true);
        continueButton.SetActive(true);
        PlayerPrefs.SetFloat("Delta Time", deltaTime);
    }

    IEnumerator WaitStopLevelNoContinue()
    {
        menuButton.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }

    private void WaitStopLevel()
    {
        levelUpImage.SetActive(true);
        menuButton.SetActive(true);
        continueButton.SetActive(true);
        StartCoroutine(WaitDecreaseScore()); // call flash score method
    }

    IEnumerator WaitResumeLevel(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        FireBullet.allowShoot = 0;
        keepTiming = true;
        levelPassed = false;
    }

    IEnumerator WaitDecreaseScore()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize("3237448", false); // ios id, test mode not enabled

        decreaseScore = true;
        flashing = true;
        scoreCounter.totalScore += scoreCounter.score;
        CoinScript.totalCoinCount += CoinScript.coinCount;
        retryButton.GetComponent<Button>().enabled = false;
        menuButton.GetComponent<Button>().enabled = false;
        continueButton.GetComponent<Button>().enabled = false;
        startOverButton.GetComponent<Button>().enabled = false;
        yield return new WaitForSeconds(3); // wait for flashing to finish
        retryButton.GetComponent<Button>().enabled = true;
        continueButton.GetComponent<Button>().enabled = true;
        if (!levelPassed)
        {
            CoinScript.totalCoinCount -= CoinScript.coinCount;
            scoreCounter.totalScore -= scoreCounter.score;
        }
        // show ad every 3 levels
        if (retriesLeft > 0 && Advertisement.IsReady(videoID) && (levelScript.level == 1 || levelScript.level % 3 == 0))
        {
            Advertisement.Show(videoID);
        }
        if (!newHighScoreActive)
        {
            menuButton.GetComponent<Button>().enabled = true;
            startOverButton.GetComponent<Button>().enabled = true;
        }
    }

    IEnumerator fadeInAnimation()
    {
        playingAnimation = true;
        fadeIn.SetActive(true);
        Animator an = fadeIn.GetComponent<Animator>();
        an.enabled = true;
        an.Play("Start to Level 1 (2)");
        yield return new WaitForSeconds(1);
        an.enabled = false;
        fadeIn.SetActive(false);
        playingAnimation = false;
    }

 

    float stopTimer()
    {
        keepTiming = false;
        menuButton.SetActive(true);
        return time;
    }

    void updateTime()
    {
        if (!playingAnimation)
        {
            deltaTime += Time.deltaTime;
            time = Mathf.Round(timeForLevel - deltaTime);
        }
        timerText.text = "Time: " + time;
    }

    void levelUp()
    {
        timerText.text = "Time: " + time;
        levelScript.level += 1;
        // before adding to total score, decrease score by 10x number of deer remaning at end of level
        /*if (MoveDeer.deerCount >= levelScript.level)
        {
            scoreCounter.totalScore += (scoreCounter.score - 10 * MoveDeer.deerCount);
        }
        // if no deer remain, then don't decrease score
        else
        {
            scoreCounter.totalScore += scoreCounter.score;
        }*/
        // for now eliminating decrease of score
        scoreCounter.score = 0;
        CoinScript.coinCount = 0;
        FireBullet.allowShoot = 1;
        // allows increase of 20 to target score every 5 levels, otherwise increases by 10
        scoreCounter.targetScore += 10;
        if (levelScript.level % 5 == 0)
        {
            scoreCounter.targetScore += 10;
            timeForLevel += 2.5f;
            MoveDeer.bigDeerUsed = false;
        }
        decreaseScore = false;
        bigDeerRandom = Random.Range(1, 6);
        PlayerPrefs.SetInt("Retries", retriesLeft);
    }

    public void loadNextLevel() 
    {
        deltaTime = 0;
        time = timeForLevel;
        StartCoroutine(WaitResumeLevel(0));
    }

    public void continueClicked()
    {
        // go to next level
        if (!paused)
        {
            levelUpImage.SetActive(false);
            continueButton.SetActive(false);
            menuButton.SetActive(false);
            levelUp();
            loadNextLevel();
        }
        // resume from pause
        else
        {
            continueButton.SetActive(false);
            menuButton.SetActive(false);
            keepTiming = true;
            paused = false;
            FireBullet.allowShoot = 0;
        }
    }

    public void menuClicked()
    {
        StartCoroutine(fadeOutAnimation());
    }

    IEnumerator fadeOutAnimation()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("Start");
        ao.allowSceneActivation = false;
        Animator an = fadeOut.GetComponent<Animator>();
        an.enabled = true;
        levelUpImage.GetComponent<SpriteRenderer>().sortingOrder = 3;
        fadeOut.SetActive(true);
        an.Play("Level 1 to Start");
        yield return new WaitForSeconds(1.5f);
        an.enabled = false;
        ao.allowSceneActivation = true;
    }

    public void showRewardedAd()
    {
        Advertisement.Show(rewardedVideoID);
        videoAdButton.SetActive(false);
        rewardedAdUsed = true;
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId.Equals(rewardedVideoID) && showResult == ShowResult.Finished)
        {
            rewardUser();
        }
    }

    private void rewardUser()
    {
        retriesLeft = 1;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Launched");
    }
}
