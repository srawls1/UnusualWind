using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //private player transform
    private AreaCheck areaCheck;
    public AudioSource intro, layer1, layer2, layer3, layer4;
    private bool introCanPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        areaCheck = GameObject.Find("Daffodil").GetComponent<AreaCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!intro.isPlaying)
        {
            //Checks whether to play the intro or the main song
            MusicLoopCheck();

            //Checks which layers to play
            MusicLayerCheck();
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
            intro.Play();
        }
    }

    private void MusicLayerCheck()
    {
        if (areaCheck.forest1)
        {
            if (!layer1.isPlaying)
            {
                layer1.volume = 1f;
            }
            if (layer2.isPlaying)
            {
                layer2.volume = 1f;
            }
            if (layer3.isPlaying)
            {
                layer3.volume = 1f;
            }
            if (layer4.isPlaying)
            {
                layer4.volume = 1f;
            }
        }

        if (areaCheck.wheat)
        {
            if (layer1.isPlaying)
            {
                layer1.volume = 0f;
            }
            if (!layer2.isPlaying)
            {
                layer2.volume = 1f;
            }
            if (layer3.isPlaying)
            {
                layer3.volume = 0f;
            }
            if (layer4.isPlaying)
            {
                layer4.volume = 1f;
            }
        }

        if (areaCheck.ocean)
        {
            if (layer1.isPlaying)
            {
                layer1.volume = 1f;
            }
            if (layer2.isPlaying)
            {
                layer2.volume = 0f;
            }
            if (layer3.isPlaying)
            {
                layer3.volume = 1f;
            }
            if (layer4.isPlaying)
            {
                layer4.volume = 0f;
            }
        }

        if (areaCheck.forest2)
        {
            if (layer1.isPlaying)
            {
                layer1.volume = 0f;
            }
            if (layer2.isPlaying)
            {
                layer2.volume = 1f;
            }
            if (layer3.isPlaying)
            {
                layer3.volume = 0f;
            }
            if (layer4.isPlaying)
            {
                layer4.volume = 1f;
            }
        }
    }
}
