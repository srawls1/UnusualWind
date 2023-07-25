using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyScript : MonoBehaviour
{
    public float frequency;
    public float dampingRatio;
    public float addVelocity;
    public float velocity = 1;
    private Rigidbody2D rb;
    public GameObject springPrefab; // Reference to the Spring Prefab
    private GameObject currentSpring; // Reference to the currently instantiated spring object
    private bool addVelocityCheck;
    private float velCheck;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Jump
            rb.velocity = Vector2.up * velocity;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyOldSpring(); // Destroy the old spring object (if it exists)
            InstantiateSpring();
            addVelocityCheck = false;
        }

        if (rb.velocity.y > 0 && !addVelocityCheck)
        {
            DestroyOldSpring();
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + addVelocity);
            addVelocityCheck = true;
        }
    }

    private void DestroyOldSpring()
    {
        if (currentSpring != null)
        {
            Destroy(currentSpring);
        }
    }

    private void InstantiateSpring()
    {
        // Get the current position of the player
        Vector3 playerPosition = transform.position;

        // Instantiate the spring prefab at the player's position
        GameObject newSpring = Instantiate(springPrefab, playerPosition, Quaternion.identity);

        // Optionally, you can attach the spring to the player's hierarchy
        newSpring.transform.SetParent(transform);

        // Add a SpringJoint2D component to the new spring object
        SpringJoint2D springJoint = newSpring.AddComponent<SpringJoint2D>();

        // Set the connected body of the SpringJoint2D to the player's Rigidbody2D
        springJoint.connectedBody = GetComponent<Rigidbody2D>();

        // Customize the SpringJoint2D properties as per your requirement
        springJoint.frequency = frequency; // Adjust the spring frequency (strength)
        springJoint.dampingRatio = dampingRatio;  // Adjust the damping ratio to control oscillations

        // Store a reference to the newly instantiated spring object
        currentSpring = newSpring;
    }
}