using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveSight : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public float speed;

    public static Rigidbody2D rb;

    private Vector2 moveVelocity;

    //private Vector3 touchPosition;

    //private Vector3 gunPosition;

    private bool moveRight = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        moveVelocity = transform.right * speed;
        if (moveRight)
        {
            MoveRight();
        }
       
        
    }



    public void MoveRight()
    {
        if(rb.position.x <= 11.0)
        {
            rb.MovePosition(rb.position + moveVelocity);
        }

        else if(rb.position.x > 11)
        {
            rb.position = new Vector2(11.0f, -3.68f);
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


    /*void updateVelocity()
    {
        Touch touch = Input.GetTouch(0);
        touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
        gunPosition = new Vector3(rb.position.x, rb.position.y, 0f);
        if (touchPosition.x > 0)
        {
            touchPosition.x = 1;
        }
        else if (touchPosition.x < 0)
        {
            touchPosition.x = -1;
        }
        touchPosition.z = 0;
        touchPosition.y = 0;
        moveVelocity = touchPosition * speed;
        print(Mathf.Abs(touchPosition.x - gunPosition.x));
        if(Mathf.Abs(touchPosition.x - gunPosition.x) > 2)
        {
            Move();
        }


    }*/
}