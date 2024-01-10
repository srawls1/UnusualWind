using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedHouseTrigger : MonoBehaviour
{
    [SerializeField] private GameObject impactPoint;
    private SeedInWind seedInWind;
    private RotationController rotationController;
    private Transform impactStartPoint;
    [SerializeField] private float impactDistance = .1f;
    [SerializeField] private GameObject houseAnimator;
    private Animator mainAnimator;
    private Rigidbody2D rig;
    public static bool isHouseTriggered = false;
    [SerializeField] private float duration = 5f;

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

    private void FixedUpdate()
    {
        if (isHouseTriggered)
        {            
            rig.isKinematic = true;
            GetComponent<Collider2D>().enabled = false;

            //If distance between impact point and seed is less than 0.1
            if (Vector3.Distance(transform.position, impactPoint.transform.position) < impactDistance)
            {        
                SwapAnimator();
            }
        }
    }

    public void HouseStart()
    {
        isHouseTriggered = true;

        seedInWind.enabled = false;

        mainAnimator.SetBool("Resting", true);

        StartCoroutine(LerpPosition(impactPoint.transform.position, duration));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //If collision is called house, call house functionz
        if (collider.gameObject.name == "HouseTrigger")
        {
            HouseStart();
        }
    }

    void SwapAnimator()
    {
        //Turn off rotation controller script
        rotationController.enabled = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        mainAnimator.enabled = false;
        
        houseAnimator.SetActive(true);
        transform.parent = houseAnimator.transform;

        transform.position = houseAnimator.transform.position;
        transform.rotation = houseAnimator.transform.rotation;

        gameObject.SetActive(false);
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            Vector2 velocity = rig.velocity;
            transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref velocity, duration - time);
            rig.velocity = velocity;
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}