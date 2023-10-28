using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Transform playerTransform;
    private Transform sunset;
    private Transform moon;
    private Transform sunrise;
    
    // Start is called before the first frame update
    void Start()
    {
        //find the player tag
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        sunset = GameObject.Find("Sunset").GetComponent<Transform>();
        moon = GameObject.Find("Moon").GetComponent<Transform>();
        sunrise = GameObject.Find("Sunrise").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        SkyUpdate();
    }

    private void SkyUpdate()
    {
        sunset.position -= new Vector3(0, playerTransform.position.x, 0);

        if (playerTransform.position.x > 1000f && playerTransform.position.x < 2000f)
        {
            moon.position += new Vector3(0, playerTransform.position.x, 0);
        }

        if (playerTransform.position.x > 2000f && playerTransform.position.x < 3000f)
        {
            moon.position -= new Vector3(0, playerTransform.position.x, 0);
        }

        if (playerTransform.position.x > 3000f)
        {
            sunrise.position += new Vector3(0, playerTransform.position.x, 0);
        }
    }
}