using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AreaCheck areaCheck;

    [SerializeField] private Transform playerTransform;
    private float playerInitialXPosition;
    [SerializeField] private Transform forest1, wheat, ocean, forest2;
    [SerializeField] private Transform oceanMid, oceanEnd;
    [SerializeField] private Transform sunset, moon, sunrise;
    [SerializeField] private Light2D sunsetLight, moonLight, sunriseLight, globalSkyLight;
    private float oceanMidpoint = 500f;
    public Animator oceanAnimator;

    [HideInInspector]
    public bool oceanPause1 = true, oceanRegular, oceanPause2;

    public int petalCount;
    [SerializeField] private TMP_Text petalText;

    //light intensity variables
    public float globalSkyLightSpeed = 1000f, skyLightMaxIntensity = 0.9f, skyLightMidPoint = 0;
    private float moonLightIntensitySpeed = .02f, moonLightMaxIntensity = .05f;
    private float sunsetIntensitySpeed = .01f;
    private float sunriseIntensitySpeed = .02f, sunriseMaxIntensity = .8f;

    //light color variables
    private Color sunriseColor = Color.white, sunsetColor = Color.red, moonColor = new Color(129f, 217f, 255f);
    public float sunsetColorSpeed = 10f, sunriseColorSpeed = .04f, moonColorSpeed = .03f;

    //celestial position variables
    public float sunsetSpeed = 1000f, sunriseSpeed = 0.01f, moonSpeed = 0.01f;
    private float moonRestingPosition = 12f, sunriseRestingPoition = 16f;

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
            sunset.position -= new Vector3(0, sunsetSpeed * Time.deltaTime, 0);
            
            //make sky gradually more orange and less bright
            sunsetLight.color = Color.Lerp(sunsetLight.color, sunsetColor, sunsetColorSpeed * Time.deltaTime);
            sunsetLight.intensity = Mathf.Lerp(sunsetLight.intensity, 0, sunsetIntensitySpeed * Time.deltaTime);

            //lower global sky light intensity to .3 using lerp
            globalSkyLight.intensity = Mathf.Lerp(globalSkyLight.intensity, skyLightMidPoint, globalSkyLightSpeed * Time.deltaTime);
        }

        if (areaCheck.ocean)
        {
            //Start ocean animation (after you've passed the transition point)
            if (playerTransform.position.x > oceanMid.transform.position.x)
            {
                oceanRegular = true;
                oceanPause1 = false;
            }

            //End the ocean animation at the end of the section
            if (playerTransform.position.x > oceanEnd.transform.position.x)
            {
                oceanPause2 = true;
                oceanRegular = false;
            }

            //Increase moon to the point where it stays in the sky
            if (playerTransform.position.x <= oceanMidpoint)
            {
                if (moon.position.y < moonRestingPosition)
                {
                    moon.position += new Vector3(0, moonSpeed, 0);
                }
                moonLight.color = Color.Lerp(moonLight.color, moonColor, moonColorSpeed * Time.deltaTime);
                moonLight.intensity = Mathf.Lerp(moonLight.intensity, moonLightMaxIntensity, moonLightIntensitySpeed * Time.deltaTime);

                //lower global sky light intensity to 0 using lerp
                globalSkyLight.intensity = Mathf.Lerp(globalSkyLight.intensity, 0, globalSkyLightSpeed * Time.deltaTime);
            }

            //Lower the moon
            if (playerTransform.position.x > oceanMidpoint)
            {
                moon.position -= new Vector3(0, moonSpeed, 0);
                moonLight.color = Color.Lerp(moonLight.color, sunriseColor, moonColorSpeed * Time.deltaTime);
                moonLight.intensity = Mathf.Lerp(moonLight.intensity, 0, moonLightIntensitySpeed * Time.deltaTime);

                //raise global sky light intensity to .3f using lerp
                globalSkyLight.intensity = Mathf.Lerp(globalSkyLight.intensity, skyLightMidPoint, globalSkyLightSpeed * Time.deltaTime);
            }
        }

        if (areaCheck.forest2)
        {
            //make sky gradually more white
            sunriseLight.color = Color.Lerp(sunriseLight.color, sunriseColor, sunriseColorSpeed * Time.deltaTime);
            sunriseLight.intensity = Mathf.Lerp(sunriseLight.intensity, sunriseMaxIntensity, sunriseIntensitySpeed * Time.deltaTime);

            //raise global sky light intensity to .7f using lerp
            globalSkyLight.intensity = Mathf.Lerp(globalSkyLight.intensity, skyLightMaxIntensity, globalSkyLightSpeed * Time.deltaTime);

            if (sunrise.position.y < sunriseRestingPoition)
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