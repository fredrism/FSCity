using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FSTexture
{
	public static Texture2D CreateFromArray(bool[,] values, float scale = 1)
	{
		if (values == null)
		{
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, new Color(1, 0, 1));
			texture.Apply();
			return texture;
		}

		int w = values.GetLength(0);
		int h = values.GetLength(1);

		Texture2D tex = new Texture2D(w, h);

		for (int x = 0; x < w; x++)
		{
			for (int y = 0; y < h; y++)
			{
				tex.SetPixel(x, y, values[x,y] ? Color.white : Color.black);
			}
		}

		tex.Apply();
		return tex;
	}

	public static Texture2D CreateFromArray(float[,] values, float scale = 1)
	{
		if(values == null)
		{
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, new Color(1, 0, 1));
			texture.Apply();
			return texture;
		}

		int w = values.GetLength(0);
		int h = values.GetLength(1);

		Texture2D tex = new Texture2D(w, h);

		for(int x = 0; x < w; x++)
		{
			for(int y = 0; y < h; y++)
			{
				tex.SetPixel(x, y, Color.white * values[x, y] * scale);
			}
		}

		tex.Apply();
		return tex;
	}

	public static Texture2D CreateFromArray(Vector3[,] values, float scale = 1)
	{
		int w = values.GetLength(0);
		int h = values.GetLength(1);

		Texture2D tex = new Texture2D(w, h);

		for (int x = 0; x < w; x++)
		{
			for (int y = 0; y < h; y++)
			{
				Vector3 v = values[x, y];
				tex.SetPixel(x, y, new Color(v.x, v.y, v.z)*scale);
			}
		}

		tex.Apply();
		return tex;
	}
}
