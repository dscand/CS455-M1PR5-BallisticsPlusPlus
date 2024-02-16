using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TargetRb : MonoBehaviour
{
	private Rigidbody rb;

	public Material defaultMaterial;
	public Material hitMaterial;

	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody>();

		gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag("projectile")) {
			gameObject.GetComponent<MeshRenderer>().material = hitMaterial;
			StartCoroutine(DeleteProjectile());
		}
	}
	void OnCollisionEnter(Collision collision)
	{
		OnTriggerEnter(collision.collider);
	}

	IEnumerator DeleteProjectile() 
	{
		yield return new WaitForSeconds(0.4f);

		Destroy(gameObject);
		yield return null;
	}
}
