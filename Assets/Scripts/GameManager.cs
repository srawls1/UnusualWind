using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class GameManager : MonoBehaviour
{
    private AreaCheck areaCheck;

    private Transform playerTransform;
    private float playerInitialXPosition;
    private Transform forest1, wheat, ocean, forest2;
    private Transform sunset;
    private Transform moon;
    private Transform sunrise;
    private float moonRestingPosition = 12f;
    private float oceanMidpoint = 500f;
    public float dayNightChangeSpeed = 0.001f;
    private Animator oceanAnimator;

    public int petalCount;
    private TMP_Text petalText;

    //Ocean Values
    private float wheatTempMax = 50f;
    private float oceanTintMax = 50f, oceanTempMax = -20f;
    
    //initialize post processing volume
    public PostProcessVolume volume;
    //initialize post processing profile
    private PostProcessProfile postProcessProfile;
    //initialize color grading
    private ColorGrading colorGrading;
    
    // Start is called before the first frame update
    void Start()
    {
        //find the player tag
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        oceanAnimator = GameObject.Find("Ocean").GetComponent<Animator>();
        sunset = GameObject.Find("Sunset").GetComponent<Transform>();
        moon = GameObject.Find("Moon").GetComponent<Transform>();
        sunrise = GameObject.Find("Sunrise").GetComponent<Transform>();
        //initialize post processing volume
        volume = GameObject.Find("Post-process Volume").GetComponent<PostProcessVolume>();
        //initialize area check
        areaCheck = GameObject.FindWithTag("Player").GetComponent<AreaCheck>();
        forest1 = GameObject.Find("Forest 1").GetComponent<Transform>();
        wheat = GameObject.Find("Wheat").GetComponent<Transform>();
        ocean = GameObject.Find("Ocean").GetComponent<Transform>();
        forest2 = GameObject.Find("Forest 2").GetComponent<Transform>();
        //initialize post processing profile
        postProcessProfile = volume.profile;
        //initialize color grading
        colorGrading = volume.profile.GetSetting<ColorGrading>();
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
            sunset.position = new Vector3(0, Mathf.Abs(wheat.position.x - playerTransform.position.x) * .05f, 0);
            //make sky gradually more orange
            if (colorGrading.temperature.value < wheatTempMax)
            {
                colorGrading.temperature.value += dayNightChangeSpeed;
                colorGrading.tint.value += dayNightChangeSpeed * .02f;
            }
        }

        if (areaCheck.ocean)
        {
            if (playerTransform.position.x > GameObject.Find("Ocean").transform.position.x)
            {
                oceanAnimator.SetBool("Pause1", false);
                oceanAnimator.SetBool("Regular", true);
            }

            if (playerTransform.position.x > GameObject.Find("Ocean End").transform.position.x)
            {
                oceanAnimator.SetBool("Regular", false);
                oceanAnimator.SetBool("Pause2", true);
            }

            if (moon.position.y < moonRestingPosition && playerTransform.position.x <= oceanMidpoint)
            {
                moon.position = new Vector3(0, (playerTransform.position.x - ocean.position.x) * .05f, 0);
            }

            if (colorGrading.temperature.value > oceanTempMax)
            {
                colorGrading.temperature.value -= dayNightChangeSpeed;
            }
            if (colorGrading.tint.value < oceanTintMax)
            {
                //make sky gradually more purple
                colorGrading.tint.value += dayNightChangeSpeed * 3f;
            }

            if (playerTransform.position.x > oceanMidpoint)
            {
                moon.position -= new Vector3(0, playerTransform.position.x * .00001f, 0);
                //make sky gradually less purple
                colorGrading.temperature.value += dayNightChangeSpeed;
                colorGrading.tint.value -= dayNightChangeSpeed;
            }
        }

        if (areaCheck.forest2)
        {
            sunrise.position = new Vector3(0, (playerTransform.position.x - forest2.position.x) * .05f, 0);
            moon.position -= new Vector3(0, playerTransform.position.x * .00001f, 0);
            
            if (colorGrading.temperature.value < 0f)
            {             
                //make sky gradually more orange
                colorGrading.temperature.value += dayNightChangeSpeed;
            }
            if (colorGrading.tint.value < 0f)
            {
                colorGrading.tint.value -= dayNightChangeSpeed;
            }
        }
    }

    private void CelestialPositionUpdate()
    {
        sunset.position = new Vector3(playerTransform.position.x - 5, sunset.position.y, sunset.position.z);
        moon.position = new Vector3(playerTransform.position.x - 1, moon.position.y, moon.position.z);
        sunrise.position = new Vector3(playerTransform.position.x + 5, sunrise.position.y, sunrise.position.z);
    }
}