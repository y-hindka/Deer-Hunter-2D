using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveDeer : MonoBehaviour
{

    public Rigidbody2D rb;

    public GameObject deer;

    public static int deerCount = 0;

    private int health = 100;

    private Vector2 deerPos;

    public static float spawnDelay = 5f;

    private float timer;

    private float initialSpawnCount = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        deerPos = new Vector2(Random.Range(-8.1f, 8.1f), 0f);
        rb.MovePosition(deerPos);
        timer = spawnDelay;
    }

    IEnumerator initialSpawn()
    {
        while (deerCount < 1)
        {
            Instantiate(deer, rb.position, Quaternion.identity);
            deerCount++;
            initialSpawnCount++;
            yield return new WaitForSeconds(0.01f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (initialSpawnCount == 0)
        {
            StartCoroutine(initialSpawn());
        }
        if (TimerScript.keepTiming)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            if (deerCount < levelScript.level) { Respawn2(); }
            else { timer = spawnDelay; }
        }
        deerCount = GameObject.FindGameObjectsWithTag("deer").Length;
    }

    private void OnDrawGizmos()
    {
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
        deerCount -= 1;

        scoreCounter.score += 10;

        if (deerCount < 1)
        {
            Respawn();
        }

        
    }


    void Respawn()
    {
        Vector2 deerPos = new Vector2(Random.Range(-8.1f, 8.1f), 0f);
        gameObject.transform.SetPositionAndRotation(deerPos, Quaternion.identity);
        health = 100;
        gameObject.SetActive(true);
    }

    void Respawn2()
    {
        deerPos.x = Random.Range(-8.1f, 8.1f);
        while (isSpotAvailable(deerPos) == false)
        {
            deerPos.x = Random.Range(-8.1f, 8.1f);
        }
        Instantiate(deer, deerPos, Quaternion.identity);
        timer = spawnDelay;
    }

    bool isSpotAvailable(Vector2 position)
    {
        Collider2D[] intersections = Physics2D.OverlapCircleAll(position, 3f);
        return intersections.Length == 0;
    }
}
