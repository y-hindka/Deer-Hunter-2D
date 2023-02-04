using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveSight : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject mud;

    public Canvas canvas;

    private Button left;

    private float speed;

    public static Rigidbody2D rb;

    private Vector2 moveVelocity;

    private bool moveRight = false;

    private bool moveLeft = false;

    private bool mudActive = false;

    public static bool speedBoosted = false;

    private float[] gunSpeeds = new float[] { 0.3f, 0.5f, 0.8f, 0.2f };

    public static bool computedLogistic;

    private float logistic;

    // Start is called before the first frame update
    void Start()
    {
        speed = gunSpeeds[SelectGunScript.index % 4];
        rb = GameObject.Find("DGT Handgun 4").GetComponent<Rigidbody2D>();
        mud.SetActive(false);
        left = GameObject.Find("MoveLeft").GetComponent<Button>();
        logistic = 0;
        rb.GetComponentInParent<RectTransform>().anchoredPosition = new Vector2(rb.GetComponentInParent<RectTransform>().anchoredPosition.x,
            GameObject.Find("Shoot Button").GetComponent<RectTransform>().anchoredPosition.y +
            (rb.GetComponentInParent<RectTransform>().rect.height * rb.GetComponentInParent<RectTransform>().localScale.y));
        computedLogistic = false;
    }

    // Update is called once per frame
    void Update()
    {
        speed = gunSpeeds[SelectGunScript.index % 4];
        moveVelocity = transform.right * speed;

        if (!computedLogistic)
        {
            logistic = computeLogisticFunction() + 0.4f;
            computedLogistic = true;
        }

        // activate/deactivate mud
        if (!mudActive && levelScript.level >= 1 && logistic > 0.5 && TimerScript.time > 2 && CoinScript.coinCount >= 2)
        {
            activateMud();
        }
        else if (TimerScript.time < 2)
        {
            mud.SetActive(false);
            mudActive = false;
        }
        // create mud effect
        if (mudActive && Mathf.Abs(rb.GetComponentInParent<RectTransform>().anchoredPosition.x - mud.GetComponent<RectTransform>().anchoredPosition.x) 
            <= ((mud.GetComponent<RectTransform>().rect.width * mud.GetComponent<RectTransform>().localScale.x) / 2))
        {
            moveVelocity *= 0.5f;
        }
        
        // x2 speed boost
        if (speedBoosted)
        {
            moveVelocity *= 2;
        }

        if (moveRight && TimerScript.keepTiming)
        {
            MoveRight();
        }
        /*else if (Input.touchCount > 0 && TimerScript.keepTiming)
        {
            if (Input.GetTouch(0).deltaPosition.x > 0)
            {
                MoveRight();
            }
        }*/
        else if (moveLeft && TimerScript.keepTiming)
        {
            MoveLeft();
        }
        /*else if (Input.touchCount > 0 && TimerScript.keepTiming)
        {
            if (Input.GetTouch(0).deltaPosition.x < 0)
            {
                MoveLeft();
            }
        }*/
    }

    private void activateMud()
    {
        float screenRight = canvas.GetComponent<RectTransform>().rect.width / 2;
        mud.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-screenRight, screenRight), rb.GetComponentInParent<RectTransform>().anchoredPosition.y);
        mud.SetActive(true);
        mudActive = true;
    }

    public void MoveRight()
    {
        float screenRight = canvas.GetComponent<RectTransform>().rect.width / 2;
        screenRight -= 50;
        if (rb.GetComponentInParent<RectTransform>().anchoredPosition.x <= screenRight)
        {
            rb.MovePosition(rb.position + moveVelocity);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        moveRight = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        moveRight = false;
    }

    public void MoveLeft()
    {
        float screenLeft = (canvas.GetComponent<RectTransform>().rect.width / 2) * -1;
        screenLeft += 50;
        if (rb.GetComponentInParent<RectTransform>().anchoredPosition.x >= screenLeft)
        {
            rb.MovePosition(rb.position + (moveVelocity * -1));

        }
    }

    public void leftOnPointerDown()
    {
        moveLeft = true;
    }

    public void leftOnPointerUp()
    {
        moveLeft = false;
    }

    private float computeLogisticFunction()
    {
        return (1 / (1 + Mathf.Exp(-1 * Random.Range(-3, 3))));
    }
}