using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterZombieControl : MonoBehaviour
{
    public Animator animator;

    public PlayerHealth playerHealth;
    public EnemyHealth enemyHealth;

    public Rigidbody body;

    public Transform Player;
    public Utility utility;

    private float attackTime = 0.0f, attackLength;
    public int damageObjPoint, damagePlayerPoint;

    private bool isAttacking;

    private float length = 3f, horizontalForce = 12000f;
    public Vector2 offset;
    private bool isJumping;

    public float speed;

    private bool hit;
    private int currFrame;

    public SkinnedMeshRenderer renderer;
    public Material opaque, transparent;

    public float range;

    public AudioSource audio;

    void Start()
    {
        isAttacking = false;
        hit = false;
        attackTime = 0.0f;
        attackLength = 1.3f;
        currFrame = 0;
    }

    void FixedUpdate()
    {
        if (enemyHealth.dead)
            return;

        if (isAttacking)
        {
            attackTime += Time.deltaTime;
            if (attackTime >= attackLength)
            {
                attackTime = 0.0f;
                playerHealth.getHarm(damagePlayerPoint, true);
            }
        }

        Vector3 dir = (Player.position - transform.position).normalized;
        dir.y = 0;
        if (!hit)
        {
            if (!isJumping && !isAttacking)
            {
                transform.LookAt(Player, Vector3.up);
                Vector3 angle = new Vector3(0, transform.eulerAngles.y, 0);
                transform.eulerAngles = angle;
                Vector3 pos = transform.position;
                pos += dir * speed * Time.deltaTime;
                body.MovePosition(pos);
                if (!audio.isPlaying)
                    audio.Play();
            }
        }
        else 
        {
            currFrame++;
            if (currFrame > 3)
            {
                currFrame = 0;
                hit = false;
                renderer.material = transparent;
            }
        }

        Vector3 startPos = transform.position + transform.forward * offset.x;
        startPos.y += offset.y;

        Debug.DrawRay(startPos, -Vector3.up * length, Color.yellow);
        Vector3 dist = transform.position - Player.position;
        dist.y = 0;
        if (!isJumping && dist.magnitude > range)
        {
            RaycastHit hit;
            if (Physics.Raycast(startPos, -Vector3.up, out hit, length))
            {
                if (hit.transform.tag != "Player" && 
                    hit.transform.tag != "WatchTower" &&
                    hit.transform.tag != "Terrain")
                {
                    animator.Play("Jump", 0, 0);
                    isJumping = true;
                    float obstacleHeight = hit.point.y - transform.position.y;
                    float force = obstacleHeight * 6677f + 12000f;
                    body.AddForce(
                        new Vector3(dir.x * horizontalForce, force, dir.z * horizontalForce), 
                        ForceMode.Impulse);
                    audio.Stop();
                }
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (isJumping && collision.contacts[0].normal == new Vector3(0, 1, 0))
        {
            isJumping = false;
        }

        if (isAttacking)
            return;
        if (collision.collider.gameObject.tag == "Player")
        {
            animator.SetBool("attack", true);
            isAttacking = true;
            renderer.material = opaque;
            audio.Stop();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            animator.SetBool("attack", false);
            isAttacking = false;
            renderer.material = transparent;
        }
    }

    public void getHit()
    {
        renderer.material = opaque;
        currFrame = 0;
        hit = true;
    }
}
