using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public bool startScene = false;
    public float timeToStart = 2f;
    private float timer;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //If any button is pressed, rotate about the z-axis to -30 degrees
        if (Input.anyKey)
        {

            if (!startScene && timer <= timeToStart)
            {
                Debug.Log("Timer: " + timer);
                timer += Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, 0, -timer * 10);
            }

            if (startScene && timer <= timeToStart)
            {
                transform.rotation = Quaternion.Euler(0, 0, -30);
            }

            //If any key is held for 3 seconds, set startScene to true
            if (timer > timeToStart && !startScene)
            {
                animator.SetBool("Resting", false);
                startScene = true;
            }
        }
        else
        {
            timer = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 2);
        }
    }
}
