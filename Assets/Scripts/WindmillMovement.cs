using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillMovement : MonoBehaviour
{
    public int movementSpeed; 

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, movementSpeed * Time.deltaTime);
    }

    //if object collides with player, increase player speed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Call the speed up function in the player script
        }
    }
}