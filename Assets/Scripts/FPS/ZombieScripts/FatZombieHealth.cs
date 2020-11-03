using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatZombieHealth : EnemyHealth
{
    public Transform explosion;

    public AudioSource audio;

    public void explode()
    {
        StartCoroutine("WaitForDeadAnimationFinish");
    }

    private IEnumerator WaitForDeadAnimationFinish()
    {
        animator.speed = 0.3f;
        animator.Play("Explode", 0);
        audio.Play();
        dead = true;
        GetComponent<Rigidbody>().isKinematic = true;
        agent.enabled = false;
        blip.SetActive(false);

        yield return new WaitForSeconds
            (animator.GetCurrentAnimatorStateInfo(0).length / animator.speed);

        Instantiate(explosion, transform.position + 0.3f * Vector3.up, Quaternion.identity);
        Destroy(gameObject);
    }
}
