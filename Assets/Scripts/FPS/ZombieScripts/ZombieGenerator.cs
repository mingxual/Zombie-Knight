using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGenerator : MonoBehaviour
{
    public List<GameObject> zombies;

    public PlayerHealth playerHealth;
    public Transform player;
    public Utility utility;

    public SpeedUp playerSpeedUp;

    public Transform enemyParent;

    private int index;

    public float range;

    void Start()
    {
        index = 0;
        InvokeRepeating("GenerateNext", 7.5f, 1.5f);
    }

    private void GenerateNext()
    {
        if(index < zombies.Count)
        {
            Vector3 offset = new Vector3(Random.Range(-range, range), 0.0f, Random.Range(-range, range));
            GameObject zombie = Instantiate(zombies[index], transform.position + offset, transform.rotation);
            zombie.transform.SetParent(enemyParent);
            /*EnemyMoveControl enemy = zombie.GetComponent<EnemyMoveControl>();
            enemy.playerHealth = playerHealth;
            enemy.utility = utility;
            enemy.Player = player;*/
            zombie.GetComponent<updateMaterial>().playerSpeedUp = playerSpeedUp;
            ++index;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
