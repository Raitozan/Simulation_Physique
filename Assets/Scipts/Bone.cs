using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour {

	Node NodeA;
	public Node NodeB;

	LineRenderer lr;

	Vector3 lastAPos;
	Vector3 lastBPos;

	// Use this for initialization
	void Start ()
	{
		NodeA = gameObject.GetComponentInParent<Node>();
		lr = gameObject.GetComponent<LineRenderer>();

		lr.SetPosition(0, NodeA.transform.position);
		lastAPos = NodeA.transform.position;
		lr.SetPosition(1, NodeB.transform.position);
		lastBPos = NodeB.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(lastAPos != NodeA.transform.position || lastBPos != NodeB.transform.position)
		{
			lr.SetPosition(0, NodeA.transform.position);
			lastAPos = NodeA.transform.position;
			lr.SetPosition(1, NodeB.transform.position);
			lastBPos = NodeB.transform.position;
		}
	}
}
