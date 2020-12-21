using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSCity : MonoBehaviour
{
	public FastNoiseSettings heightSettings;
	public FSGradient heightGradient;

	public float waterLevel;

	public int resolution = 256;
	public ImageMap previewTexture;

	float[,] heightmap;
	bool[,] landMap;


	public enum ImageMap
	{
		Height, LandMass
	}

	public void Generate()
	{
		FillHeightMap();
	}

	void FillHeightMap()
	{
		heightmap = new float[resolution, resolution];
		heightSettings.SetParams();

		landMap = new bool[resolution, resolution];

		for(int x = 0; x < resolution; x++)
		{
			for(int y = 0; y < resolution; y++)
			{
				float tx = (float)x / resolution;
				float ty = (float)y / resolution;

				float v = heightSettings.Get(tx, ty) * heightGradient.Get(tx, ty);

				heightmap[x, y] = v;
				landMap[x,y] = v > waterLevel;
			}
		}
	}

	public void UpdatePreview()
	{
		GameObject preview = GameObject.Find("city_preview");
		if(preview == null)
		{
			preview = GameObject.CreatePrimitive(PrimitiveType.Plane);
			preview.transform.parent = transform;
			preview.GetComponent<MeshRenderer>().sharedMaterial.shader = Shader.Find("Unlit/Texture");
			preview.name = "city_preview";
		}

		Material mat = preview.GetComponent<MeshRenderer>().sharedMaterial;

		switch (previewTexture)
		{
			case (ImageMap.Height):
				mat.mainTexture = FSTexture.CreateFromArray(heightmap);
				break;
			case (ImageMap.LandMass):
				mat.mainTexture = FSTexture.CreateFromArray(landMap);
				break;
		}
	}


}
