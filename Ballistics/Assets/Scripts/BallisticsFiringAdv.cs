using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticsFiringAdv : MonoBehaviour
{
	public GameObject target;
	public GameObject projectile;
	public GameObject cannon;

	public MeshRenderer materialSet;
	public Material defaultMaterial;
	public Material firingFailMaterial;

	public float muzzleV;
	public float canonDelay = 1f;

	public bool maxTime = false;

	private Nullable<Vector3> firingVector;
	
	void Start()
	{
		//Physics.gravity = new Vector3(0, -1f, 0);
		materialSet.material = defaultMaterial;
		InvokeRepeating("LaunchProjectile", 0.4f, canonDelay);
	}

	void Update()
	{
		if (target) firingVector = calculateFiringSolution(transform, muzzleV, target.transform.position, target.GetComponent<Rigidbody>().velocity, maxTime);

		if (firingVector != null) {
			Quaternion firingRot = Quaternion.LookRotation((Vector3)firingVector);
			firingRot.eulerAngles = new Vector3(firingRot.eulerAngles.x + 90, firingRot.eulerAngles.y, firingRot.eulerAngles.z);
			cannon.transform.rotation = firingRot;
		}
	}

	public void LaunchProjectile()
	{
		if (target) {
			if (firingVector != null) {
				materialSet.material = defaultMaterial;

				Quaternion firingRot =  Quaternion.LookRotation((Vector3)firingVector);
				firingRot.eulerAngles = new Vector3(firingRot.eulerAngles.x + 90, firingRot.eulerAngles.y, firingRot.eulerAngles.z);

				Rigidbody instance = Instantiate(projectile, transform.position, firingRot).GetComponent<Rigidbody>();
				instance.velocity = ((Vector3)firingVector).normalized * muzzleV;
			}
			else materialSet.material = firingFailMaterial;
		}
	}

	public static Nullable<Vector3> calculateFiringSolution(Transform transform, float muzzleV, Vector3 pos, Vector3 vel, bool maxTime = false)
	{
		Vector3 gravity = Physics.gravity;
		Vector3 start = transform.position;

		Vector3 P = pos - start;

		float a = vel.sqrMagnitude - muzzleV*muzzleV;
		float b = 2f * Vector3.Dot(pos, vel);
		float c = P.sqrMagnitude;

		// Check for no real solutions.
		float b2minus4ac = b * b - 4 * a * c;
		if (b2minus4ac < 0) {
			Debug.Log("No Real Solution");
			return null;
		}

		// Find the candidate times.
		float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b2minus4ac)) / (2 * a));
		float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b2minus4ac)) / (2 * a));

		// Find the time to target.
		float ttt;
		if (time0 < 0 || float.IsNaN(time0)) {
			if (time1 < 0 || float.IsNaN(time1)) {
				// We have no valid times.
				Debug.Log("No Valid Times");
				return null;
			}
			else ttt = time1;
		}
		else {
			if (time1 < 0 || float.IsNaN(time1)) ttt = time0;
			else {
				if (maxTime) ttt = Mathf.Max(time0,time1);
				else ttt = Mathf.Min(time0,time1);
			}
		}

		// Return the firing vector.
		Vector3 bullet_vector = pos + vel*ttt + gravity/2 * (ttt * ttt);
		return bullet_vector.normalized;
	}
}
