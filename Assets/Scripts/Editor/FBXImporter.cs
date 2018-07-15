using System;
using UnityEditor;
using UnityEngine;

public class FBXImporter : AssetPostprocessor
{
	void OnPreprocessModel ()
	{
		ModelImporter importer = assetImporter as ModelImporter;
		String name = importer.assetPath.ToLower ();
		if (name.Substring (name.Length - 4, 4) == ".fbx")
		{
			importer.useFileScale = true;
			importer.isReadable = false;
			importer.importCameras = false;
			importer.importBlendShapes = false;
			importer.importLights = false;
			// importer.importAnimation = false;
			// importer.importMaterials = false;
		}
	}
}