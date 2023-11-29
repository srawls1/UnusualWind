using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanAnimation : MonoBehaviour
{
    private GameManager gameManager;
    private Animator animator;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.oceanPause1)
        {
            Debug.Log("Pause1");
            animator.SetBool("Regular", false);
            animator.SetBool("Pause2", false);
            animator.SetBool("Pause1", true);
        }

        if (gameManager.oceanRegular)
        {
            Debug.Log("Regular");
            animator.SetBool("Pause1", false);
            animator.SetBool("Pause2", false);
            animator.SetBool("Regular", true);
        }

        if (gameManager.oceanPause2)
        {
            Debug.Log("Pause2");
            animator.SetBool("Pause1", false);
            animator.SetBool("Regular", false);
            animator.SetBool("Pause2", true);
        }
    }
}
