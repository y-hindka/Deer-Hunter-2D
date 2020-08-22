using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectGunScript : MonoBehaviour, IPointerDownHandler
{

    public GameObject gunImage;

    public GameObject gunSelectButton;

    public GameObject insufficientBalance;

    public GameObject padlock;

    public GameObject fadeIn;

    public GameObject fadeOut;

    public TMPro.TextMeshProUGUI gunStats;

    public static Sprite selectedGun;

    public Sprite gun1;

    public Sprite gun2;

    public Sprite gun3;

    public Sprite gun4;

    public GameObject gunName;

    private Sprite[] spriteArray;

    private int[] gunSpeeds = new int[] { 5, 7, 10, 3 };

    private string[] gunDamages = new string[] { "4-6", "5-10", "2-5", "10" };

    private string[] gunNames = new string[] { "Basic Boomer", "Orange You Glad It's Yours", "White Delight", "Deadly Destroyer"}; // Crafty Cracker

    private bool[] gunsOwned = new bool[] { true, false, false, false };

    public static int index;

    private Color selectButtonColor;

    private int soundOn;

    void Start()
    {
        soundOn = PlayerPrefs.GetInt("Sound");
        StartCoroutine(fadeInAnimation());
        spriteArray = new Sprite[] { gun1, gun2, gun3, gun4 };
        index = 0;
        selectedGun = spriteArray[index];
        selectButtonColor = gunSelectButton.GetComponent<Image>().color;
        gunStats.text = "Speed: " + gunSpeeds[index] + "\n" + "Damage: " + gunDamages[index];
        gunName.GetComponent<TMPro.TextMeshProUGUI>().text = gunNames[index];
        insufficientBalance.SetActive(false);
        padlock.SetActive(false);
        if (PlayerPrefs.HasKey("Gun"))
        {
            selectedGun = spriteArray[PlayerPrefs.GetInt("Gun")];
        }
        if (PlayerPrefs.HasKey("Guns Owned"))
        {
            gunsOwned = JsonUtility.FromJson<OwnedGuns>(PlayerPrefs.GetString("Guns Owned")).gunsOwned;
        }
    }

    void Update()
    {
        if (gunImage.GetComponent<SpriteRenderer>().sprite == selectedGun)
        {
            gunSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Select";
            gunSelectButton.GetComponent<Image>().color = Color.gray;
            gunSelectButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            gunSelectButton.GetComponent<Image>().color = selectButtonColor;
            gunSelectButton.GetComponent<Button>().interactable = true;
        }
    }

    // called when next button is clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        gunImage.GetComponent<SpriteRenderer>().sprite = spriteArray[(++index) % 4];
        gunStats.text = "Speed: " + gunSpeeds[index%4] + "\n" + "Damage: " + gunDamages[index%4];
        gunName.GetComponent<TMPro.TextMeshProUGUI>().text = gunNames[index%4];
        padlockActivation();
    }

    // called when previous button is clicked
    public void leftSelect()
    {
        if (index % 4 == 0)
        {
            index += 4;
        }
        gunImage.GetComponent<SpriteRenderer>().sprite = spriteArray[(--index) % 4];
        gunStats.text = "Speed: " + gunSpeeds[index%4] + "\n" + "Damage: " + gunDamages[index%4];
        gunName.GetComponent<TMPro.TextMeshProUGUI>().text = gunNames[index%4];
        padlockActivation();
    }

    public void selectGun()
    {
        // select gun
        if (gunSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text == "Select")
        {
            selectedGun = spriteArray[index % 4];
            PlayerPrefs.SetInt("Gun", index % 4);
        }
        // buy gun
        else
        {
            if (CoinScript.totalCoinCount >= costScript.gunCosts[index % 4])
            {
                gunsOwned[index % 4] = true;
                gunSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Select";
                CoinScript.totalCoinCount -= costScript.gunCosts[index % 4];
                // save coins
                PlayerPrefs.SetInt("Coins", CoinScript.totalCoinCount);

                // save to owned guns
                OwnedGuns og = new OwnedGuns() { gunsOwned = gunsOwned };
                string json = JsonUtility.ToJson(og);
                PlayerPrefs.SetString("Guns Owned", json);
            }

            else
            {
                insufficientBalance.SetActive(true);
                StartCoroutine(waitInsufficientBalance());
            }
        }
    }

    IEnumerator waitInsufficientBalance()
    {
        yield return new WaitForSeconds(1.5f);
        insufficientBalance.SetActive(false);
    }

    private void padlockActivation()
    {
        if (gunsOwned[index % 4] == false)
        {
            gunSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Buy";
            if (CoinScript.totalCoinCount < costScript.gunCosts[index % 4])
            {
                padlock.SetActive(true);
                gunSelectButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                padlock.SetActive(false);
                gunSelectButton.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            gunSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Select";
            padlock.SetActive(false);
            gunSelectButton.GetComponent<Button>().interactable = true;
        }
    }

    IEnumerator fadeInAnimation()
    {
        Animator anim = fadeIn.GetComponent<Animator>();
        fadeIn.SetActive(true);
        anim.enabled = true;
        if (soundOn == 0)
        {
            fadeIn.GetComponent<AudioSource>().playOnAwake = false;
        }
        else
        {
            fadeIn.GetComponent<AudioSource>().playOnAwake = true;
        }
        anim.Play("Gun Shop");
        yield return new WaitForSeconds(3);
        anim.enabled = false;
        fadeIn.SetActive(false);
    }

    public void menuClicked()
    {
        StartCoroutine(fadeOutAnimation());
    }

    IEnumerator fadeOutAnimation()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("Start");
        ao.allowSceneActivation = false;
        Animator anim = fadeOut.GetComponent<Animator>();
        fadeOut.SetActive(true);
        anim.enabled = true;
        anim.Play("Gun Shop to Start");
        yield return new WaitForSeconds(1);
        anim.enabled = false;
        ao.allowSceneActivation = true;
    }
}

[System.Serializable]
class OwnedGuns
{
    public bool[] gunsOwned;
}
