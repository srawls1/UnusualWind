using System.IO;
using UnityEngine;
using UnityEditor;

public class NoiseGenerator : EditorWindow
{
	private float scale;
	private Vector2 offset;
	private Vector2Int resolution;
	private Texture2D texturePreview;

	[MenuItem("Window/Custom/Noise Generator")]
	public static void GetNoiseGeneratorWindow()
	{
		GetWindow(typeof(NoiseGenerator));
	}

	void OnGUI()
	{
		bool dirty = false;
		Vector2Int newResolution = EditorGUILayout.Vector2IntField("Resolution", resolution);
		float newScale = EditorGUILayout.FloatField("Scale", scale);
		Vector2 newOffset = EditorGUILayout.Vector2Field("Offset", offset);

		if (GUILayout.Button("Randomize Scale"))
		{
			newScale = Random.Range(0f, 10f);
		}

		if (GUILayout.Button("Randomize Offset"))
		{
			newOffset = Random.insideUnitCircle * 10f;
		}

		if (newResolution != resolution)
		{
			resolution = newResolution;
			dirty = true;
		}
		if (newScale != scale)
		{
			scale = newScale;
			dirty = true;
		}
		if (newOffset != offset)
		{
			offset = newOffset;
			dirty = true;
		}

		if (dirty)
		{
			texturePreview = GenerateTexture();
		}

		GUILayout.Label(AssetPreview.GetAssetPreview(texturePreview));

		if (GUILayout.Button("Save Texture"))
		{
			string path = EditorUtility.SaveFilePanelInProject("Select Texture Save Location", "Noise", "png", "Select where to save the texture");
			byte[] pngData = texturePreview.EncodeToPNG();
			File.WriteAllBytes(path, pngData);
			AssetDatabase.ImportAsset(path);
		}
	}

	private Texture2D GenerateTexture()
	{
		Texture2D texture = new Texture2D(resolution.x, 1);

		for (int x = 0; x < resolution.x; ++x)
		{
			float angleRadians = Mathf.PI * 2 * x / resolution.x;
			float noiseX = Mathf.Cos(angleRadians) * scale + offset.x;
			float noiseY = Mathf.Sin(angleRadians) * scale + offset.y;
			float noiseValue = Mathf.PerlinNoise(noiseX, noiseY);
			Color color = new Color(noiseValue, noiseValue, noiseValue);
			texture.SetPixel(x, 0, color);
		}

		//for (int x = 0; x < resolution.x; ++x)
		//{
		//	for (int y = 0; y < resolution.y; ++y)
		//	{
		//		float xNormalized = (float)x / resolution.x;
		//		float yNormalized = (float)y / resolution.y;
		//		float noiseValue = Mathf.PerlinNoise(xNormalized * scale + offset.x, yNormalized * scale + offset.y);
		//		Color color = new Color(noiseValue, noiseValue, noiseValue);
		//		texture.SetPixel(x, y, color);
		//	}
		//}

		texture.Apply();
		return texture;
	}
}