using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RotationController : MonoBehaviour
{
    public bool startScene = false;
    public float timeToStart = 2f;
    private float timer;
    private StartMenu startMenu;
    private Transform pivot;

    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private TMP_Text startText;
    private GameObject mainCamera;
    private float cameraSize;
    private float diveRotation = 30f;
    private float rotationSpeed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.Find("Main Camera");
        startMenu = GameObject.Find("StartMenu").GetComponent<StartMenu>();
        cameraSize = Camera.main.orthographicSize;
        startText = GameObject.Find("StartText").GetComponent<TMP_Text>();
        timer = 0;
        pivot = GameObject.Find("StartPivot").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (startScene && timer > timeToStart && !Input.anyKey)
        {
            animator.SetBool("Dive", false);
            animator.SetBool("Rise", true);
        }
        //If any button is pressed, rotate about the z-axis to -30 degrees
        if (Input.anyKey)
        {
            if (startScene && timer > timeToStart)
            {
                animator.SetBool("Normal", false);
                animator.SetBool("Dive", true);
            }

            if (!startScene && timer <= timeToStart)
            {
                timer += Time.deltaTime;
                RotateAroundPivotPoint();
            }

            if (startScene && timer <= timeToStart)
            {
                timer = 0;
                transform.rotation = Quaternion.Euler(0, 0, -diveRotation);
            }

            //If any key is held for 3 seconds, set startScene to true
            if (timer > timeToStart && !startScene)
            {
                animator.SetBool("Resting", false);
                animator.SetBool("Normal", true);
                startScene = true;
            }
        }

        else
        {
            if (!startScene)
            {
                timer = 0;
                Quaternion newRotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 2);
                float newAngle = newRotation.eulerAngles.z;
                float angleDiff = transform.eulerAngles.z - newAngle;
				transform.RotateAround(pivot.position, Vector3.forward, -angleDiff);
            }
            else
            {
				timer = 0;
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 2);
			}
        }

        //If startscene is true and rigidbody2D velocity is greater than 0
        if (startScene && rigidbody2D.velocity.magnitude > 0)
        {
            //Change sprite layer to "player"
            //gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        }
    }

    private void RotateAroundPivotPoint()
    {
        //Rotate around pivot point
        //transform.rotation = Quaternion.Euler(0, 0, -timer * 10);
        transform.RotateAround(pivot.position, Vector3.forward, -rotationSpeed * Time.deltaTime);

        //gradually decrease main camera size
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, cameraSize - (timer * .5f), Time.deltaTime * 2);
    }
}