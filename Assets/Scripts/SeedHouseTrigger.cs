using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedHouseTrigger : MonoBehaviour
{
    [SerializeField] private GameObject impactPoint;
    private SeedInWind seedInWind;
    private Transform impactStartPoint;
    private float impactSpeed = 6f;

    private void Start()
    {
        seedInWind = GetComponent<SeedInWind>();
    }

    private void Update()
    {
        if (impactStartPoint != null)
        {
            //Inverse lerp
            float distance = Vector2.Distance(impactStartPoint.position, impactPoint.transform.position);
            float lerpValue = distance / impactSpeed;
            //Move towards impact point using lerp
            transform.position = Vector2.Lerp(impactStartPoint.position, impactPoint.transform.position, lerpValue);
        }
    }

    public void HouseStart()
    {
        if (impactStartPoint == null)
        {
            impactStartPoint = transform;
        }

        //Turn off seed in wind script
        seedInWind.enabled = false;
    }
}
