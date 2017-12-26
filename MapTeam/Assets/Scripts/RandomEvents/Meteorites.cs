﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorites : GenericYvant {

    [Header("direction min max")]
    [SerializeField][Range(-1,0)]
    float x_min;
    [SerializeField][Range(-1,0)]
    float y_min;
    [SerializeField][Range(0, 1)]
    float x_max;
    [SerializeField][Range(0, 1)]
    float y_max;
    [Header("vitesse meteorite")]
    [SerializeField]
    float speed;

    private int meteoriteTimeoutLimit = 3;  // destroy the meteorite and the associated shedew if it lives past this amount of seconds
    private Vector3 impactPosition;
    private GridCell impactCell;
    private Vector3 offset = new Vector3(0, 1, 0);

    [SerializeField]
    AudioSource yee;

    private GameObject myShedew;
    public GameObject prefab;

    public override void activate()
    {
        base.activate();
        Debug.Log("meteorite!!!");
    }
    
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, impactPosition, step);

        //No Longer needed with the Timeout? (see at the end)
        /*if (this.transform.position.y < -5)
        {
            Destroy(myShedew);
            this.gameObject.GetComponent<Renderer>().enabled = false;
            yee.Play();
            Destroy(this.gameObject, 2);
        }*/
	}

    public override void spawn(float meteoriteHeight, GameObject map)
    {
        //ALL METEORITES SPAWN AT THE CENTER OF THE MAP VARIANT
        /*int centerX = map.GetComponent<GridMap>().getCenterX();
        int centerY = map.GetComponent<GridMap>().getCenterY();
        this.transform.position = new Vector3(centerX, meteoriteHeight, centerY);*/

        int spawnX = (int)Random.Range(0, map.GetComponent<GridMap>().lengthX);
        int spawnY = (int)Random.Range(0, map.GetComponent<GridMap>().lengthY);
        this.transform.position = new Vector3(spawnX, meteoriteHeight, spawnY);
        //this.transform.rotation = Random.rotation;

        int impactX = (int) Random.Range(0, map.GetComponent<GridMap>().lengthX);
        int impactY = (int)Random.Range(0, map.GetComponent<GridMap>().lengthY);
        impactPosition = new Vector3(impactX, 0, impactY);
        impactCell = map.GetComponent<GridMap>().getCell(impactX, impactY);

        myShedew = Instantiate(prefab, impactPosition + offset, Quaternion.identity) as GameObject;

        StartCoroutine(meteoriteTimeout());
        /*
        Vector3 direction = new Vector3(Random.Range(x_min, x_max), -1, Random.Range(y_min, y_max)); // vecteur direction + celle du ray
        //Vector3 direction = new Vector3(0, -1, 0);    //debug
        GetComponent<Rigidbody>().velocity = direction.normalized * speed;

        RaycastHit hit;//contient tt l'information sur le hit du ray

        if (Physics.Raycast(this.transform.position + direction * 2, direction, out hit, 50.0f)) // if hit
        {
            Vector3 offset = hit.point;
            offset.y = 1.0f; //shedew
            // Debug.Log(hit.collider.gameObject.name);
            myShedew = Instantiate(prefab, offset, Quaternion.identity) as GameObject;
        }
        */
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<player>().loseHp();
            this.gameObject.GetComponent<Renderer>().enabled = false;
            Destroy(myShedew);
            yee.Play();
            Destroy(this.gameObject, 3);
        }
        else if (collision.gameObject.tag == "Terrain")
        {
            impactCell.Cell.GetComponent<gridCellBehavior>().meteorHit();
            this.gameObject.GetComponent<Renderer>().enabled = false;
            Destroy(myShedew);
            yee.Play();
            Destroy(this.gameObject, 3);
        }
    }

    IEnumerator meteoriteTimeout()
    {
        yield return new WaitForSeconds(meteoriteTimeoutLimit);
        Destroy(myShedew);
        Destroy(this.gameObject);
    }
}
