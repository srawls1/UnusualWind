using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class GlowingPetalLight : MonoBehaviour
{
    [SerializeField] Light2D light2D;
    void TurnOnLight() {
        light2D.gameObject.SetActive(true);
    }
}
