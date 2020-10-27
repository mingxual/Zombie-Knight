using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GunnerZombieControl : MonoBehaviour
{
    public NavMeshAgent agent;
    private NavMeshPath path;

    public Animator animator;

    public PlayerHealth playerHealth;

    private ObjectUpdate objUpdate;

    public Transform Player;
    public Utility utility;

    private float attackTime = 0.0f, attackLength;
    public int damageObjPoint, damagePlayerPoint;

    private bool isAttacking, attackPlayer, finishAttack, finishHit;

    private Vector3 pos;

    public Transform gunnerBomb;
    private float coolLength, coolTimer;
    private bool hasFired, finishFire;

    void Start()
    {
        path = new NavMeshPath();
        isAttacking = false;
        finishAttack = false;
        finishHit = false;
        finishFire = false;
        attackTime = 0.0f;
        attackLength = 1.3f;
        coolLength = 10.0f;
        coolTimer = 0.0f;
    }

    void Update()
    {
        agent.SetDestination(Player.position);
        path = new NavMeshPath();
        agent.CalculatePath(agent.destination, path);
        agent.SetPath(path);

        if (hasFired)
            return;

        Vector3 dist = Player.position - transform.position;
        dist.y = 0;
        if (!isAttacking && dist.magnitude < 10)
        {
            coolTimer += Time.deltaTime;
            if (coolTimer >= coolLength)
            {
                coolTimer = 0.0f;
                Fire();
            }
        }

        if (finishAttack)
        {
            agent.updatePosition = true;
            agent.updateRotation = true;
            transform.position = pos;
            finishAttack = false;
        }

        if (finishFire)
        {
            transform.position = pos;
            finishFire = false;
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
                if (attackTime >= attackLength / 2 && !finishHit)
                {
                    finishHit = true;
                    if (attackPlayer)
                    {
                        playerHealth.getHarm(damagePlayerPoint);
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
                if (attackTime >= attackLength)
                {
                    attackTime = 0.0f;
                    finishHit = false;
                }
            }
        }
    }

    void Fire()
    {
        hasFired = true;
        animator.Play("Fire", 0);
        Transform bomb = Instantiate(gunnerBomb, Player.position, Quaternion.Euler(-90, 0, 0));
        bomb.GetComponent<GunnerBombControl>().gunner = this;
        bomb.GetComponent<GunnerBombControl>().player = Player;
        agent.updatePosition = false;

        pos = transform.position;
    }

    public void stopFire()
    {
        hasFired = false;
        if(agent != null) agent.updatePosition = true;
        animator.Play("Walk", 0);
        finishFire = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isAttacking || hasFired)
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
}
