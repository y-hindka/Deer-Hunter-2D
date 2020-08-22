using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDamage : MonoBehaviour
{

    public GameObject doubleDamage;

    private Vector2 hideVector = new Vector2(-950f, -128f);

    public static bool doubleDamageActive;

    public static Vector2 doubleDamageVector;

    public Canvas canvas;

    private float logistic;

    private bool computedLogistic;

    // Start is called before the first frame update
    void Start()
    {
        doubleDamage.SetActive(true);
        doubleDamage.GetComponent<RectTransform>().anchoredPosition = hideVector;
        doubleDamageVector = hideVector;
        doubleDamageActive = false;
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

        if (!doubleDamageActive && levelScript.level >= 1 && logistic > 0.5 && TimerScript.time >= 2 && (scoreCounter.targetScore - scoreCounter.score <= (levelScript.level * 10)))
        {
            // need higher score if double speed already active
            if (DoubleSpeed.doubleSpeedActive)
            {
                if (scoreCounter.targetScore > scoreCounter.score)
                {
                    activateDoubleDamage();
                }
            }
            else
            {
                activateDoubleDamage();
            }
        }
        if (TimerScript.time < 2)
        {
            doubleDamage.GetComponent<RectTransform>().anchoredPosition = hideVector;
            doubleDamageVector = hideVector;
            doubleDamageActive = false;
            bullet.damageBoosted = false;
        }
    }

    private void activateDoubleDamage()
    {
        float screenRight = canvas.GetComponent<RectTransform>().rect.width / 2;
        Vector2 newVector = new Vector2(Random.Range(-screenRight + 50, screenRight - 50), MoveSight.rb.GetComponentInParent<RectTransform>().anchoredPosition.y);

        // make sure doesn't overlap with x2 speed
        if (DoubleSpeed.doubleSpeedActive)
        {
            if (Mathf.Abs(newVector.x - DoubleSpeed.doubleSpeedVector.x) < 120)
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
        doubleDamage.GetComponent<RectTransform>().anchoredPosition = newVector;
        doubleDamageVector = doubleDamage.GetComponent<RectTransform>().anchoredPosition;
        doubleDamageActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("DGT Handgun 4"))
        {
            doubleDamage.GetComponent<RectTransform>().anchoredPosition = hideVector;
            doubleDamageVector = hideVector;
            bullet.damageBoosted = true;
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
        bullet.damageBoosted = false;
    }
}
