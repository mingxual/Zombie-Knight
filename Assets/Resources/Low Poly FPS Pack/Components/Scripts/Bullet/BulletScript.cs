using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	[Range(5, 100)]
	[Tooltip("After how long time should the bullet prefab be destroyed?")]
	public float destroyAfter;
	[Tooltip("If enabled the bullet destroys on impact")]
	public bool destroyOnImpact = false;
	[Tooltip("Minimum time after impact that the bullet is destroyed")]
	public float minDestroyTime;
	[Tooltip("Maximum time after impact that the bullet is destroyed")]
	public float maxDestroyTime;
	[Tooltip("Damage point associated with the bullet")]
	public int damagePoints;

	[Header("Impact Effect Prefabs")]
	public Transform [] bloodImpactPrefabs;
	public Transform [] metalImpactPrefabs;
	public Transform [] dirtImpactPrefabs;
	public Transform []	concreteImpactPrefabs;

	private void Start () 
	{
		//Start destroy timer
		StartCoroutine (DestroyAfter ());
		gameObject.layer = 16;
	}

	//If the bullet collides with anything
	private void OnCollisionEnter (Collision collision) 
	{
		//If destroy on impact is false, start 
		//coroutine with random destroy timer
		if (!destroyOnImpact) 
		{
			StartCoroutine (DestroyTimer ());
		}
		//Otherwise, destroy bullet on impact
		else 
		{
			Destroy (gameObject);
		}

		if (collision.collider.gameObject.tag == "ZombieHead")
		{
			collision.gameObject.GetComponent<EnemyHealth>().GetDamage(damagePoints * 2, true);

			if (collision.gameObject.GetComponent<MonsterZombieControl>() != null)
				collision.gameObject.GetComponent<MonsterZombieControl>().getHit();

			Instantiate(bloodImpactPrefabs[Random.Range
				(0, bloodImpactPrefabs.Length)], transform.position,
				Quaternion.LookRotation(collision.contacts[0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		if (collision.collider.gameObject.tag == "ZombieBody")
		{
			collision.gameObject.GetComponent<EnemyHealth>().GetDamage(damagePoints, false);

			if (collision.gameObject.GetComponent<MonsterZombieControl>() != null)
				collision.gameObject.GetComponent<MonsterZombieControl>().getHit();

			collision.gameObject.GetComponent<Rigidbody>().AddForce
				(transform.forward * 1000, ForceMode.Impulse);

			Instantiate(bloodImpactPrefabs[Random.Range
				(0, bloodImpactPrefabs.Length)], transform.position,
				Quaternion.LookRotation(collision.contacts[0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		if (collision.collider.gameObject.tag == "Blood")
		{
			Instantiate(bloodImpactPrefabs[Random.Range
				(0, bloodImpactPrefabs.Length)], transform.position,
				Quaternion.LookRotation(collision.contacts[0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		//Ignore collision if bullet collides with "Player" tag
		if (collision.gameObject.tag == "Player") 
		{
			//Physics.IgnoreCollision (collision.collider);
			Debug.LogWarning("Collides with player");
			//Physics.IgnoreCollision(GetComponent<Collider>(), GetComponent<Collider>());

			Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
		}

		//If bullet collides with "Enemy" tag
		/*if(collision.transform.tag == "Enemy")
        {
			if (collision.gameObject.GetComponent<EnemyHealth>() != null)
			{
				//Trigger the EnemyHealth script in the other side
				collision.gameObject.GetComponent<EnemyHealth>().GetDamage(damagePoints);
			}
			else if(collision.gameObject.GetComponent<FatZombieLogic>() != null)
            {
				collision.gameObject.GetComponent<FatZombieLogic>().GetDamage(damagePoints);
			}
			//Destroy bullet object
			Destroy(gameObject);
        }*/

		//If bullet collides with "Blood" tag
		if (collision.transform.tag == "Blood") 
		{
			//Instantiate random impact prefab from array
			Instantiate (bloodImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		//If bullet collides with "Metal" tag
		if (collision.transform.tag == "Metal") 
		{
			//Instantiate random impact prefab from array
			Instantiate (metalImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		//If bullet collides with "Dirt" tag
		if (collision.transform.tag == "Dirt" || collision.transform.tag == "Terrain") 
		{
			//Instantiate random impact prefab from array
			Instantiate (dirtImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		//If bullet collides with "Concrete" tag
		if (isConcrete(collision.transform.tag)) 
		{
			//Instantiate random impact prefab from array
			Instantiate (concreteImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}

		//If bullet collides with "Target" tag
		if (collision.transform.tag == "Target") 
		{
			//Toggle "isHit" on target object
			collision.transform.gameObject.GetComponent
				<TargetScript>().isHit = true;
			//Destroy bullet object
			Destroy(gameObject);
		}
			
		//If bullet collides with "ExplosiveBarrel" tag
		if (collision.transform.tag == "ExplosiveBarrel") 
		{
			//Toggle "explode" on explosive barrel object
			collision.transform.gameObject.GetComponent
				<ExplosiveBarrelScript>().explode = true;
			//Destroy bullet object
			Destroy(gameObject);
		}

		//If bullet collides with "GasTank" tag
		if (collision.transform.tag == "GasTank") 
		{
			//Toggle "isHit" on gas tank object
			collision.transform.gameObject.GetComponent
				<GasTankScript> ().isHit = true;
			//Destroy bullet object
			Destroy(gameObject);
		}
	}

	private IEnumerator DestroyTimer () 
	{
		//Wait random time based on min and max values
		yield return new WaitForSeconds
			(Random.Range(minDestroyTime, maxDestroyTime));
		//Destroy bullet object
		Destroy(gameObject);
	}

	private IEnumerator DestroyAfter () 
	{
		//Wait for set amount of time
		yield return new WaitForSeconds (destroyAfter);
		//Destroy bullet object
		Destroy (gameObject);
	}

	private bool isConcrete(string tag)
	{
		return tag == "Concrete" || tag == "Wall" || tag == "SideWall";
	}
}