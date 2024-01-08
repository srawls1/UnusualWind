using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedHouseTrigger : MonoBehaviour
{
    [SerializeField] private GameObject impactPoint;
    private SeedInWind seedInWind;
    private RotationController rotationController;
    private Transform impactStartPoint;
    private float impactDistance = .2f;
    [SerializeField] private GameObject houseAnimator;
    private Animator mainAnimator;
    private Rigidbody2D rig;
    private bool isHouse = false;

    private float velocityHolder;

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
            //Lower y position to impact point y position only using lerp
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, impactPoint.transform.position.y, transform.position.z), 0.5f * Time.deltaTime);
            
            rig.isKinematic = true;
            GetComponent<Collider2D>().enabled = false;

            if (Vector3.Distance(transform.position, impactPoint.transform.position) < impactDistance * 4) {
                mainAnimator.SetBool("Resting", true);
            }

            //If distance between impact point and seed is less than 0.1, call house function
            if (Vector3.Distance(transform.position, impactPoint.transform.position) < impactDistance)
            {        
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
        velocityHolder = rig.velocity.magnitude;

        //Turn off seed in wind script
        seedInWind.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //If collision is called house, call house function
        if (collider.gameObject.name == "HouseTrigger")
        {
            Debug.Log("House Collision");
            HouseStart();
        }
    }

    void SwapAnimator()
    {

        //Turn off rotation controller script
        rotationController.enabled = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        rig.velocity = new Vector2(0, 0);

        mainAnimator.enabled = false;
        isHouse = true;
        
        houseAnimator.SetActive(true);
        transform.parent = houseAnimator.transform;
    }
}
