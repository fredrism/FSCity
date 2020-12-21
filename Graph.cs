using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Graph
{
	public class Node
	{
		public List<Edge> edges;
		public Vector3 position;

		public Node(Vector3 position)
		{
			this.position = position;
		}
	}

	public class Edge
	{
		public readonly Node A, B;
		public readonly float Length;

		public Edge(Node A, Node B)
		{
			this.A = A;
			this.B = B;

			Length = Vector3.Distance(A.position, B.position);
		}

		public Node Other(Node n)
		{
			if(n == A)
			{
				return B;
			}
			return A;
		}

		public bool Contains(Node n)
		{
			return (A == n || B == n);
		}
	}

	List<Node> nodes = new List<Node>();
	List<Edge> edges = new List<Edge>();

	public Node AddNode(Vector3 pos, Node root = null)
	{
		Node n = new Node(pos);
		nodes.Add(n);

		if(root != null)
		{
			AddEdge(n, root);
		}

		return n;
	}

	public Edge AddEdge(Node A, Node B)
	{
		Edge e = new Edge(A, B);

		A.edges.Add(e);
		B.edges.Add(e);
		edges.Add(e);

		return e;
	}

	public void SplitEdge(Node n, Edge e)
	{
		DeleteEdge(e);
		AddEdge(e.A, n);
		AddEdge(n, e.B);
	}

	public enum MergeMode
	{
		Center, First, Last
	}
	public void Merge(Node A, Node B, MergeMode mode = MergeMode.Center)
	{
		Vector3 pos = A.position;
		if(mode == MergeMode.Center)
		{
			pos = Vector3.Lerp(A.position, B.position, 0.5f);
		}
		else if(mode == MergeMode.Last)
		{
			pos = B.position;
		}

		List<Node> connections = new List<Node>();

		foreach(Edge e in A.edges)
		{
			if(e.Other(A) != B)
			{
				connections.Add(e.Other(B));
			}
		}

		foreach (Edge e in B.edges)
		{
			if (e.Other(B) != A)
			{
				connections.Add(e.Other(A));
			}
		}

		DeleteNode(A);
		DeleteNode(B);

		Node n0 = AddNode(pos);

		foreach(Node n1 in connections)
		{
			AddEdge(n0, n1);
		}
	}

	public void DeleteNode(Node n)
	{
		nodes.Remove(n);

		List<Edge> toRemove = edges.FindAll(x => x.Contains(n));

		foreach(Edge e in toRemove)
		{
			DeleteEdge(e);
		}

	}

	public void DeleteEdge(Edge e)
	{
		e.A.edges.Remove(e);
		e.B.edges.Remove(e);
		edges.Remove(e);
	}

	public int DeleteLoose()
	{
		return nodes.RemoveAll(x => x.edges.Count == 0);
	}

	public (float, Vector3) ClosestPointOnEdge(Edge e, Vector3 v)
	{
		Vector3 m = e.B.position - e.A.position;
		float t0 = Vector3.Dot(m, v - e.A.position);
		Vector3 intersect = e.A.position + t0 * m;

		return (Vector3.Distance(v, intersect), intersect);
	}

	public Node ClosestNode(Vector3 v)
	{
		float dist = float.MaxValue;
		Node closest = null;

		foreach(Node n in nodes)
		{
			if(Vector3.Distance(n.position, v) < dist)
			{
				dist = Vector3.Distance(n.position, v);
				closest = n;
			}
		}

		return closest;
	}

	public (float, Vector3, Edge) ClosestPointOnGraph(Vector3 v)
	{
		float closestDistance = float.MaxValue;
		Vector3 closestPoint = new Vector3();
		Edge closestEdge = null;

		foreach(Edge e in edges)
		{
			(float, Vector3) dist = ClosestPointOnEdge(e, v);
			{
				if(dist.Item1 < closestDistance)
				{
					closestDistance = dist.Item1;
					closestPoint = dist.Item2;
					closestEdge = e;
				}
			}
		}

		return (closestDistance, closestPoint, closestEdge);
	}
}
