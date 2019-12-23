using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{

    public GameObject coin;

    // Start is called before the first frame update
    void Start()
    {
        coin.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(scoreCounter.score >= 50 && TimerScript.time >= 8)
        {
            setActive();
        }
    }

    void setActive()
    {
        print("Active");
        Vector2 coinVector = new Vector2(Random.Range(-8.1f, 8.1f), -170f);
        coin.transform.SetPositionAndRotation(coinVector, gameObject.transform.rotation);
        coin.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }
}
