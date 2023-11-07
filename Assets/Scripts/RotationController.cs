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

    private Animator animator;
    private TMP_Text startText;
    private GameObject mainCamera;
    private float cameraSize;
    private float diveRotation = 30f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera");
        startMenu = GameObject.Find("StartMenu").GetComponent<StartMenu>();
        startMenu.timeToStart = timeToStart;
        cameraSize = Camera.main.orthographicSize;
        startText = GameObject.Find("StartText").GetComponent<TMP_Text>();
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
                timer += Time.deltaTime;
                transform.rotation = Quaternion.Euler(0, 0, -timer * 10);
                //gradually decrease main camera size
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, cameraSize - (timer * .5f), Time.deltaTime * 2);
            }

            if (startScene && timer <= timeToStart)
            {
                transform.rotation = Quaternion.Euler(0, 0, -diveRotation);
            }

            //If any key is held for 3 seconds, set startScene to true
            if (timer > timeToStart && !startScene)
            {
                //Change sprite layer to "player"
                gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
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
