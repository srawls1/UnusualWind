using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    private AreaCheck areaCheck;

    private Transform playerTransform;
    private Transform sunset;
    private Transform moon;
    private Transform sunrise;
    
    //initialize post processing volume
    public PostProcessVolume volume;
    
    // Start is called before the first frame update
    void Start()
    {
        //find the player tag
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        Debug.Log(playerTransform.position.x);
        sunset = GameObject.Find("Sunset").GetComponent<Transform>();
        moon = GameObject.Find("Moon").GetComponent<Transform>();
        sunrise = GameObject.Find("Sunrise").GetComponent<Transform>();
        //initialize post processing volume
        volume = GameObject.Find("Post-process Volume").GetComponent<PostProcessVolume>();
        //initialize area check
        areaCheck = GameObject.FindWithTag("Player").GetComponent<AreaCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        SkyUpdate();
    }

    private void SkyUpdate()
    {
        if (areaCheck.forest1)
        {
            sunset.position -= new Vector3(0, playerTransform.position.x, 0);
            //make sky gradually darker
            volume.weight -= 0.0001f;
            //make sky gradually more orange
            volume.profile.GetSetting<ColorGrading>().temperature.value -= 0.0001f;
        }

        if (areaCheck.wheat)
        {
            sunset.position -= new Vector3(0, playerTransform.position.x, 0);
            moon.position += new Vector3(0, playerTransform.position.x, 0);
            //make sky gradually darker
            volume.weight -= 0.0001f;
            //make sky gradually more orange
            volume.profile.GetSetting<ColorGrading>().temperature.value -= 0.0001f;
        }

        if (areaCheck.ocean)
        {
            moon.position -= new Vector3(0, playerTransform.position.x, 0);
            //make sky gradually more purple
            volume.profile.GetSetting<ColorGrading>().temperature.value += 0.0001f;
            //make sky gradually brighter
            volume.weight += 0.0001f;
        }

        if (areaCheck.forest2)
        {
            sunrise.position += new Vector3(0, playerTransform.position.x, 0);
            //make sky gradually more orange
            volume.profile.GetSetting<ColorGrading>().temperature.value += 0.0001f;
            //make sky gradually brighter
            volume.weight += 0.0001f;
        }
    }
}