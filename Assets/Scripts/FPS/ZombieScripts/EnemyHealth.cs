using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    public int totalHealth, currHealth;

    public int value;

    public bool dead;

    public GameObject blip, head, body, agentCollider;

    public SpeedUp speedUp;

    public Transform headShot;

    public GameControl gameControl;

    void Start()
    {
        currHealth = totalHealth;
    }

    public void GetDamage(int damage, bool isHead, bool playerAnimation = true)
    {
        if (dead) return;
        currHealth -= damage;
        print("hurt" + damage);
        
        speedUp.increaseFillin(damage);

        if (currHealth <= 0)
        {
            if (isHead)
                Instantiate(headShot, Vector3.zero, Quaternion.identity);

            head.SetActive(false);
            body.SetActive(false);
            if(agentCollider)
                agentCollider.SetActive(false);

            gameControl.killOneZombie(value);

            StartCoroutine("WaitForDeadAnimationFinish");
        }
        else
        {
            if (playerAnimation)
            {
                animator.Play("Get_Hit", 0, 0);
            }
        }
    }

    private IEnumerator WaitForDeadAnimationFinish()
    {
        animator.Play("Dead", 0);
        dead = true;
        GetComponent<Rigidbody>().isKinematic = true;
        if (agent)
        {
            agent.enabled = false;
        }

        blip.SetActive(false);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject, 3);
    }
}
