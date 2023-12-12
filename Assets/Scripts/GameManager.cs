using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using UnityEngine.Rendering.Universal;
using System;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    private class MoveSpeedChangeParameters
    {
        public float baseSpeed;
        public float maxSpeed;
        public float maxBoostedSpeed;
        public float speedDecayRate;
    }

    [SerializeField] private MoveSpeedChangeParameters baseMovementParameters;
    [SerializeField] private MoveSpeedChangeParameters oceanMovementParameters;
    [SerializeField] private AreaCheck areaCheck;
    [SerializeField] private GameObject title;
    private float alphaDecrease = 0.01f;

    [SerializeField] private Transform playerTransform;
    private float playerInitialXPosition;
    [SerializeField] private Transform forest1, wheat, wheatMidPoint, ocean, oceanMidpoint, forest2, house;
    [SerializeField] private Transform oceanMid, oceanEnd;
    [SerializeField] private Transform sunset, moon, sunrise;
    [SerializeField] private Light2D sunsetLight, moonLight, sunriseLight, globalSkyLight;
    public Animator oceanAnimator;

    [HideInInspector]
    public bool oceanPause1 = true, oceanRegular, oceanPause2;

    public int petalCount;
    [SerializeField] private TMP_Text petalText;

    //light intensity variables
    public float skyLightMidPoint = .1f, skyLightMinIntensity = 0f, skyLightMaxIntensity = 0.9f;
    private float moonLightMinIntensity = 0f, moonLightMaxIntensity = 0.005f;
    private float sunriseMinIntensity = 0f, sunriseMaxIntensity = .8f;
    private float sunsetMinIntensity = .1f, sunsetMaxIntensity = .8f;

    //light color variables
    private Color sunriseEndColor = Color.white, sunsetEndColor = Color.red, moonEndColor = new Color(129f, 217f, 255f);
    private Color sunriseStartColor = Color.yellow, sunsetStartColor = Color.white, moonStartColor = Color.white;
    public float sunsetColorSpeed = 10f, sunriseColorSpeed = .04f, moonColorSpeed = .03f;

    //celestial position variables
    private float moonSpeed = 0.01f;
    private Vector3 sunsetStartPosition, sunsetEndPosition, sunriseStartPosition, sunriseEndPosition, moonStartPosition, moonEndPosition;

    private SeedInWind seed;
    
    // Start is called before the first frame update
    void Start()
    {
        seed = playerTransform.GetComponent<SeedInWind>();

        sunsetStartPosition = sunset.position;
        sunsetEndPosition = new Vector3(playerTransform.position.x, -15f, playerTransform.position.z);
        sunriseStartPosition = sunrise.position;
        sunriseEndPosition = new Vector3(playerTransform.position.x, 16f, playerTransform.position.z);
        moonStartPosition = moon.position;
        moonEndPosition = new Vector3(playerTransform.position.x, 12f, playerTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        SkyUpdate();
        CelestialPositionUpdate();
        MovementSpeedUpdate();
        petalText.text = "Petals: " + petalCount.ToString();

        if (title == null && petalText.color.a < 1f)
        {
            //set alpha of petal text to 1
            petalText.color = new Color(1f, 1f, 1f, petalText.color.a + alphaDecrease);
        }
    }

    private bool previouslyInOcean = false;

	private void MovementSpeedUpdate()
	{
        if (areaCheck.ocean && !previouslyInOcean)
        {
            seed.LongTermChangeSpeed(oceanMovementParameters.baseSpeed, oceanMovementParameters.maxSpeed, 
                oceanMovementParameters.maxBoostedSpeed, oceanMovementParameters.speedDecayRate);
        }
        else if (!areaCheck.ocean && previouslyInOcean)
        {
			seed.LongTermChangeSpeed(oceanMovementParameters.baseSpeed, oceanMovementParameters.maxSpeed,
				oceanMovementParameters.maxBoostedSpeed, oceanMovementParameters.speedDecayRate);
		}

        previouslyInOcean = areaCheck.ocean;
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
            float p = Mathf.InverseLerp(wheat.position.x, ocean.position.x, playerTransform.position.x);
            
            sunset.position = Vector3.Lerp(sunsetStartPosition, sunsetEndPosition, p);
            //make sky gradually more orange and less bright
            sunsetLight.color = Color.Lerp(sunsetStartColor, sunsetEndColor, p);
            sunsetLight.intensity = Mathf.Lerp(sunsetMaxIntensity, sunsetMinIntensity, p);
            globalSkyLight.intensity = Mathf.Lerp(skyLightMaxIntensity, skyLightMidPoint, p);
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
            if (playerTransform.position.x <= oceanMidpoint.position.x)
            {
                float p = Mathf.InverseLerp(ocean.position.x, oceanMidpoint.position.x, playerTransform.position.x);

                sunsetLight.intensity = Mathf.Lerp(sunsetMinIntensity, -1f, p);

                moon.position = Vector3.Lerp(moonStartPosition, moonEndPosition, p);
                moonLight.color = Color.Lerp(moonStartColor, moonEndColor, p);
                moonLight.intensity = Mathf.Lerp(moonLightMinIntensity, moonLightMaxIntensity, p);

                //lower global sky light intensity to 0 using lerp
                globalSkyLight.intensity = Mathf.Lerp(skyLightMidPoint, skyLightMinIntensity, p);
            }

            //Lower the moon
            if (playerTransform.position.x > oceanMidpoint.position.x)
            {
                float p = Mathf.InverseLerp(oceanMidpoint.position.x, forest2.position.x, playerTransform.position.x);

                moon.position = Vector3.Lerp(moonEndPosition, moonStartPosition, p);
                moonLight.intensity = Mathf.Lerp(moonLightMaxIntensity, moonLightMinIntensity, p);

                //raise global sky light intensity to .3f using lerp
                globalSkyLight.intensity = Mathf.Lerp(skyLightMinIntensity, skyLightMidPoint, p);
            }
        }

        if (areaCheck.forest2)
        {
            float p = Mathf.InverseLerp(forest2.position.x, house.position.x, playerTransform.position.x);

            sunrise.position = Vector3.Lerp(sunriseStartPosition, sunriseStartPosition, p);

            //make sky gradually more white
            sunriseLight.color = Color.Lerp(sunriseStartColor, sunriseEndColor, p);
            sunriseLight.intensity = Mathf.Lerp(sunriseMinIntensity, sunriseMaxIntensity, p);

            //raise global sky light intensity to .7f using lerp
            globalSkyLight.intensity = Mathf.Lerp(skyLightMidPoint, skyLightMaxIntensity, p);

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