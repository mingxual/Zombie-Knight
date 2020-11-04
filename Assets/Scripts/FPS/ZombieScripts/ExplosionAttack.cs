using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAttack : MonoBehaviour
{
	[Header("Customizable Options")]
	//How long before the explosion prefab is destroyed
	public float despawnTime = 10.0f;
	//How long the light flash is visible
	public float lightDuration = 0.02f;
	[Header("Light")]
	public Light lightFlash;
	[Header("Collider")]
	public Collider collider;

	[Header("Audio")]
	public AudioClip[] explosionSounds;
	public AudioSource audioSource;

	private void Start()
	{
		//Start the coroutines
		StartCoroutine(DestroyTimer());
		StartCoroutine(LightFlash());

		//Get a random impact sound from the array
		audioSource.clip = explosionSounds
			[Random.Range(0, explosionSounds.Length)];
		//Play the random explosion sound
		audioSource.Play();
	}

	private IEnumerator LightFlash()
	{
		//Show the light
		lightFlash.GetComponent<Light>().enabled = true;
		//Wait for set amount of time
		yield return new WaitForSeconds(lightDuration);
		//Hide the light
		lightFlash.GetComponent<Light>().enabled = false;
		collider.enabled = false;
	}

	private IEnumerator DestroyTimer()
	{
		//Destroy the explosion prefab after set amount of seconds
		yield return new WaitForSeconds(despawnTime);
		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<PlayerHealth>().getHarm(50, false);
		}
		else if (other.tag == "Wall" || other.tag == "SideWall" || other.tag == "Spike")
		{
			other.gameObject.GetComponent<ObjectUpdate>().Destroy();
		}
	}
}
