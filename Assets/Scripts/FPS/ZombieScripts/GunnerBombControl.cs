using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBombControl : MonoBehaviour
{
    public GunnerZombieControl gunner;
    public Transform player;

    public SpriteRenderer render;

    public Transform bomb;

    public float speed, range, aimTime, fireTime;
    private float aimTimer;

    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = 0.35f;
        transform.position = pos;
    }

    void Update()
    {
        if (aimTimer < aimTime)
        {
            aimTimer += Time.deltaTime;

            Vector3 dist = player.position - transform.position;
            dist.y = 0f;
            if (dist.magnitude > range)
            {
                Vector3 pos = transform.position;
                pos += Time.deltaTime * speed * dist.normalized;
                transform.position = pos;
            }

            if (aimTimer >= aimTime)
            {
                StartCoroutine("fire");
            }
        }
    }

    private IEnumerator fire()
    {
        render.color = Color.red;

        yield return new WaitForSeconds(fireTime);

        Destroy(gameObject);
        Vector3 pos = transform.position;
        pos.y = 0.52f;
        Instantiate(bomb, pos, Quaternion.identity);
        gunner.stopFire();
    }
}
