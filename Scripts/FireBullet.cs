using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float speed;

    public GameObject bulletEmitter;

    public GameObject Bullet;

    public static int allowShoot = 0;

    private Vector3 touchPosition;

    private Vector3 gunPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 




    }

    public void Shoot()
    {
        if (allowShoot == 0)
        {
            //instaniate bullet
            GameObject temporaryBulletHandler;
            temporaryBulletHandler = Instantiate(Bullet, bulletEmitter.transform.position, bulletEmitter.transform.rotation) as GameObject;

            Rigidbody2D temporaryRB;
            temporaryRB = temporaryBulletHandler.GetComponent<Rigidbody2D>();

            //add speed
            temporaryRB.AddForce(transform.forward * speed);

            //destroy bullet
            Destroy(temporaryBulletHandler, 0.5f);
        }

    }

}