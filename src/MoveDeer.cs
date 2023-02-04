using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveDeer : MonoBehaviour
{
    public Rigidbody2D rb;

    private float deerSpeed;

    public GameObject deer;

    public Canvas canvas;

    public static int deerCount = 0;

    private int health = 100;

    public static Vector2 deerPos;

    //public static float spawnDelay = 5f;

    private float timer;

    private float initialSpawnCount = 0;

    private bool deerMoving = false;

    public static bool bigDeerUsed = false;

    private bool bigDeerInUse = false;

    private bool deerMovingLeft = false;
    

    // Start is called before the first frame update
    void Start()
    {
        if (deerCount == 0)
        {
            float screenRight = canvas.GetComponent<RectTransform>().rect.width / 2;
            deerPos = new Vector2(Random.Range(-screenRight + 75, screenRight - 75), 0f);
            rb.MovePosition(deerPos);
            GameObject newDeer = Instantiate(deer, rb.position, Quaternion.identity);
            newDeer.GetComponent<RectTransform>().Find("Slider").GetComponent<Slider>().value = health;
            newDeer.GetComponent<MoveDeer>().canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            newDeer.transform.parent = canvas.GetComponent<RectTransform>();
            newDeer.transform.SetAsFirstSibling();
            deerCount++;
        }
    }

    void Update()
    {
        if (!TimerScript.keepTiming && deerMoving)
        {
            rb.velocity = new Vector2(0, 0);
            deerMoving = false;
        }
        else if (TimerScript.keepTiming)
        {
            int dividedFive = levelScript.level / 5;
            if (!deerMovingLeft)
            {
                deerSpeed = dividedFive * 2;
            }
            else
            {
                deerSpeed = dividedFive * 2 * -1;
            }

            if (!deerMoving)
            {
                rb.velocity = new Vector2(deerSpeed, 0);
                deerMoving = true;
            }

            float screenRight = canvas.GetComponent<RectTransform>().rect.width / 2;
            if (deer.GetComponent<RectTransform>().anchoredPosition.x < -screenRight + 50 && deerMovingLeft)
            {
                //rb.velocity = new Vector2(deerSpeed, 0);
                Vector2 newPos = new Vector2(screenRight + 10, deer.GetComponent<RectTransform>().anchoredPosition.y);
                deer.GetComponent<RectTransform>().anchoredPosition = newPos;
            }
            else if (deer.GetComponent<RectTransform>().anchoredPosition.x > screenRight - 50 && !deerMovingLeft)
            {
                //rb.velocity = new Vector2(-deerSpeed, 0);
                Vector2 newPos = new Vector2(-screenRight - 10, deer.GetComponent<RectTransform>().anchoredPosition.y);
                deer.GetComponent<RectTransform>().anchoredPosition = newPos;
            }
        }
    }


    public void TakeDamage(int damage)
    {
        if (TimerScript.time >= 0.2)
        {
            health -= damage;
            deer.GetComponent<RectTransform>().Find("Slider").GetComponent<Slider>().value = health;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        deerCount--;

        // handle scoring
        if (bigDeerInUse)
        {
            scoreCounter.score += 30;
        }
        else
        {
            scoreCounter.score += 10;
        }

        Respawn();

        
    }


    void Respawn()
    {
        if (bigDeerInUse)
        {
            decreaseDeerScale();
        }
        float screenRight = canvas.GetComponent<RectTransform>().rect.width / 2;
        Vector2 newPos = new Vector2(Random.Range(-screenRight + 75, screenRight - 75), 0f);
        if (Mathf.Abs(newPos.x - deerPos.x) < (deer.GetComponent<RectTransform>().rect.width * deer.GetComponent<RectTransform>().localScale.x))
        {
            if (newPos.x < 0)
            {
                newPos.x += 150;
            }
            else
            {
                newPos.x -= 150;
            }
        }
        deerPos = newPos;
        deer.GetComponent<RectTransform>().anchoredPosition = deerPos;
        health = 100;
        if (levelScript.level >= 10 && levelScript.level % TimerScript.bigDeerRandom == 0 && !bigDeerUsed)
        {
            Vector3 scale = deer.GetComponent<RectTransform>().localScale;
            scale.Scale(new Vector3(2, 2, 1));
            deer.GetComponent<RectTransform>().localScale = scale;
            deer.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPos.x, newPos.y + 20f);
            health = 400;
            bigDeerUsed = true;
            bigDeerInUse = true;
            deer.GetComponent<RectTransform>().Find("Slider").GetComponent<Slider>().maxValue = health;
        }
        deer.GetComponent<RectTransform>().Find("Slider").GetComponent<Slider>().value = health;
        deer.transform.SetAsFirstSibling();
        rb.velocity = new Vector2(deerSpeed, 0);
        deerMovingLeft = !deerMovingLeft;
        deerMoving = false;
        deerCount++;
    }

    private void decreaseDeerScale()
    {
        Vector3 scale = deer.GetComponent<RectTransform>().localScale;
        scale.Scale(new Vector3(0.5f, 0.5f, 1));
        deer.GetComponent<RectTransform>().localScale = scale;
        deer.GetComponent<RectTransform>().Find("Slider").GetComponent<Slider>().maxValue = 100;

        bigDeerInUse = false;
    }
}
