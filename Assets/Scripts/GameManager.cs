using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    private AreaCheck areaCheck;

    private Transform playerTransform;
    private float playerInitialXPosition;
    private Transform forest1, wheat, ocean, forest2;
    
    private Transform sunset, moon, sunrise;
    private Light2D sunsetLight, moonLight, sunriseLight, globalSkyLight;
    private Color sunriseColor = Color.white, sunsetColor = Color.red, moonColor = new Color(129f, 217f, 255f);
    public float sunsetColorSpeed, sunriseColorSpeed, moonColorSpeed;
    public float sunsetSpeed = 0.01f, sunriseSpeed = 0.01f, moonSpeed = 0.01f;

    private float moonRestingPosition = 12f, sunriseRestingPosition = 16f;
    private float oceanMidpoint = 500f;
    public float globalSkyLightSpeed = 0.1f, skyLightMaxIntensity = 0.7f, skyLightMidPoint = 0.4f;
    public Animator oceanAnimator;

    [HideInInspector]
    public bool oceanPause1 = true, oceanRegular, oceanPause2;

    public int petalCount;
    private TMP_Text petalText;
    
    // Start is called before the first frame update
    void Start()
    {
        //find the player tag
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        //find the sun and moon and their lights
        sunset = GameObject.Find("Sunset").GetComponent<Transform>();
        sunsetLight = GameObject.Find("Sunset Light").GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        moon = GameObject.Find("Moon").GetComponent<Transform>();
        moonLight = GameObject.Find("Moon Light").GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        sunrise = GameObject.Find("Sunrise").GetComponent<Transform>();
        sunriseLight = GameObject.Find("Sunrise Light").GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        //find the global sky light
        globalSkyLight = GameObject.Find("Global Sky Light").GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        //turn off moon and sunrise lights
        moonLight.gameObject.SetActive(false);
        sunriseLight.gameObject.SetActive(false);

        //sunrise goes red to white
        sunriseLight.color = sunsetColor;

        //initialize area check
        areaCheck = GameObject.FindWithTag("Player").GetComponent<AreaCheck>();
        forest1 = GameObject.Find("Forest 1").GetComponent<Transform>();
        wheat = GameObject.Find("Wheat").GetComponent<Transform>();
        ocean = GameObject.Find("Ocean").GetComponent<Transform>();
        forest2 = GameObject.Find("Forest 2").GetComponent<Transform>();
        petalText = GameObject.Find("Petal Text").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        SkyUpdate();
        CelestialPositionUpdate();
        petalText.text = "Petals: " + petalCount.ToString();
    }

    private void SkyUpdate()
    {
        if (areaCheck.forest1)
        {
            oceanAnimator.SetBool("Pause1", true);
            oceanAnimator.SetBool("Pause2", false);
        }

        if (areaCheck.wheat)
        {
            sunset.position -= new Vector3(0, sunsetSpeed, 0);
            //make sky gradually more orange
            sunsetLight.color = Color.Lerp(sunsetLight.color, sunsetColor, sunsetColorSpeed * Time.deltaTime);

            //lower global sky light intensity to .4 using lerp
            globalSkyLight.intensity = Mathf.Lerp(globalSkyLight.intensity, skyLightMidPoint, globalSkyLightSpeed);
        }

        if (areaCheck.ocean)
        {
            sunsetLight.gameObject.SetActive(false);
            moonLight.gameObject.SetActive(true);

            //Start ocean animation (after you've passed the transition point)
            if (playerTransform.position.x > GameObject.Find("Ocean (2)").transform.position.x)
            {
                oceanRegular = true;
                oceanPause1 = false;
            }

            //End the ocean animation at the end of the section
            if (playerTransform.position.x > GameObject.Find("Ocean End").transform.position.x)
            {
                oceanPause2 = true;
                oceanRegular = false;
            }

            //Increase moon to the point where it stays in the sky
            if (moon.position.y < moonRestingPosition && playerTransform.position.x <= oceanMidpoint)
            {
                moon.position += new Vector3(0, moonSpeed, 0);
                moonLight.color = Color.Lerp(moonLight.color, moonColor, moonColorSpeed * Time.deltaTime);

                //lower global sky light intensity to 0 using lerp
                globalSkyLight.intensity = Mathf.Lerp(globalSkyLight.intensity, 0, globalSkyLightSpeed);
            }

            //Lower the moon
            if (playerTransform.position.x > oceanMidpoint)
            {
                moon.position -= new Vector3(0, moonSpeed, 0);
                moonLight.color = Color.Lerp(moonLight.color, sunriseColor, moonColorSpeed * Time.deltaTime);

                //raise global sky light intensity to .4f using lerp
                globalSkyLight.intensity = Mathf.Lerp(globalSkyLight.intensity, skyLightMidPoint, globalSkyLightSpeed);
            }
        }

        if (areaCheck.forest2)
        {
            moonLight.gameObject.SetActive(false);
            sunriseLight.gameObject.SetActive(true);

            //make sky gradually more white
            sunriseLight.color = Color.Lerp(sunriseLight.color, sunriseColor, sunriseColorSpeed * Time.deltaTime);

            //raise global sky light intensity to .7f using lerp
            globalSkyLight.intensity = Mathf.Lerp(globalSkyLight.intensity, skyLightMaxIntensity, globalSkyLightSpeed);

            if (sunrise.position.y < sunriseRestingPosition)
            {
                sunrise.position += new Vector3(0, sunriseSpeed, 0);
            }

            moon.position -= new Vector3(0, moonSpeed, 0);
        }
    }

    private void CelestialPositionUpdate()
    {
        sunset.position = new Vector3(playerTransform.position.x - 5, sunset.position.y, sunset.position.z);
        moon.position = new Vector3(playerTransform.position.x - 1, moon.position.y, moon.position.z);
        sunrise.position = new Vector3(playerTransform.position.x + 5, sunrise.position.y, sunrise.position.z);
    }
}