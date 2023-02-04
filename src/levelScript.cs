using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class levelScript : MonoBehaviour
{

    public GameObject levelTextObject;

    private TMPro.TextMeshProUGUI levelText;

    private AudioSource audioSrc;

    public GameObject timeUpText;

    public AudioClip levelPlaying;

    public AudioClip levelPassed;

    public AudioClip levelFailed;

    private bool musicPlaying;

    private bool afterLevelSoundsPlaying;

    public static int level = 1;

    private int soundOn;

    // Start is called before the first frame update
    void Start()
    {
        // get level from memory if possible
        if (PlayerPrefs.HasKey("Level") && totalScoreScript.tempLevelHolder < 1)
        {
            level = PlayerPrefs.GetInt("Level");
        }
        levelText = levelTextObject.GetComponent<TMPro.TextMeshProUGUI>();
        timeUpText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50f, -63f);
        timeUpText.SetActive(false);
        audioSrc = levelTextObject.GetComponent<AudioSource>();
        musicPlaying = false;
        afterLevelSoundsPlaying = false;
        soundOn = PlayerPrefs.GetInt("Sound");
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = "Level " + level;
        timeUpText.SetActive(false);
        if (TimerScript.levelFailed) //TimerScript.time == 0 && scoreCounter.score < scoreCounter.targetScore && !TimerScript.levelPassed
        {
            timeUpText.SetActive(true);
        }

        // background music
        if (TimerScript.keepTiming && !musicPlaying && soundOn == 1)
        {
            audioSrc.clip = levelPlaying;
            StartCoroutine(playMusic());
        }
        else if (!TimerScript.keepTiming)
        {
            if (audioSrc.clip == levelPlaying)
            {
                audioSrc.Stop();
                StopCoroutine(playMusic());
                afterLevelSoundsPlaying = false;
                musicPlaying = false;
            }
            // play after-level sounds
            if (TimerScript.levelPassed && !afterLevelSoundsPlaying && soundOn == 1)
            {
                audioSrc.clip = levelPassed;
                StartCoroutine(playSound());
            }
            else if (TimerScript.levelFailed && !afterLevelSoundsPlaying && soundOn == 1)
            {
                audioSrc.clip = levelFailed;
                StartCoroutine(playSound());
            }
        }
    }

    // background music
    IEnumerator playMusic()
    {
        musicPlaying = true;
        audioSrc.Play();
        yield return new WaitForSeconds(audioSrc.clip.length);
        musicPlaying = false;
    }

    // sound after level failed/passed
    IEnumerator playSound()
    {
        afterLevelSoundsPlaying = true;
        audioSrc.Play();
        yield return new WaitForSeconds(audioSrc.clip.length);
    }
}
