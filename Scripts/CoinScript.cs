using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{

    public GameObject coin;

    private bool active;

    private int coinCount;

    private Vector2 hideVector = new Vector2(25f, -3.2f);

    // Start is called before the first frame update
    void Start()
    {
        coin.transform.position = hideVector;
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(scoreCounter.score >= 50 && TimerScript.time >= 8 && !active)
        {
            setActive();
        }
    }

    void setActive()
    {
        print("Active");
        Vector2 coinVector = new Vector2(Random.Range(-8.1f, 8.1f), -3.2f);
        coin.transform.position = coinVector;
        coin.SetActive(true);
        active = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        coinCount++;
        gameObject.transform.position = hideVector;
        active = false;
    }
}
