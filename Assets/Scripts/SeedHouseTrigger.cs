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
    private Animator animator;

    private void Start()
    {
        seedInWind = GetComponent<SeedInWind>();
        rotationController = GetComponent<RotationController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (impactStartPoint != null)
        {
            //Inverse lerp
            float t = Mathf.InverseLerp(impactStartPoint.position.x, impactPoint.transform.position.x, transform.position.x * impactSpeed);
            //Move towards impact point using lerp
            transform.position = Vector3.Lerp(impactStartPoint.position, impactPoint.transform.position, t);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        //If collision is called house, call house function
        if (collision.gameObject.name == "House")
        {
            //Trigger house animation
            animator.SetTrigger("House");
        }
    }
}
