using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour
{
	public Renderer render;

	private void Reset() {
		render = GetComponentInChildren<Renderer>();
	}
}

