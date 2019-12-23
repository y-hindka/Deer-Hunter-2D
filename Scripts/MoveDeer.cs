using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveDeer : MonoBehaviour
{

    public Rigidbody2D rb;

    public GameObject deer;

    private int health = 100;

    public static float spawnDelay = 5f;

    private float timer;
    

    // Start is called before the first frame update
    void Start()
    {
        
        rb.position = new Vector2(1.12f, 0f);
        timer = spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Respawn2();
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        gameObject.SetActive(false);
        scoreCounter.deerCount -= 1;

        scoreCounter.score += 10;

        if (scoreCounter.deerCount < 1)
        {
            Respawn();
        }

        //Respawn(levelScript.level, scoreCounter.score);

        
    }


    void Respawn()
    {
        /*Vector2 deerPos = new Vector2(Random.Range(-8.1f, 8.1f), 0f);
        gameObject.transform.SetPositionAndRotation(deerPos, gameObject.transform.rotation);
        health = 100;
        gameObject.SetActive(true);
        if(timer <= 0)
        {
            Vector2 deerPos2 = new Vector2(Random.Range(-8.1f, 8.1f), 0f);
            Instantiate(deer, deerPos2, deer.transform.rotation);
        }*/

        Vector2 deerPos = new Vector2(Random.Range(-8.1f, 8.1f), 0f);
        gameObject.transform.SetPositionAndRotation(deerPos, gameObject.transform.rotation);
        health = 100;
        gameObject.SetActive(true);

    }

    void Respawn2()
    {
        Vector2 deerPos2 = new Vector2(Random.Range(-8.1f, 8.1f), 0f);
        Instantiate(deer, deerPos2, deer.transform.rotation);
        timer = spawnDelay;
    }
}
