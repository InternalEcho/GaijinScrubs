﻿    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

    [Header("Basic player parameters")]
    public float defaultPlayerSpeed = 0.5f;
    public float playerSpeed;
    public float playerDecreasedSpeed;
    public float hp = 1.0f; // public for debug purposes but put private later
    public GameObject gridCell;
    public Color playerColor;
    public Color gridColor;
    public Color[] playerColors;
    public string playerNumber = "1";
    public float decreaseSpeedDuration = 3.0f;

    [Header("Movement/Rotation parameters")]
    private float deltaX;
    private float deltaY;
	public GameObject gun;

    [Header("Projectile Power-up")]
    public float bulletSpeed;
    public int numberStunProjectile;
    public GameObject bullet;
    public Transform bulletEmitter;

    // Use this for initialization
    void Start () {
        //activeShield = false;
        playerSpeed = defaultPlayerSpeed;
        playerDecreasedSpeed = defaultPlayerSpeed / 2;
		numberStunProjectile = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
		rotate();
        move();
        shoot();
    }

	void rotate()
	{
        deltaX = Input.GetAxis("RotateX"+playerNumber);
        deltaY = Input.GetAxis("RotateY"+playerNumber); 

		Vector3 point = new Vector3 (deltaX, 0, deltaY);
     //   Debug.Log(point);
	
		Vector3 pointToLook = transform.position + point;
		Vector3 currentLook = transform.position + transform.GetChild(0).forward;

		gun.transform.LookAt(Vector3.Lerp(currentLook, pointToLook, .5f));
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
        float x = Input.GetAxisRaw("Horizontal" + playerNumber);
        float z = Input.GetAxisRaw("Vertical" + playerNumber);
        Vector3 direction = new Vector3(x, 0f, z);
        this.transform.Translate(direction * playerSpeed, Space.World);
    }

    public void decreaseSpeed()
    {
        playerSpeed = playerDecreasedSpeed;
        StartCoroutine(decreaseSpeedTime());
    }

    IEnumerator decreaseSpeedTime()
    {
        yield return new WaitForSeconds(decreaseSpeedDuration);
        playerSpeed = defaultPlayerSpeed;
    }

    public void loseHp()
    {
        if (!this.GetComponent<playerPowerUpManager>().activeShield)    // if shield isn't up
        {
            hp--;
        }
        else
        {
            this.GetComponent<playerPowerUpManager>().activeShield = false; // else take down shield
        }
    }
}

   