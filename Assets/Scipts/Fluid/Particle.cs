using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {
	public float mass;
	public Vector3 previousPos = Vector3.zero;
	public Vector3 velocity = Vector3.zero;

	void Start()
	{
		mass = Random.Range(1.0f, 3.0f);
		GetComponent<SpriteRenderer>().color = Color.Lerp(Color.green, Color.red, (mass - 1.0f) / 2.0f);
	}
}
