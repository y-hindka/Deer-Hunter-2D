using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{

    public GameObject coin;

    public Canvas canvas;

    private GameObject coinTextObject;

    private TMPro.TextMeshProUGUI coinText;

    private GameObject totalCoinTextObject;

    private bool active;

    private bool localFlashing = false;

    public static int coinCount = 0;

    public static int totalCoinCount = 0;

    private Vector2 hideVector;

    private Vector2 showVector = new Vector2(9f, 62f);

    private Vector2 hideVectorFlashing = new Vector2(850f, 75f);

    // Start is called before the first frame update
    void Start()
    {
        hideVector = new Vector2(canvas.GetComponentInParent<RectTransform>().rect.width + 100, 0f);
        coin.GetComponent<RectTransform>().anchoredPosition = hideVector;
        active = false;
        coinTextObject = GameObject.Find("CoinCount");
        coinText = coinTextObject.GetComponent<TMPro.TextMeshProUGUI>();
        totalCoinTextObject = GameObject.Find("Total Coin Count");
        totalCoinTextObject.GetComponent<RectTransform>().anchoredPosition = hideVectorFlashing;
        //top right position
        coinTextObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(399f, 138f);
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreCounter.score >= (scoreCounter.targetScore - levelScript.level * 10) && TimerScript.time >= 8 && TimerScript.keepTiming && !active) 
        {
            setActive();
        }
        if (!TimerScript.keepTiming && !TimerScript.flashing)
        {
            totalCoinTextObject.GetComponent<RectTransform>().anchoredPosition = showVector;
            totalCoinTextObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Total Coins: " + totalCoinCount;
        }
        else if (TimerScript.flashing && !localFlashing)
        {
            localFlashing = true;
            flashTotalScoreScript();
        }
        else if (TimerScript.keepTiming)
        {
            totalCoinTextObject.GetComponent<RectTransform>().anchoredPosition = hideVectorFlashing;
        }
        if (!TimerScript.flashing)
        {
            coinText.text = "Coins: " + coinCount;
        }
    }

    void setActive()
    {
        Vector2 prevVector = coin.GetComponent<RectTransform>().anchoredPosition;

        float screenRight = canvas.GetComponentInParent<RectTransform>().rect.width / 2;
        Vector2 coinVector = new Vector2(Random.Range(-screenRight + 50, screenRight - 50), MoveSight.rb.GetComponentInParent<RectTransform>().anchoredPosition.y);

        // make sure coin doesn't spawn at same location twice
        if (Mathf.Abs(prevVector.x - coinVector.x) < 5)
        {
            if (coinVector.x > 0)
            {
                coinVector.x -= 15f;
            }
            else
            {
                coinVector.x += 15f;
            }
        }

        // make sure coin doesn't spawn on top of gun
        if (Mathf.Abs(coinVector.x - MoveSight.rb.GetComponentInParent<RectTransform>().anchoredPosition.x) < 10)
        {
            if (coinVector.x > 0)
            {
                coinVector.x -= 10f;
            }
            else
            {
                coinVector.x += 10f;
            }
        }

        coin.GetComponent<RectTransform>().anchoredPosition = coinVector;
        coin.SetActive(true);
        active = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("DGT Handgun 4"))
        {
            coinCount++;
            coin.GetComponent<RectTransform>().anchoredPosition = hideVector;
            //gameObject.transform.position = hideVector;
            StartCoroutine(coinPause());
        }
    }

    IEnumerator coinPause()
    {
        yield return new WaitForSeconds(1.5f);
        active = false;
    }

    private void flashTotalScoreScript()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        totalCoinTextObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Total Coins: " + totalCoinCount;
        for (int i = 0; i < 3; i++)
        {
            totalCoinTextObject.GetComponent<RectTransform>().anchoredPosition = showVector;
            yield return new WaitForSeconds(0.5f);
            totalCoinTextObject.GetComponent<RectTransform>().anchoredPosition = hideVectorFlashing;
            yield return new WaitForSeconds(0.5f);
        }
        TimerScript.flashing = false; // maybe not needed
        localFlashing = false;
    }
}
