using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FSCity))]
public class FSCityEditor : Editor
{
	FSCity.ImageMap preview;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		FSCity city = target as FSCity;

		if(GUILayout.Button("Generate"))
		{
			city.Generate();
		}

		if(city.previewTexture != preview)
		{
			city.UpdatePreview();
			preview = city.previewTexture;
		}
	}
}
