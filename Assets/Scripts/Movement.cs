using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int movementSpeed; 
    public bool isWindmill; //If true, the object will rotate instead of move

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
        else
        {
            //Movement
            transform.position += new Vector3(movementSpeed * Time.deltaTime, 0f, 0f);
        }
    }
}
