using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FastNoiseSettings))]
public class FastNoiseSettingsDrawer : PropertyDrawer
{
	int current_height = 0;
	const int fieldheight = 25;
	const int fieldwidth = 50;
	Texture2D preview;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		current_height = 0;

		GUIStyle headerStyle = new GUIStyle();
		headerStyle.alignment = TextAnchor.MiddleCenter;
		headerStyle.fontStyle = FontStyle.Bold;
		headerStyle.normal.textColor = Color.white;

		FastNoiseSettings target = fieldInfo.GetValue(property.serializedObject.targetObject) as FastNoiseSettings;

		EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUI.Foldout(new Rect( position.x, position.y,position.width, 25), property.isExpanded, label);

		if (property.isExpanded)
		{
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 90;

			EditorGUI.BeginChangeCheck();

			EditorGUI.LabelField(NextPropertyRect(position), "General", headerStyle);
			EditorGUI.PropertyField(NextPropertyRect(position), property.FindPropertyRelative("levels"));
			EditorGUI.PropertyField(NextPropertyRect(position), property.FindPropertyRelative("noiseType"));
			EditorGUI.PropertyField(NextPropertyRect(position), property.FindPropertyRelative("seed"));
			EditorGUI.PropertyField(NextPropertyRect(position), property.FindPropertyRelative("frequency"));

			EditorGUI.LabelField(NextPropertyRect(position), "Fractal", headerStyle);
			EditorGUI.BeginDisabledGroup(!((int)target.noiseType == 1 || (int)target.noiseType == 3 || (int)target.noiseType == 5 || (int)target.noiseType == 9));

			EditorGUI.PropertyField(NextPropertyRect(position), property.FindPropertyRelative("fractalType"));
			EditorGUI.PropertyField(NextPropertyRect(position), property.FindPropertyRelative("octaves"));
			EditorGUI.PropertyField(NextPropertyRect(position), property.FindPropertyRelative("gain"));
			EditorGUI.PropertyField(NextPropertyRect(position), property.FindPropertyRelative("lacunarity"));

			EditorGUI.EndDisabledGroup();

			bool changed = EditorGUI.EndChangeCheck();

			if(changed || preview == null)
			{
				preview = CreatePreviewTexture(target);
			}

			float s = Mathf.Min(position.width / 2f - 25, 200);
			EditorGUI.DrawPreviewTexture(new Rect(position.x, position.y + 50, s, s), preview);

			EditorGUI.indentLevel = indent;
			EditorGUIUtility.labelWidth = labelWidth;
			EditorGUI.EndFoldoutHeaderGroup();
			EditorGUI.EndProperty();
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return (property.isExpanded) ? 300 : 25;
	}

	public override bool CanCacheInspectorGUI(SerializedProperty property)
	{
		return false;
	}

	Rect NextPropertyRect(Rect position)
	{
		Rect r = new Rect(position.width / 2, position.y + current_height, position.width / 2f - 5, fieldheight);
		current_height += fieldheight;
		return r;
	}

	const int previewSize = 64;
	Texture2D CreatePreviewTexture(FastNoiseSettings fsSettings)
	{
		fsSettings.SetParams();
		Texture2D texture = new Texture2D(previewSize, previewSize);

		for(int x = 0; x < previewSize; x++)
		{
			for(int y = 0; y < previewSize; y++)
			{
				Color c = Color.white * fsSettings.Get((float)x / previewSize, (float)y / previewSize);
				texture.SetPixel(x, y, c);
			}
		}

		texture.Apply();
		return texture;
	}
}
