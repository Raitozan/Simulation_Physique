using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidManager : MonoBehaviour {

	const float G = 9.80665f;

	public List<Particle> particles;
	public GameObject particlePrefab;

	public float H;
	public float Rho0;
	public float K;
	public float KNear;

	float timer = 0.0f;
	float spawnTime = 0.01f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (timer <= 0.0f)
		{
			if (Input.GetMouseButton(0))
			{
				Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector3 instancePos = new Vector3(mp.x, mp.y, 0.0f);
				particles.Add(Instantiate(particlePrefab, instancePos, Quaternion.identity, transform).GetComponent<Particle>());
				WallsAvoidance();
				timer = spawnTime;
			}
		}
		else
			timer -= Time.deltaTime;
	}

	void FixedUpdate()
	{
		FluidUpdate();
	}

	public void FluidUpdate()
	{
		foreach(Particle p in particles)
		{
			Vector3 gravityForce = new Vector3(0.0f, -G, 0.0f);
			p.velocity = p.velocity + Time.fixedDeltaTime * gravityForce;
		}
		foreach(Particle p in particles)
		{
			p.previousPos = p.transform.position;
			p.transform.position = p.transform.position + Time.fixedDeltaTime * p.velocity;
		}

		WallsAvoidance();

		DoubleDensityRelaxation();

		foreach(Particle p in particles)
		{
			p.velocity = (p.transform.position - p.previousPos) / Time.fixedDeltaTime;
		}
	}

	public void WallsAvoidance()
	{
		foreach(Particle p in particles)
		{
			if (p.transform.position.x < -20.0f)
				p.transform.position = new Vector3(Random.Range(-20.0f, -19.9f), p.transform.position.y, 0.0f);
			else if (p.transform.position.x > 20.0f)
				p.transform.position = new Vector3(Random.Range(19.9f, 20.0f), p.transform.position.y, 0.0f);

			if (p.transform.position.y < -25.0f)
				p.transform.position = new Vector3(p.transform.position.x, Random.Range(-25.0f, -24.9f), 0.0f);
		}
	}

	public void DoubleDensityRelaxation()
	{
		foreach(Particle pi in particles)
		{
			float rho = 0.0f;
			float rhoNear = 0.0f;
			foreach(Particle pj in particles)
			{
				if (pj != pi)
				{
					Vector3 Rij = pj.transform.position - pi.transform.position;
					float q = Rij.magnitude / H;
					if (q < 1)
					{
						rho = rho + Mathf.Pow(1 - q, 2);
						rhoNear = rhoNear + Mathf.Pow(1 - q, 3);
					}
				}
			}
			float P = K * (rho - Rho0);
			float PNear = KNear * rhoNear;
			Vector3 di = Vector3.zero;
			foreach (Particle pj in particles)
			{
				if (pj != pi)
				{
					Vector3 Rij = pj.transform.position - pi.transform.position;
					float q = Rij.magnitude / H;
					if (q < 1)
					{
						Vector3 D = Mathf.Pow(Time.fixedDeltaTime, 2) * (P * (1 - q) + PNear * Mathf.Pow(1 - q, 2)) * Rij.normalized;
						pj.transform.position = pj.transform.position + (D / 2)/pj.mass;
						di = di - (D / 2)/pi.mass;
					}
				}
			}
			pi.transform.position = pi.transform.position + di;
		}
	}
}
