using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float timer;
    public bool isFish; //If true, object will move in an arc
    public float arcHeight; //How high the arc is
    public float speed; //How fast the object moves
    public float distanceToPlayer; // negative if coming from left

    private float movementSpeed = 0f;
    private float passedAmount = 50;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFish)
        {
            //Movement
            transform.position += new Vector3(movementSpeed * Time.deltaTime, Mathf.Sin(Time.time) * arcHeight, 0f);
            //Rotation
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time) * arcHeight * 300);
        }
        else
        {
            //Movement
            transform.position += new Vector3(movementSpeed * Time.deltaTime, 0f, 0f);
        }

        ActivateMovement();
        CheckIfPassed();
    }

    private void ActivateMovement()
    {
        //If player is within a certain distance of the object, activate the prefab
        if (Mathf.Abs(transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x) < distanceToPlayer)
        {
            //Activate the speed
            movementSpeed = speed;
        }
    }

    private void CheckIfPassed()
    {
        //If the object has passed the player, destroy it
        if (transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x < -passedAmount)
        {
            Destroy(gameObject);
        }

        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

        if (movementSpeed == speed) {
            if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height) {
                timer += Time.deltaTime;

                if (timer > 10f) {
                    Destroy(gameObject);
                }
            } else { timer = 0f; }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the object collides with the player, destroy it
        if (collision.gameObject.tag == "Area Marker" && !IsObjectOnScreen())
        {
            Destroy(gameObject);
        }
    }

    bool IsObjectOnScreen()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return false;
        }

        // Convert the object's position to viewport space
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the object is outside the viewport (not on the screen)
        return viewportPoint.x > 1 || viewportPoint.x < 0 || viewportPoint.y > 1 || viewportPoint.y < 0;
    }
}
