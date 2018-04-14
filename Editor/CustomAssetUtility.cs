#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Handy {

    /// <summary>
    /// Utility for creating ScriptableObject in Unity
    /// </summary>
	public static class CustomAssetUtility
	{
		/// <typeparam name="T"></typeparam>
		public static void CreateAsset<T>() where T : ScriptableObject {
			T asset = ScriptableObject.CreateInstance<T>();

			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (path == "") {
				path = "Assets";
			}
			else if (Path.GetExtension(path) != "") {
				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			}

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");
			Debug.Log("Created new asset at: " + assetPathAndName);
			AssetDatabase.CreateAsset(asset, assetPathAndName);

			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

		/// <typeparam name="T"></typeparam>
		public static void CreateAsset<T>(string path) where T : ScriptableObject {
			T asset = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

		/// <typeparam name="T"></typeparam>
		public static T CreateAndGetAsset<T>(string path) where T : ScriptableObject {
			T asset = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
			return asset;
		}
	}
}
#endif
