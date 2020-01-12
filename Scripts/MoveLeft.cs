using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveLeft : MonoBehaviour, IPointerDownHandler, IPointerUpHandler

{

    public float speed;

    private Vector2 moveVelocity;

    private bool moveLeft = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        moveLeft = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        moveLeft = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveVelocity = -transform.right * speed;
        if (moveLeft && TimerScript.keepTiming)
        {
            Left();
        }
    }

    public void Left()
    {
        if (MoveSight.rb.position.x >= -11.0)
        {
            MoveSight.rb.MovePosition(MoveSight.rb.position + moveVelocity);

        }

        else if (MoveSight.rb.position.x < -11)
        {
            MoveSight.rb.position = new Vector2(-11.0f, -3.68f);
        }
    }
}
