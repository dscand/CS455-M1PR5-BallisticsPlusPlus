using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AutoTargetCannon : MonoBehaviour
{
	public BallisticsFiringAdv ballistics;

	public Material noTargetsMaterial;
	
	void Update()
	{
		if (!ballistics.target) {
			List<GameObject> targets = GameObject.FindGameObjectsWithTag("target").ToList();
			targets = targets.OrderBy(t => Vector3.Distance(t.transform.position, transform.position)).ToList();

			if (targets.Count <= 0) ballistics.materialSet.material = noTargetsMaterial;
			else
			{
				ballistics.materialSet.material = ballistics.defaultMaterial;
				ballistics.target = targets[0];
			}
		}
	}
}
