using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingPetal : MonoBehaviour
{
    private GameObject gameManager; 

    // Start is called before the first frame update
    void Start()
    {
        //Find the game manager using the name
        gameManager = GameObject.Find("GameManager");
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Play the collect animation before destroying the object
            Destroy(gameObject);
            //Increase petal count from the game manager
            gameManager.GetComponent<GameManager>().petalCount++;
        }
    }
}
