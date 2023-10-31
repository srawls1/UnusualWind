using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormCloud : MonoBehaviour
{
    [SerializeField] private float seedDropDuration;

    private Animator animator;
    private GameObject lightning;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lightning = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Whenever player collides with cloud, lightning strikes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //flash lightning on and off twice
            animator.SetTrigger("Strike");

            //Temporarily force player to drop
            SeedInWind seed = collision.GetComponent<SeedInWind>();
            seed.Drop(seedDropDuration);
        }
    }
}