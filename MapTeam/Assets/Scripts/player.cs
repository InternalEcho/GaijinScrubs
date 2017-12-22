﻿    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

    //Basic player parameters
    public float playerSpeed;
    public float boostSpeed;
    public float hp;
    public GameObject gridCell;
    public Color playerColor;
    public Color gridColor;
    public Color[] playerColors;
    public string playerNumber = "1";
    public float decreaseSpeedDuration = 1.0f;

    //Movement/Rotation parameters
    private float deltaX;
    private float deltaY;
	public GameObject gun;

    //Power-up parameters
    public float powerUpDuration;
    public bool activeShield;
        //Projectile Power-up
    public float bulletSpeed;
    private int numberStunProjectile;
    public GameObject bullet;
    public Transform bulletEmitter;

    //Obsolete?
    //private float boostCooldown

    // Use this for initialization
    void Start () {
        activeShield = false;
		numberStunProjectile = 0;
        hp = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
        move();
		rotate();
        shoot();
    }

	void rotate()
	{

		deltaX = Input.GetAxis("RotateX"+playerNumber);
        deltaY = Input.GetAxis("RotateY"+playerNumber); 

		Vector3 point = new Vector3 (deltaX, 0, deltaY);
	
		Vector3 pointToLook = transform.position + point;
		Vector3 currentLook = transform.position + transform.GetChild(0).forward;

		gun.transform.LookAt(Vector3.Lerp(currentLook, pointToLook, .5f));

        //		Debug.Log ("X:" + deltaX);
        //		Debug.Log ("Y:" + deltaY);
    }



    void shoot()
    {
        if (numberStunProjectile != 0)
        {
            if (Input.GetButtonDown("Fire"+playerNumber))
            {
                GameObject go = (GameObject)Instantiate(bullet, bulletEmitter.position, bulletEmitter.rotation);
                go.GetComponent<Rigidbody>().AddForce(bulletEmitter.forward * bulletSpeed);
                numberStunProjectile -= 1;
            }
        }
    }

    void move()
    {
        //transform.Translate(Input.GetAxisRaw("Horizontal") * playerSpeed, 0, Input.GetAxisRaw("Vertical") * playerSpeed, Space.World);
        if (Input.GetAxisRaw("Horizontal" + playerNumber) != 0 && Input.GetAxisRaw("Vertical" + playerNumber) != 0)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(Input.GetAxisRaw("Horizontal" + playerNumber) * playerSpeed * 3 / 4, 0, Input.GetAxisRaw("Vertical" + playerNumber) * playerSpeed * 3 / 4);
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().AddForce(Input.GetAxisRaw("Horizontal" + playerNumber) * playerSpeed, 0, Input.GetAxisRaw("Vertical" + playerNumber) * playerSpeed);
        }

    }

    public void becomeXS()
    {
        this.transform.localScale = new Vector3(.5f,.5f,.5f);
        StartCoroutine(PowerUpUptime(1));
    }

	public void becomeXL()
	{
		this.transform.localScale = new Vector3(2.0f,2.0f,2.0f);
		StartCoroutine(PowerUpUptime(2));
	}

    public void increaseSpeed()
    {
        playerSpeed *= 2;
        StartCoroutine(PowerUpUptime(3));
    }

    public void stunProjectile()    // can store many projectiles? will need a visual indicator
    {
        numberStunProjectile += 3;
    }
	public void gainShield()
	{
        activeShield = true;
	}
    IEnumerator PowerUpUptime(int powerUpType)  //1 = becomeXS; 2 = increaseSpeed;
    {
        yield return new WaitForSeconds(powerUpDuration);

        switch (powerUpType)
        {
            case 1:
                Debug.Log("Size back to normal");   //debug
                this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                break;
			case 2:
				Debug.Log("Size back to normal");   //debug
				this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				break;
            case 3:
                Debug.Log("Speed down to normal");  //debug
                playerSpeed /= 2;
                break;
            default:
                Debug.Log("PowerUp Error");
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Renderer>().material.color != playerColor
            && collision.gameObject.GetComponent<Renderer>().material.color != gridColor)
        {
            Debug.Log(playerNumber);
            decreaseSpeed();
        }
    }

    public void decreaseSpeed()
    {
        playerSpeed /= 2;
        StartCoroutine(decreaseSpeedTime());
    }

    IEnumerator decreaseSpeedTime()
    {
        yield return new WaitForSeconds(decreaseSpeedDuration);
        playerSpeed *= 2;
    }
}

   