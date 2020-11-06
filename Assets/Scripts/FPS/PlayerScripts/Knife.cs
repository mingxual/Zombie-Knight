using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public Animator anim;
    public int damagePoint;

    void OnCollisionEnter(Collision collision)
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Knife Attack 1") &&
           !anim.GetCurrentAnimatorStateInfo(0).IsName("Knife Attack 2"))
            return;
        if (collision.collider.tag == "ZombieHead")
        {
            EnemyHealth enemy = collision.collider.gameObject.GetComponent<EnemyHealth>();
            enemy.GetDamage(damagePoint * 2, true);
        }
        else if (collision.collider.tag == "ZombieBody")
        {
            EnemyHealth enemy = collision.collider.gameObject.GetComponent<EnemyHealth>();
            enemy.GetDamage(damagePoint, false);
        }
    }
}
