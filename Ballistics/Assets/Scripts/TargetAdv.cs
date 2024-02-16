using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TargetAdv : MonoBehaviour
{
	private Rigidbody rb;

	public Material defaultMaterial;
	public Material hitMaterial;

	public Transform target;

	public float MuzzleV = 20.0f;

	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody>();

		gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;

		Nullable<Vector3> firingVector = BallisticsFiring.calculateFiringSolution(transform, MuzzleV, target.transform.position, true);

		if (!firingVector.IsUnityNull()) {
			rb.velocity = ((Vector3)firingVector).normalized * MuzzleV;
		}
		else StartCoroutine(DeleteProjectile());
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
