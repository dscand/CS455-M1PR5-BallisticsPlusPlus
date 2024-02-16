using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSwitcher : MonoBehaviour
{
	public Material defaultMaterial;
	public Material hitMaterial;

	public Transform[] targetPlacements;
	private int index = 0;

	public float TimeToMove = 2.0f;

	void Start()
	{
		gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
		transform.position = targetPlacements[index].position;
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.CompareTag("projectile")) {
			StartCoroutine(switchTargetPlacement());
		}
	}

	IEnumerator switchTargetPlacement() 
	{
		gameObject.GetComponent<MeshRenderer>().material = hitMaterial;
		yield return new WaitForSeconds(1);

		index++;
		if (index > targetPlacements.Length - 1) index = 0;
		gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;


		Vector3 start = transform.position;
		Vector3 end =  targetPlacements[index].position;

		float t = 0;
		while (t < 1) {
			transform.position = Vector3.Lerp(start, end, t);
			t += Time.deltaTime / TimeToMove;
			yield return null;
		}
		transform.position = end;
	}
}
