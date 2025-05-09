using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RotationController : MonoBehaviour
{
    public float timeToStart = 2f;
    public float timer;
    [SerializeField] private Transform pivot;

    private Animator animator;
    private float cameraSize;
    private float diveRotation = 30f;
    private float rotationSpeed = 7f;

	private bool anyKeyPreviouslyHeld;
	private bool anyKeyHeld;
	private bool anyKeyReleased;

    private bool m_startScene;
    public bool startScene
    {
        get { return m_startScene; }
        set
        {
            m_startScene = value;
            if (value)
            {
				animator.SetBool("Resting", false);
				animator.SetBool("Normal", true);
			}
        }
    }

	// Start is called before the first frame update
	void Start()
    {
        animator = GetComponent<Animator>();
        cameraSize = Camera.main.orthographicSize;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();

        if (startScene && timer > timeToStart && anyKeyReleased)
        {
            animator.SetBool("Dive", false);
            animator.SetBool("Rise", true);
        }
        //If any button is pressed, rotate about the z-axis to -30 degrees
        if (anyKeyHeld)
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
	}

	private void UpdateInput()
	{
		bool anyKeyCurrentlyPressed = Input.anyKey && !Input.GetButton("Pause");
		anyKeyHeld = anyKeyCurrentlyPressed;
		anyKeyReleased = !anyKeyHeld && anyKeyPreviouslyHeld;
		anyKeyPreviouslyHeld = anyKeyHeld;
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