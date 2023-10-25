using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int movementSpeed; 
    public bool isWindmill; //If true, the object will rotate instead of move
    public bool isFish; //If true, object will move in an arc
    public float arcHeight; //How high the arc is

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
    }
}
