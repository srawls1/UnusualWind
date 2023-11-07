using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //private player transform
    private AreaCheck areaCheck;
    private RotationController rotationController;
    public AudioSource intro, layer1, layer2, layer3, layer4;
    private bool introCanPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        intro.loop = true;

        //find object with player tag
        areaCheck = GameObject.FindWithTag("Player").GetComponent<AreaCheck>();
        rotationController = GameObject.FindWithTag("Player").GetComponent<RotationController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationController.startScene)
        {
            intro.loop = false;

            if (!intro.isPlaying)
            {
                //Checks whether to play the intro or the main song
                MusicLoopCheck();

                //Checks which layers to play
                MusicLayerCheck();
            }
        } 
    }

    private void MusicLoopCheck()
    {
        if (!introCanPlay)
        {
            introCanPlay = true;
            if (!layer1.isPlaying)
            {
                layer1.Play();
            }
            if (!layer2.isPlaying)
            {
                layer2.Play();
            }
            if (!layer3.isPlaying)
            {
                layer3.Play();
            }
            if (!layer4.isPlaying)
            {
                layer4.Play();
            }
        }
        else
        {
            introCanPlay = false;
            if (!layer1.isPlaying)
            {
                intro.Play();
            }
        }
    }

    private void MusicLayerCheck()
    {
        if (areaCheck.forest1)
        {
            intro.volume = Mathf.Lerp(intro.volume, 0f, Time.deltaTime);
            layer1.volume = Mathf.Lerp(layer1.volume, 1f, Time.deltaTime);
            layer2.volume = Mathf.Lerp(layer2.volume, 0f, Time.deltaTime);
            layer3.volume = Mathf.Lerp(layer3.volume, 1f, Time.deltaTime);
            layer4.volume = Mathf.Lerp(layer4.volume, 0f, Time.deltaTime);
        }

        if (areaCheck.wheat)
        {
            intro.volume = Mathf.Lerp(intro.volume, 1f, Time.deltaTime);
            layer1.volume = Mathf.Lerp(layer1.volume, 0f, Time.deltaTime);
            layer2.volume = Mathf.Lerp(layer2.volume, 1f, Time.deltaTime);
            layer3.volume = Mathf.Lerp(layer3.volume, 1f, Time.deltaTime);
            layer4.volume = Mathf.Lerp(layer4.volume, 1f, Time.deltaTime);
        }

        if (areaCheck.ocean)
        {
            intro.volume = Mathf.Lerp(intro.volume, 0f, Time.deltaTime);
            layer1.volume = Mathf.Lerp(layer1.volume, 0f, Time.deltaTime);
            layer2.volume = Mathf.Lerp(layer2.volume, 0f, Time.deltaTime);
            layer3.volume = Mathf.Lerp(layer3.volume, 0f, Time.deltaTime);
            layer4.volume = Mathf.Lerp(layer4.volume, 1f, Time.deltaTime);
        }

        if (areaCheck.forest2)
        {
            intro.volume = Mathf.Lerp(intro.volume, 0f, Time.deltaTime);
            layer1.volume = Mathf.Lerp(layer1.volume, 1f, Time.deltaTime);
            layer2.volume = Mathf.Lerp(layer2.volume, 1f, Time.deltaTime);
            layer3.volume = Mathf.Lerp(layer3.volume, 1f, Time.deltaTime);
            layer4.volume = Mathf.Lerp(layer4.volume, 0f, Time.deltaTime);
        }
    }
}
