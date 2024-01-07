using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedHouseTrigger : MonoBehaviour
{
    [SerializeField] private GameObject impactPoint;
    private SeedInWind seedInWind;
    private RotationController rotationController;
    private Transform impactStartPoint;
    private float impactSpeed = 2f;
    [SerializeField] private GameObject houseAnimator;
    private Animator mainAnimator;
    private Rigidbody2D rig;
    private bool isHouse = false;

    // Swap the Animator on the GameObject


    private void Start()
    {
        seedInWind = GetComponent<SeedInWind>();
        if (seedInWind == null)
        {
            Debug.LogError("Seed in wind script not found");
        }
        rotationController = GetComponent<RotationController>();
        if (rotationController == null)
        {
            Debug.LogError("Rotation controller script not found");
        }
        mainAnimator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (impactStartPoint != null)
        {
            //Move towards impact point using lerp
            transform.position = Vector3.Lerp(impactStartPoint.position, impactPoint.transform.position, impactSpeed * Time.deltaTime);
            rig.velocity = Vector2.zero;
            rig.isKinematic = true;
            GetComponent<Collider2D>().enabled = false;

            //If distance between impact point and seed is less than 0.1, call house function
            if (Vector3.Distance(transform.position, impactPoint.transform.position) < 0.1f)
            {
                Debug.Log("Swap Animator");
                SwapAnimator();
            }

            if (isHouse)
            {
                transform.position = houseAnimator.transform.position;
                transform.rotation = houseAnimator.transform.rotation;
            }
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

        //Turn off rotation controller script
        rotationController.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //If collision is called house, call house function
        if (collider.gameObject.name == "House")
        {
            Debug.Log("House Collision");
            HouseStart();
        }
    }

    void SwapAnimator()
    {
        mainAnimator.enabled = false;
        isHouse = true;
        
        houseAnimator.SetActive(true);
        transform.parent = houseAnimator.transform;
    }
}
