using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeChain : MonoBehaviour
{

	List<Transform> nodes;
	List<float> lengths;

	Vector3 target;

	LineRenderer lr;

	// Use this for initialization
	void Start()
	{
		Init();
	}

	// Update is called once per frame
	void Update()
	{
		UpdateTarget();
		if (transform.position != target)
			MoveChain();
	}

	public void Init()
	{
		nodes = new List<Transform>();
		lengths = new List<float>();
		target = transform.position;
		Transform actualNode = transform;

		while (!actualNode.CompareTag("AnchorNode"))
		{
			Transform lastNode = actualNode;
			actualNode = actualNode.parent;

			lengths.Add(Vector3.Distance(lastNode.position, actualNode.position));

			nodes.Add(actualNode);
		}

		lr = GetComponent<LineRenderer>();
		lr.positionCount = nodes.Count + 1;
		UpdateLineRenderer();
	}

	public void UpdateTarget()
	{
		if (Input.GetMouseButton(0))
		{
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target.z = 0.0f;
		}
	}

	public void MoveChain()
	{
		Vector3 anchorPos = nodes[nodes.Count - 1].position;

		transform.position = target;
		Vector3 lastNode = target;
		for (int i = 0; i < nodes.Count; i++)
		{
			nodes[i].position = lastNode + (nodes[i].position - lastNode).normalized * lengths[i];
			lastNode = nodes[i].position;
		}

		nodes.Reverse();
		lengths.Reverse();

		nodes[0].position = anchorPos;
		lastNode = anchorPos;
		for (int i = 1; i < nodes.Count; i++)
		{
			nodes[i].position = lastNode + (nodes[i].position - lastNode).normalized * lengths[i - 1];
			lastNode = nodes[i].position;
		}
		transform.position = lastNode + (transform.position - lastNode).normalized * lengths[lengths.Count - 1];

		nodes.Reverse();
		lengths.Reverse();

		UpdateLineRenderer();
	}

	public void UpdateLineRenderer()
	{
		LineRenderer lr = GetComponent<LineRenderer>();
		lr.SetPosition(0, transform.position);
		int i = 1;
		foreach(Transform node in nodes)
			lr.SetPosition(i++, node.position);
	}
}
