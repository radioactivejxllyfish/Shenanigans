using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ult_CombatMode : MonoBehaviour
{
    public PlayerAnimationHandler playerAnimationHandler;
    private PlayerController playerController;
    public GameObject complex;
    public bool combatMode;
    
    public Animator animator;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();    
    }

    void Update()
    {
        if (playerController.ultMeter >= 100 && Input.GetKeyDown(KeyCode.T))
        {
            bool deployed = false;
            if (!deployed)
            {
                playerController.ultMeter = 0;
                combatMode = true;
                deployed = true;
                StartCoroutine("Deploy");
                StartCoroutine("Timer");
            }
        }
    }

    private IEnumerator Deploy()
    {
        complex.SetActive(true);
        animator.Play("Deploy");
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(30f);
        combatMode = false;
        animator.Play("Retract");
        yield return new WaitForSeconds(0.5f);
        complex.SetActive(false);
        playerAnimationHandler.combatMode = false;
    }
    
}
