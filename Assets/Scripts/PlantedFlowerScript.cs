using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantedFlowerScript : MonoBehaviour
{
    [SerializeField] private GameObject petalHead;
    [SerializeField] private AudioSource petalPop;

    private void PetalHead()
    {
        petalHead.SetActive(true);
    }

    private void PetalPop()
    {
        //Random pitch
        petalPop.pitch = Random.Range(.8f, 1.2f);
        petalPop.Play();
    }
}
