using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Transform timeOfDayTracker;
    private Transform sunset;
    private Transform moon;
    private Transform sunrise;
    
    // Start is called before the first frame update
    void Start()
    {
        timeOfDayTracker = GameObject.Find("TimeOfDayTracker").GetComponent<Transform>();
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
        sunset.position -= new Vector3(0, timeOfDayTracker.position.x, 0);

        if (timeOfDayTracker.position.x > 1000f && timeOfDayTracker.position.x < 2000f)
        {
            moon.position += new Vector3(0, timeOfDayTracker.position.x, 0);
        }

        if (timeOfDayTracker.position.x > 2000f && timeOfDayTracker.position.x < 3000f)
        {
            moon.position -= new Vector3(0, timeOfDayTracker.position.x, 0);
        }

        if (timeOfDayTracker.position.x > 3000f)
        {
            sunrise.position += new Vector3(0, timeOfDayTracker.position.x, 0);
        }
    }
}