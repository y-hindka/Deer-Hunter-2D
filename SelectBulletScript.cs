using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectBulletScript : MonoBehaviour, IPointerDownHandler
{

    public GameObject bulletImage;

    public GameObject bulletSelectButton;

    public GameObject insufficientBalance;

    public GameObject padlock;

    public TMPro.TextMeshProUGUI bulletStats;

    public static Sprite selectedBullet;

    public Sprite bullet1;

    public Sprite bullet2;

    public Sprite bullet3;

    public Sprite bullet4;

    public GameObject bulletName;

    private Sprite[] bulletArray;

    private int[] bulletSpeeds = new int[] { 5, 7, 10, 3 };

    private string[] bulletDamages = new string[] { "4-6", "5-10", "2-5", "10" };

    private string[] bulletNames = new string[] { "Basic Bullet", "Crafty Cracker", "Speedy Sorcerer", "Demon" };

    private bool[] bulletsOwned = new bool[] { true, false, false, false };

    public static int bulletIndex;

    private Color bulletSelectButtonColor;

    // Start is called before the first frame update
    void Start()
    {
        bulletArray = new Sprite[] { bullet1, bullet2, bullet3, bullet4 };
        bulletIndex = 0;
        selectedBullet = bulletArray[bulletIndex];
        bulletSelectButtonColor = bulletSelectButton.GetComponent<Image>().color;
        bulletStats.text = "Speed: " + bulletSpeeds[bulletIndex] + "\n" + "Damage: " + bulletDamages[bulletIndex];
        bulletName.GetComponent<TMPro.TextMeshProUGUI>().text = bulletNames[bulletIndex];
        insufficientBalance.SetActive(false);
        padlock.SetActive(false);
        if (PlayerPrefs.HasKey("Bullet"))
        {
            selectedBullet = bulletArray[PlayerPrefs.GetInt("Bullet")];
        }
        if (PlayerPrefs.HasKey("Bullets Owned"))
        {
            bulletsOwned = JsonUtility.FromJson<OwnedBullets>(PlayerPrefs.GetString("Bullets Owned")).bulletsOwned;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletImage.GetComponent<SpriteRenderer>().sprite == selectedBullet)
        {
            bulletSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Select";
            bulletSelectButton.GetComponent<Image>().color = Color.gray;
            bulletSelectButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            bulletSelectButton.GetComponent<Image>().color = bulletSelectButtonColor;
            bulletSelectButton.GetComponent<Button>().interactable = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        bulletImage.GetComponent<SpriteRenderer>().sprite = bulletArray[(++bulletIndex) % 4];
        bulletStats.text = "Speed: " + bulletSpeeds[bulletIndex % 4] + "\n" + "Damage: " + bulletDamages[bulletIndex % 4];
        bulletName.GetComponent<TMPro.TextMeshProUGUI>().text = bulletNames[bulletIndex % 4];
        padlockActivation();
    }

    public void bulletLeftSelect()
    {
        if (bulletIndex % 4 == 0)
        {
            bulletIndex += 4;
        }
        bulletImage.GetComponent<SpriteRenderer>().sprite = bulletArray[(--bulletIndex) % 4];
        bulletStats.text = "Speed: " + bulletSpeeds[bulletIndex % 4] + "\n" + "Damage: " + bulletDamages[bulletIndex % 4];
        bulletName.GetComponent<TMPro.TextMeshProUGUI>().text = bulletNames[bulletIndex % 4];
        padlockActivation();
        
    }

    public void selectBullet()
    {
        // select bullet
        if (bulletSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text == "Select")
        {
            selectedBullet = bulletArray[bulletIndex % 4];
            PlayerPrefs.SetInt("Bullet", bulletIndex % 4);
        }
        // buy bullet
        else
        {
            if (CoinScript.totalCoinCount >= costScript.bulletCosts[bulletIndex % 4])
            {
                bulletsOwned[bulletIndex % 4] = true;
                bulletSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Select";
                CoinScript.totalCoinCount -= costScript.bulletCosts[bulletIndex % 4];
                // save coins
                PlayerPrefs.SetInt("Coins", CoinScript.totalCoinCount);

                // save to owned bullets
                OwnedBullets ob = new OwnedBullets() { bulletsOwned = bulletsOwned };
                string json = JsonUtility.ToJson(ob);
                PlayerPrefs.SetString("Bullets Owned", json);
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
        if (bulletsOwned[bulletIndex % 4] == false)
        {
            bulletSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Buy";
            if (CoinScript.totalCoinCount < costScript.bulletCosts[bulletIndex % 4])
            {
                padlock.SetActive(true);
                bulletSelectButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                padlock.SetActive(false);
                bulletSelectButton.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            bulletSelectButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Select";
            padlock.SetActive(false);
            bulletSelectButton.GetComponent<Button>().interactable = true;
        }
    }
}

[System.Serializable]
class OwnedBullets
{
    public bool[] bulletsOwned;
}
