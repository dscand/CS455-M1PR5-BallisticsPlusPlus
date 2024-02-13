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
		transform.position = targetPlacements[index].position;
		gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;

		yield return null;
	}
}
