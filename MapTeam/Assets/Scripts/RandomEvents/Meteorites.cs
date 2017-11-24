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

    [SerializeField]
    AudioSource yee;

    private GameObject myShedew;
    private bool flag = true;
    public GameObject prefab;

    public override void activate()
    {
        base.activate();
        Debug.Log("meteorite!!!");
    }
    /*
    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Ground")
        {
          //  Destroy(myShedew);
          //  Destroy(this.gameObject);
        }
    }
    */
	// Use this for initialization
	void Start () {
        Vector3 direction = new Vector3(Random.Range(x_min, x_max), -1, Random.Range(y_min, y_max)); // vecteur direction + celle du ray
        GetComponent<Rigidbody>().velocity = direction.normalized * speed;

        RaycastHit hit;//contient tt l'information sur le hit du ray

        if(Physics.Raycast(this.transform.position + direction * 2, direction, out hit, 50.0f)) // if hit
        {
            Vector3 offset = hit.point;
            offset.y = 30.0f; //shedew
            // Debug.Log(hit.collider.gameObject.name);
            myShedew = Instantiate(prefab, offset, Quaternion.identity) as GameObject;
        }



	}
	
	// Update is called once per frame
	void Update () {
        
        if (this.transform.position.y < 0 && flag)
        {
            flag = false;
            Destroy(myShedew);
            this.gameObject.GetComponent<Renderer>().enabled = false;
            yee.Play();
            Destroy(this.gameObject, 2);
        }
	}
}
