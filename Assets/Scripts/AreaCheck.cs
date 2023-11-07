using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCheck : MonoBehaviour
{
    public bool forest1 = true, wheat, ocean, forest2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If collision is called area 1, play the intro
        if (collision.gameObject.name == "Forest 1")
        {
            forest1 = true;
            forest2 = false;
        }

        //If collision is called wheat, wheat is true
        if (collision.gameObject.name == "Wheat")
        {
            wheat = true;
            forest1 = false;
        }

        //If collision is called ocean, ocean is true
        if (collision.gameObject.name == "Ocean")
        {
            ocean = true;
            wheat = false;
        }

        //If collision is called forest 2, forest 2 is true
        if (collision.gameObject.name == "Forest 2")
        {
            forest2 = true;
            ocean = false;
        }
    }
}
