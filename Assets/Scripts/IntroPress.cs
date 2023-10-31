using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPress : MonoBehaviour
{
    public bool startScene = false;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If any button is pressed, rotate about the z-axis to -30 degrees
        if (Input.anyKey)
        {
            transform.rotation = Quaternion.Euler(0, 0, -30);
            timer += Time.deltaTime;
            //If any key is held for 3 seconds, set startScene to true
            if (timer > 3 && !startScene)
            {
                startScene = true;
            }
        }
        else
        {
            timer = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 2);
        }
    }
}
