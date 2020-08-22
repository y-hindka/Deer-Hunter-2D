using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bullet : MonoBehaviour
{ 
    public static float bulletSpeed = 20f;

    public Rigidbody2D rb;

    private int[] gunDamages = new int[] { 20, 30, 25, 50, 10, 25, 50, 50 };

    private int[] bulletDamages = new int[] { 20, 30, 25, 50, 10, 25, 50, 50 };

    private int damage;

    private int realIndex;

    private int realBulletIndex;

    public static bool damageBoosted = false;

    // Start is called before the first frame update

    void Start()
    {

        rb.velocity = transform.up * bulletSpeed;
        
    }

    private void Update()
    {
        realIndex = (SelectGunScript.index % 4) * 2;
        realBulletIndex = (SelectBulletScript.bulletIndex % 4) * 2;
        damage = Random.Range((gunDamages[realIndex] + bulletDamages[realBulletIndex]), (gunDamages[realIndex+1] + bulletDamages[realBulletIndex + 1]));
        
        // x2 damage effect
        if (damageBoosted)
        {
            damage *= 2;
        }
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
