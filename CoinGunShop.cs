using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGunShop : MonoBehaviour
{
    public GameObject coinTextObject;

    private AudioSource audioSrc;

    private TMPro.TextMeshProUGUI coinText;

    private bool musicPlaying;

    private int soundOn;

    // Start is called before the first frame update
    void Start()
    {
        coinText = coinTextObject.GetComponent<TMPro.TextMeshProUGUI>();
        audioSrc = coinTextObject.GetComponent<AudioSource>();
        musicPlaying = false;
        if (CoinScript.totalCoinCount == 0 && PlayerPrefs.HasKey("Coins"))
        {
            CoinScript.totalCoinCount = PlayerPrefs.GetInt("Coins");
        }
        soundOn = PlayerPrefs.GetInt("Sound");
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = "Coins: "  + CoinScript.totalCoinCount;
        if (!musicPlaying && soundOn == 1)
        {
            StartCoroutine(playMusic());
        }
    }

    IEnumerator playMusic()
    {
        musicPlaying = true;
        audioSrc.Play();
        yield return new WaitForSeconds(audioSrc.clip.length);
        musicPlaying = false;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("Launched");
    }
}
