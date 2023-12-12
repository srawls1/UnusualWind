using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlowingPetal : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioSource collectSfx;
    private Animator animator;
    private float collectSpeed = 10f;
    private float moveAwaySpeed = 10f;
    private bool moveTowardsPlayer = false;
    private bool moveAway = false;
    private float alphaDecrease = .01f;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private SpriteRenderer glow;
    [SerializeField] private Light2D light2D;
    private bool fadeOut = false;
    private int petalPoints = 1;

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

            if (transform.position == player.transform.position)
            {
                gameManager.GetComponent<GameManager>().petalCount += petalPoints;
                //Destroy the petal
                Destroy(gameObject);
            }
        }

        if (moveAway && !moveTowardsPlayer)
        {
            //Move away from the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -moveAwaySpeed * Time.deltaTime);
        }

        if (fadeOut)
        {
            //decrease alpha value of sprite
            sprite.color = new Color(1f, 1f, 1f, sprite.color.a - alphaDecrease);
            glow.color = new Color(1f, 1f, 1f, glow.color.a - alphaDecrease);
            //decrease intensity of light
            light2D.intensity -= alphaDecrease;

            if (sprite.color.a <= 0)
            {                
                gameManager.GetComponent<GameManager>().petalCount += petalPoints;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Trigger the animation
            animator.SetTrigger("Collect");

            audioManager.PlayCollectSfx();

            //turn off the collider
            GetComponent<Collider2D>().enabled = false;
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

    private void FadeOut()
    {
        fadeOut = true;
    }
}
