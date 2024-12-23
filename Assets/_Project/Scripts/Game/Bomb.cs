using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public Rigidbody rb;
	public ParticleSystem particlePrefab;

	private void Reset()
	{
		rb = GetComponent<Rigidbody>();
	}
}