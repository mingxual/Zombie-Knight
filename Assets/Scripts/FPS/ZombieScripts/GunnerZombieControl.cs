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
    public EnemyHealth enemyHealth;

    public Transform Player;
    public Utility utility;

    private float attackTime = 0.0f, attackLength;
    public int damageObjPoint, damagePlayerPoint;

    private bool isAttacking, attackPlayer, finishAttack;

    private Vector3 pos;

    public Transform gunnerBomb;
    private float coolLength, coolTimer;
    private bool hasFired, finishFire;

    private ObjectUpdate objUpdate;

    private float spikeTimer;
    private ObjectUpdate spike;

    public ParticleSystem flameParticle1, flameParticle2;
    public AudioSource flameSound;

    public float canAttackRadius;

    void Start()
    {
        path = new NavMeshPath();
        isAttacking = false;
        finishAttack = false;
        finishFire = false;
        attackTime = 0.0f;
        attackLength = 1.3f;
        coolLength = 10.0f;
        coolTimer = 0.0f;
    }

    void Update()
    {
        if (enemyHealth.dead)
            return;

        agent.SetDestination(Player.position);
        path = new NavMeshPath();
        agent.CalculatePath(agent.destination, path);
        agent.SetPath(path);

        if (hasFired)
            return;

        Vector3 dist = Player.position - transform.position;
        dist.y = 0;
        if (!isAttacking && dist.magnitude < 10 && spike == null)
        {
            coolTimer += Time.deltaTime;
            if (coolTimer >= coolLength)
            {
                coolTimer = 0.0f;
                hasFired = true;
                animator.Play("Fire", 0);        
                agent.updatePosition = false;
                pos = transform.position;
            }
        }

        if (finishAttack)
        {
            agent.updatePosition = true;
            agent.updateRotation = true;
            transform.position = pos;
            finishAttack = false;
            animator.SetBool("attack", false);
            isAttacking = false;
        }

        if (finishFire)
        {
            transform.position = pos;
            finishFire = false;
        }

        if (isAttacking)
        {
            if (!attackPlayer && path.status == NavMeshPathStatus.PathComplete)
            {
                agent.CalculatePath(Player.position, path);
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    finishAttack = true;
                    animator.SetBool("attack", false);
                    isAttacking = false;
                }
            }
        }
    }

    public void Attack()
    {
        if (attackPlayer)
        {
            float dist = Vector3.Distance(transform.position, Player.position);
            if (dist >= canAttackRadius)
                return;
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

    void Fire()
    {
        Transform bomb = Instantiate(gunnerBomb, Player.position, Quaternion.Euler(-90, 0, 0));
        bomb.SetParent(transform);
        bomb.GetComponent<GunnerBombControl>().gunner = this;
        bomb.GetComponent<GunnerBombControl>().player = Player;
        flameParticle1.Play();
        flameParticle2.Play();
        flameSound.Play();
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
                agent.updatePosition = false;
                transform.rotation = Quaternion.LookRotation
                    (-(collision.contacts[0].normal), Vector3.up);
                agent.updateRotation = false;
                animator.SetBool("attack", true);
                isAttacking = true;
                attackPlayer = false;
                pos = transform.position;
                objUpdate = collision.collider.gameObject.GetComponent<ObjectUpdate>();
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
        if (collision.collider.gameObject.tag == "Player")
        {
            if (isAttacking)
            {
                finishAttack = true;
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
