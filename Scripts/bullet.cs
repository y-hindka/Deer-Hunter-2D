using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bullet : MonoBehaviour
{

    public float speed = 20f;

    public Rigidbody2D rb;

    private int damage;

    // Start is called before the first frame update

    void Start()
    {

        rb.velocity = transform.up * speed;
        
    }

    private void Update()
    {
        damage = Random.Range(30, 70);
    }

    void OnTriggerEnter2D(Collider2D collisionInfo)
    {
        Destroy(gameObject);
        MoveDeer deer = collisionInfo.GetComponent<MoveDeer>();
        if (deer != null)
        {
            deer.TakeDamage(damage);
        }

    }



}
