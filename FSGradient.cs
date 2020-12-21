using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FSGradient
{
	public float angle;

	public float Get(float x, float y)
	{
		Vector2 v = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
		return Mathf.Clamp01(Vector2.Dot(v, new Vector2(x, y)));
	}
}
