using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //private player transform
    [SerializeField] private RotationController rotationController;
    [SerializeField] private AreaCheck areaCheck;
    [SerializeField] private AudioSource intro, layer1, layer2, layer3, layer4;
    [SerializeField] private AudioSource wind1, wind2, wind3;
    [SerializeField] private AudioSource oceanAudio, collectSfx, riseSfx;
    private bool introCanPlay = false;

    // Start is called before the first frame update
    void Start()
    {
        intro.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !Input.GetButton("Pause") && !rotationController.startScene)
        {
            wind3.volume = Mathf.Lerp(wind3.volume, 1f, Time.deltaTime);
        }

        else if (!rotationController.startScene)
        {
            wind3.volume = Mathf.Lerp(wind3.volume, 0, Time.deltaTime);
        }

        if (rotationController.startScene)
        {
            intro.loop = false;

            if (!intro.isPlaying)
            {
                //Checks whether to play the intro or the main song
                MusicLoopCheck();

                //Checks which layers to play
                MusicLayerCheck();

                //Checks which wind to play
                WindCheck();
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

    private void WindCheck()
    {
        
        if (areaCheck.ocean)
        {
            wind1.volume = Mathf.Lerp(wind1.volume, .2f, Time.deltaTime);
            wind3.volume = Mathf.Lerp(wind3.volume, 0, Time.deltaTime);
            wind2.volume = Mathf.Lerp(wind2.volume, 0, Time.deltaTime);
            oceanAudio.volume = Mathf.Lerp(oceanAudio.volume, .2f, Time.deltaTime);
        } 

        else
        {
            wind1.volume = Mathf.Lerp(wind1.volume, .35f, Time.deltaTime);
            wind2.volume = Mathf.Lerp(wind2.volume, .15f, Time.deltaTime);
            wind3.volume = Mathf.Lerp(wind3.volume, .25f, Time.deltaTime);
            oceanAudio.volume = Mathf.Lerp(oceanAudio.volume, 0, Time.deltaTime);
        }
    }

    public void PlayCollectSfx()
    {
        if (!collectSfx.isPlaying)
        {
            collectSfx.Play();
        }
    }

    public void RiseSfx()
    {
        if (!riseSfx.isPlaying) {
            //randomize pitch
            riseSfx.pitch = Random.Range(0.8f, 1.2f);
            riseSfx.Play();
        }
    }
}
