using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSpeed : MonoBehaviour
{

    public GameObject doubleSpeed;

    private Vector2 hideVector = new Vector2(-850f, -128f);

    public static bool doubleSpeedActive;

    public static Vector2 doubleSpeedVector;

    public Canvas canvas;

    private float logistic;

    private bool computedLogistic;

    // Start is called before the first frame update
    void Start()
    {
        doubleSpeed.SetActive(true);
        doubleSpeed.GetComponent<RectTransform>().anchoredPosition = hideVector;
        doubleSpeedVector = hideVector;
        doubleSpeedActive = false;
        logistic = 0;
        computedLogistic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!computedLogistic)
        {
            logistic = computeLogisticFunction() + 0.4f;
        }
        computedLogistic = MoveSight.computedLogistic;

        if (!doubleSpeedActive && levelScript.level >= 1 && logistic > 0.5 && TimerScript.time >= 2 && CoinScript.coinCount >= 2)
        {
            // need higher coin count if double damage already active
            if (DoubleDamage.doubleDamageActive)
            {
                if (CoinScript.coinCount >= 3)
                {
                    activateDoubleSpeed();
                }
            }
            else
            {
                activateDoubleSpeed();
            }
        }
        if (TimerScript.time < 2)
        {
            doubleSpeed.GetComponent<RectTransform>().anchoredPosition = hideVector;
            doubleSpeedVector = hideVector;
            doubleSpeedActive = false;
            MoveSight.speedBoosted = false;
        }
    }

    private void activateDoubleSpeed()
    {
        float screenRight = canvas.GetComponent<RectTransform>().rect.width / 2;
        Vector2 newVector = new Vector2(Random.Range(-screenRight + 50, screenRight - 50), MoveSight.rb.GetComponentInParent<RectTransform>().anchoredPosition.y);

        // make sure doesn't overlap with x2 damage
        if (DoubleDamage.doubleDamageActive)
        {
            if (Mathf.Abs(newVector.x - DoubleDamage.doubleDamageVector.x) < 120)
            {
                if (newVector.x > 0)
                {
                    newVector.x -= 240;
                }
                else
                {
                    newVector.x += 240;
                }
            }
        }
        doubleSpeed.GetComponent<RectTransform>().anchoredPosition = newVector;
        doubleSpeedVector = doubleSpeed.GetComponent<RectTransform>().anchoredPosition;
        doubleSpeedActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("DGT Handgun 4"))
        {
            doubleSpeed.GetComponent<RectTransform>().anchoredPosition = hideVector;
            doubleSpeedVector = hideVector;
            MoveSight.speedBoosted = true;
            StartCoroutine(boost());

        }
    }

    private float computeLogisticFunction()
    {
        return (1 / (1 + Mathf.Exp(-1 * Random.Range(-3f, 3f))));
    }

    IEnumerator boost()
    {
        yield return new WaitForSeconds(7);
        MoveSight.speedBoosted = false;
    }
}
