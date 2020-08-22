using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    private float speed = 100f;

    public GameObject handgun;

    public Sprite gun1;

    public Sprite gun2;

    public Sprite gun3;

    public Sprite gun4;

    private Sprite[] spriteArray;

    public Sprite bullet1;

    public Sprite bullet2;

    public Sprite bullet3;

    public Sprite bullet4;

    private Sprite[] bulletArray;

    public GameObject bulletEmitter;

    public GameObject Bullet;

    public AudioClip gunShot;

    private AudioSource audioSrc;

    public static int allowShoot = 0; // 0 if shooting allowed, 1 otherwise

    private Vector3 touchPosition;

    private Vector3 gunPosition;

    private float[] bulletSpeeds = new float[] { 5, 7, 10, 3 }; // 50, 70, 100, 30 // 35, 55, 100, 20

    private int soundOn;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = handgun.GetComponent<AudioSource>();
        audioSrc.clip = gunShot;
        spriteArray = new Sprite[] { gun1, gun2, gun3, gun4 };
        bulletArray = new Sprite[] { bullet1, bullet2, bullet3, bullet4 };
        soundOn = PlayerPrefs.GetInt("Sound");

        // set gun sprite
        if (PlayerPrefs.HasKey("Gun"))
        {
            handgun.GetComponent<SpriteRenderer>().sprite = spriteArray[PlayerPrefs.GetInt("Gun")];
        }
        else
        {
            handgun.GetComponent<SpriteRenderer>().sprite = gun1;
        }

        // set bullet sprite and speed
        if (PlayerPrefs.HasKey("Bullet"))
        {
            Bullet.GetComponent<SpriteRenderer>().sprite = bulletArray[PlayerPrefs.GetInt("Bullet")];
            bullet.bulletSpeed = bulletSpeeds[PlayerPrefs.GetInt("Bullet")];
        }
        else
        {
            Bullet.GetComponent<SpriteRenderer>().sprite = bullet1;
            bullet.bulletSpeed = bulletSpeeds[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (SelectGunScript.selectedGun != null)
        {
            handgun.GetComponent<SpriteRenderer>().sprite = SelectGunScript.selectedGun;
        }*/
        /*if (SelectBulletScript.selectedBullet != null)
        {
            Bullet.GetComponent<SpriteRenderer>().sprite = SelectBulletScript.selectedBullet;
        }*/
        if (!TimerScript.keepTiming)
        {
            allowShoot = 1;
        }
        else
        {
            allowShoot = 0;
        }
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


            // play sound
            if (soundOn == 1)
            {
                audioSrc.Play();
            }

            //destroy bullet
            Destroy(temporaryBulletHandler, 0.5f);
        }

    }

}