using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public Rigidbody rb;

	private bool guide = true;

	void Start()
	{
		guide = true;
		StartCoroutine(DeleteProjectile());
	}

	void Update()
	{
		if (guide) {
			Quaternion velocityRot = Quaternion.LookRotation(rb.velocity);
			velocityRot.eulerAngles = new Vector3(velocityRot.eulerAngles.x + 90, velocityRot.eulerAngles.y, velocityRot.eulerAngles.z);
			transform.rotation = velocityRot;
		}
	}

	IEnumerator DeleteProjectile() 
	{
		yield return new WaitForSeconds(8);

		Destroy(gameObject);
		yield return null;
	}

	void OnCollisionEnter(Collision collision)
	{
		guide = false;
	}
}
