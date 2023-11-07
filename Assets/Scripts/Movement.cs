using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int movementSpeed; 
    public bool isWindmill; //If true, the object will rotate instead of move
    public bool isFish; //If true, object will move in an arc
    public float arcHeight; //How high the arc is

    public float distanceToPlayer; //Make positive if entering from right, negative if entering from left

    // Update is called once per frame
    void Update()
    {
        if (isWindmill)
        {
            //Rotation
            transform.Rotate(0f, 0f, movementSpeed * Time.deltaTime);
        }
        else if (isFish)
        {
            //Movement
            transform.position += new Vector3(movementSpeed * Time.deltaTime, Mathf.Sin(Time.time) * arcHeight, 0f);
            //Rotation
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time) * arcHeight * 300);
        }
        else
        {
            //Movement
            transform.position += new Vector3(movementSpeed * Time.deltaTime, 0f, 0f);
        }

        ActivatePrefab();
    }

    //if object collides with player, increase player speed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWindmill && collision.gameObject.tag == "Player")
        {
            //Call the speed up function in the player script
        }
    }

    private void ActivatePrefab()
    {
        //If player is within a certain distance of the object, activate the prefab
        if ((transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x) < distanceToPlayer)
        {
            //Activate the prefab
            gameObject.SetActive(true);
        }
    }
}