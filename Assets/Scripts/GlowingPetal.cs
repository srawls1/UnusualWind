using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingPetal : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject player;
    private Animator animator;
    private float collectSpeed = 15f;
    private float moveAwaySpeed = 10f;
    private bool moveTowardsPlayer = false;
    private bool moveAway = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (moveTowardsPlayer)
        {
            //Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, collectSpeed * Time.deltaTime);
        }

        if (moveAway && !moveTowardsPlayer)
        {
            //Move away from the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -moveAwaySpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Trigger the animation
            animator.SetTrigger("Collect");
            //Increase petal count from the game manager
            gameManager.GetComponent<GameManager>().petalCount++;
        }
    }

    private void MoveTowardsPlayer()
    {
        moveAway = false;
        moveTowardsPlayer = true;
    }

    private void MoveAway()
    {
        moveAway = true;
    }

    private void DestroyPetal()
    {
        //Destroy the petal
        Destroy(gameObject);
    }
}
