using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using CloudOnce;

public class PlayButtonScript : MonoBehaviour, IPointerDownHandler
{
    private Animator an;

    private Animator an2;

    public AnimationClip anim;

    public Camera mainCam;

    public GameObject fadeOut;

    public GameObject fadeIn;

    public AudioClip backgroundMusic; // primal track

    private AudioSource audioSrc;

    public GameObject soundButton;

    public Sprite soundOnSprite;

    public Sprite soundOffSprite;

    private bool musicPlaying;

    private int soundOn; // 1 means on, 0 means off


    void Start()
    {
        StartCoroutine(fadeInAnimation());
        audioSrc = mainCam.GetComponent<AudioSource>();
        musicPlaying = false;

        //initialize cloud
        if (!PlayerPrefs.HasKey("Launched"))
        {
            Cloud.OnInitializeComplete += initializeComplete;
            Cloud.Initialize(false, true);
            PlayerPrefs.SetInt("Launched", 1);
        }


        if (PlayerPrefs.HasKey("Sound"))
        {
            soundOn = PlayerPrefs.GetInt("Sound");
            if (soundOn == 0)
            {
                soundButton.GetComponent<Image>().sprite = soundOffSprite;
            }
            else
            {
                soundButton.GetComponent<Image>().sprite = soundOnSprite;
            }
        }
        else
        {
            soundOn = 1;
            PlayerPrefs.SetInt("Sound", soundOn);
        }
    }

    public void initializeComplete()
    {
        Cloud.OnInitializeComplete -= initializeComplete;
        Debug.Log("Initialized");
    }


    void Update()
    {
        if (!musicPlaying && soundOn == 1)
        {
            StartCoroutine(playMusic());
        }
        if (soundOn == 0)
        { 
            StopCoroutine(playMusic());
            audioSrc.Stop();
            musicPlaying = false;
        }
    }

    IEnumerator playMusic()
    {
        musicPlaying = true;
        audioSrc.clip = backgroundMusic;
        audioSrc.Play();
        yield return new WaitForSeconds(backgroundMusic.length);
        musicPlaying = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        an = mainCam.GetComponent<Animator>();
        an2 = fadeOut.GetComponent<Animator>();
        MoveDeer.deerCount = 0;
        StartCoroutine(playAnimation());
        if (totalScoreScript.tempTimeHolder > 0)
        {
            TimerScript.time = totalScoreScript.tempTimeHolder;
        }
        else if (PlayerPrefs.HasKey("Time For Level"))
        {
            TimerScript.time = PlayerPrefs.GetFloat("Time For Level");
        }
    }

    IEnumerator fadeInAnimation()
    {
        fadeIn.SetActive(true);
        Animator an3 = fadeIn.GetComponent<Animator>();
        an3.enabled = true;
        an3.Play("Start");
        yield return new WaitForSeconds(1f);
        an3.enabled = false;
        fadeIn.SetActive(false);
    }

    IEnumerator waitLoadLevel()
    {
        yield return new WaitForSeconds(3);
    }

    IEnumerator playAnimation()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("Level 1");
        ao.allowSceneActivation = false;
        an.enabled = true;
        an2.enabled = true;
        fadeOut.SetActive(true);
        an2.Play("Start to Level 1 (1) Panel");
        an.Play(anim.name);
        yield return new WaitForSeconds(anim.length);
        an.enabled = false;
        an2.enabled = false;
        ao.allowSceneActivation = true;
    }

    public void soundButtonClicked()
    {
        if (soundButton.GetComponent<Image>().sprite == soundOnSprite)
        {
            soundOn = 0;
            soundButton.GetComponent<Image>().sprite = soundOffSprite;
        }
        else
        {
            soundOn = 1;
            soundButton.GetComponent<Image>().sprite = soundOnSprite;
        }
        PlayerPrefs.SetInt("Sound", soundOn);
    }

    public void openWebsite()
    {
        Application.OpenURL("https://yhapps.wixsite.com/deerhunter2d");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Launched");
    }
}