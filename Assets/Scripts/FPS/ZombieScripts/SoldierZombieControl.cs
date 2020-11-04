using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierZombieControl : MonoBehaviour
{
    public NavMeshAgent agent;
    private NavMeshPath path;

    public Animator animator;

    public PlayerHealth playerHealth;
    public EnemyHealth enemyHealth;

    private ObjectUpdate objUpdate;

    public Transform Player;
    public Utility utility;

    private float attackTime = 0.0f, attackLength;
    public int damageObjPoint, damagePlayerPoint;

    private bool isAttacking, attackPlayer, finishAttack;

    private Vector3 pos;

    private float spikeTimer;
    private ObjectUpdate spike;

    void Start()
    {
        path = new NavMeshPath();
        isAttacking = false;
        finishAttack = false;
        attackTime = 0.0f;
        spikeTimer = 0;
        attackLength = 1.3f;
    }

    void Update()
    {
        if (enemyHealth.dead)
            return;

        agent.SetDestination(Player.position);
        path = new NavMeshPath();
        agent.CalculatePath(agent.destination, path);
        agent.SetPath(path);

        if (finishAttack)
        {
            agent.updatePosition = true;
            agent.updateRotation = true;
            transform.position = pos;
            finishAttack = false;
        }

        if (isAttacking)
        {
            agent.CalculatePath(Player.position, path);
            if (!attackPlayer && path.status == NavMeshPathStatus.PathComplete)
            {
                finishAttack = true;
                animator.SetBool("attack", false);
                isAttacking = false;
            }
            else
            {
                attackTime += Time.deltaTime;
                if (attackTime >= attackLength)
                {
                    attackTime = 0.0f;
                    if (attackPlayer)
                    {
                        playerHealth.getHarm(damagePlayerPoint, true);
                    }
                    else
                    {
                        if (objUpdate && !objUpdate.Damage(damageObjPoint))
                        {
                            animator.SetBool("attack", false);
                            isAttacking = false;
                            finishAttack = true;
                        }
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isAttacking)
            return;
        if (collision.collider.tag == "SideWall")
        {
            agent.CalculatePath(Player.position, path);
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                objUpdate = collision.collider.gameObject.GetComponent<ObjectUpdate>();
                agent.updatePosition = false;
                transform.rotation = Quaternion.LookRotation
                    (-(collision.contacts[0].normal), Vector3.up);
                agent.updateRotation = false;
                animator.SetBool("attack", true);
                isAttacking = true;
                attackPlayer = false;
                pos = transform.position;
            }
        }
        if (collision.collider.gameObject.tag == "Player")
        {
            agent.updatePosition = false;
            animator.SetBool("attack", true);
            isAttacking = true;
            attackPlayer = true;
            pos = transform.position;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (utility.belongToObj(collision.collider.gameObject.tag) ||
            collision.collider.gameObject.tag == "Player")
        {
            if (isAttacking)
            {
                finishAttack = true;
                animator.SetBool("attack", false);
                isAttacking = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SpikeHurt")
        {
            agent.speed /= 1.5f;
            spike = other.transform.parent.GetComponent<ObjectUpdate>();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "SpikeHurt")
        {
            spikeTimer += Time.deltaTime;
            if (spikeTimer >= 0.3f)
            {
                enemyHealth.GetDamage(5, false, false);
                if (spike)
                    spike.Damage(1);
                spikeTimer = 0.0f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "SpikeHurt")
        {
            agent.speed *= 1.5f;
            spike = null;
        }
    }
}
