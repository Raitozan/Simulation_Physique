using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeChain : MonoBehaviour
{

	public List<Node> nodes;
	public List<Bone> bones;

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
		nodes = new List<Node>();
		bones = new List<Bone>();
		target = transform.position;
		Transform actualNode = transform;

		while (!actualNode.CompareTag("AnchorNode"))
		{
			nodes.Add(actualNode.GetComponent<Node>());
			bones.Add(actualNode.GetComponent<Bone>());
			actualNode = actualNode.parent;
		}
		nodes.Add(actualNode.GetComponent<Node>());
		bones.Add(actualNode.GetComponent<Bone>());

		lr = GetComponent<LineRenderer>();
		lr.positionCount = nodes.Count;
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
		Vector3 anchorPos = nodes[nodes.Count - 1].transform.position;

		nodes[0].transform.position = target;
		for (int i = 1; i < nodes.Count; i++)
		{
			nodes[i].transform.position = nodes[i - 1].transform.position + (nodes[i].transform.position - nodes[i - 1].transform.position).normalized * bones[i - 1].maxLength;
		}

		nodes.Reverse();
		bones.Reverse();

		nodes[0].transform.position = anchorPos;
		for (int i = 1; i < nodes.Count; i++)
		{
			nodes[i].transform.position = nodes[i - 1].transform.position + (nodes[i].transform.position - nodes[i - 1].transform.position).normalized * bones[i].maxLength;
		}

		nodes.Reverse();
		bones.Reverse();

		UpdateLineRenderer();
	}

	public void UpdateLineRenderer()
	{
		LineRenderer lr = GetComponent<LineRenderer>();
		lr.SetPosition(0, transform.position);
		int i = 0;
		foreach(Node node in nodes)
			lr.SetPosition(i++, node.transform.position);
	}
}
