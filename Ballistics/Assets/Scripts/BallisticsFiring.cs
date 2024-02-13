using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticsFiring : MonoBehaviour
{
	public GameObject target;
	public GameObject projectile;

	public MeshRenderer materialSet;
	public Material defaultMaterial;
	public Material firingFailMaterial;

	public float muzzleV;
	public float canonDelay = 2.0f;

	public bool maxTime = false;
	
	void Start()
	{
		materialSet.material = defaultMaterial;
		InvokeRepeating("LaunchProjectile", 2f, canonDelay);
	}

	public void LaunchProjectile()
	{
		Vector3 firingVector = calculateFiringSolution(target.transform.position);
		
		if (firingVector != Vector3.zero) {
			materialSet.material = defaultMaterial;

			Quaternion firingRot =  Quaternion.LookRotation(firingVector);
			firingRot.eulerAngles = new Vector3(firingRot.eulerAngles.x + 90, firingRot.eulerAngles.y, firingRot.eulerAngles.z);

			Rigidbody instance = Instantiate(projectile, transform.position, firingRot).GetComponent<Rigidbody>();
			instance.velocity = firingVector.normalized * muzzleV;
		}
		else materialSet.material = firingFailMaterial;
	}

	
	private Vector3 calculateFiringSolution(Vector3 end)
	{
		Vector3 gravity = Physics.gravity;
		Vector3 start = transform.position;

		// Calculate the vector from the target back to the start.
		Vector3 delta = end - start;

		// Calculate the real-valued a,b,c coefficients of a
		// conventional quadratic equation.
		float a = gravity.sqrMagnitude;
		float b = -4f * (Vector3.Dot(gravity, delta) + muzzleV * muzzleV);
		float c = 4f * delta.sqrMagnitude;

		// Check for no real solutions.
		float b2minus4ac = b * b - 4 * a * c;
		if (b2minus4ac < 0) {
			Debug.Log("No Real Solution");
			return Vector3.zero;
		}

		// Find the candidate times.
		float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b2minus4ac)) / (2 * a));
		float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b2minus4ac)) / (2 * a));

		// Find the time to target.
		float ttt;
		if (time0 < 0) {
			if (time1 < 0) {
				// We have no valid times.
				Debug.Log("No Valid Times");
				return Vector3.zero;
			}
			else ttt = time1;
		}
		else {
			if (time1 < 0) ttt = time1;
			else {
				if (maxTime) ttt = Mathf.Max(time0,time1);
				else ttt = Mathf.Min(time0,time1);
			}
		}

		// Return the firing vector.
		Vector3 vector = delta * 2 - gravity * (ttt * ttt);
		float scalar = 2 * muzzleV * ttt;
		return new Vector3(vector.x / scalar, vector.y / scalar, vector.z / scalar);
	}
}
