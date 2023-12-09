using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    public bool isFish; //If true, object will move in an arc
    public float arcHeight; //How high the arc is
    public float speed; //How fast the object moves
    public float distanceToPlayer; // negative if coming from left

    private float movementSpeed = 0f;
    private float passedAmount = 50;
    private string naturalHabitat;

    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        //Set the natural habitat
        if (isFish)
        {
            naturalHabitat = "Water";
        }
        else
        {
            naturalHabitat = "Forest";
        }

        //Find the game manager
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
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
        if (isFish && transform.position.x - gameManager.forest2.position.x < passedAmount)
        {
            Destroy(gameObject);
        }

        if (!isFish && transform.position.x - gameManager.wheat.position.x < passedAmount)
        {
            Destroy(gameObject);
        }
    }
}
