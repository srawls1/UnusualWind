using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If leaf velocity is less than 0.1, settrigger rest to true
        if (GetComponent<Rigidbody2D>().velocity.magnitude < 0.1)
        {
            GetComponent<Animator>().SetTrigger("Rest");
        }

        //If leaf has been resting for 5 seconds, destroy it
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Rest") && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 5)
        {
            Destroy(gameObject);
        }
    }
}
