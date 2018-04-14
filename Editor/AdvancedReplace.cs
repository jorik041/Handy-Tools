#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Handy {

	public class AdvancedReplace : EditorWindow {
		
		public enum SearchType
		{
			SearchByName = 0,
			SearchByTag = 1
		}
		public SearchType searchType;
		
		[MenuItem("Window/Advanced Replace")]
		public static void Init()
		{
			// Get existing open window or if none, make a new one:
			AdvancedReplace window = (AdvancedReplace)EditorWindow.GetWindow(typeof(AdvancedReplace));
			
			#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
			window.title = "Replace Objects";
			#else
			window.titleContent = new GUIContent ("Replace Objects");
			#endif
			window.Show();
		}
		
		string find = "";
		GameObject replace;
		bool selected = true;
		bool byTag = false;
		
		void OnGUI()
		{
			GUILayout.Label("Find and Replace with GameObjects", EditorStyles.boldLabel);
			GUILayout.Label("Note: Undo is not currently supported. Save your scene first!");
			find = EditorGUILayout.TextField("Search for: ", find);
			searchType = (SearchType)EditorGUILayout.EnumPopup("Search by: ", searchType);
			replace = EditorGUILayout.ObjectField("Replace with: ", replace, typeof(GameObject), true) as GameObject;
			selected = EditorGUILayout.Toggle("Search within selection: ", selected);
			
			if (GUILayout.Button("Find and Replace GameObjects"))
			{
				byTag = searchType == SearchType.SearchByTag;
				FindReplaceObjects(find, replace, selected, byTag);
			}
			
			GUILayout.Space (10);
			
			if (Selection.transforms.Length > 0 && find != "") {
				GUILayout.BeginVertical ();
				
				string replaceText = "<select a prefab>";
				if (replace != null) {
					replaceText = replace.name;
				}
				
				GUILayout.Label ("Replace these with " + replaceText, EditorStyles.boldLabel);
				
				foreach (Transform t in Selection.transforms) {
					GUILayout.Label (t.gameObject.name);
				}
				GUILayout.EndVertical ();
			}
		}

		public static void FindReplaceNames(string find, string replace, bool withinSelection)
		{

			if (withinSelection)
			{
				if (Selection.gameObjects.Length > 0)
				{
					Undo.RecordObjects(Selection.gameObjects, "Find Replace Names");
					foreach (GameObject gO in Selection.gameObjects)
					{
						gO.name = gO.name.Replace(find, replace);
					}
				}
			}
			else
			{
				//Undo.RecordObjects(Selection.gameObjects, "Find Replace Names");
				foreach (GameObject gO in FindObjectsOfType<GameObject>())
				{
					gO.name = gO.name.Replace(find, replace);
				}
			}
		}

		public static void FindReplaceObjects(string find, GameObject replace, bool withinSelection, bool byTag)
		{
			if (withinSelection)
			{
				if (Selection.gameObjects.Length > 0)
				{
					Undo.RecordObjects(Selection.gameObjects, "Find Replace Objects");
					foreach (GameObject gO in Selection.gameObjects)
					{
						if (!byTag && gO.name.Contains(find))
						{
							GameObject newObj = Instantiate(replace, gO.transform.position, gO.transform.rotation) as GameObject;
							newObj.transform.parent = gO.transform.parent;
							newObj.name = newObj.name.Replace("(Clone)", "");
							DestroyImmediate(gO);
						}
						else if (byTag && gO.tag.Contains(find))
						{
							GameObject newObj = Instantiate(replace, gO.transform.position, gO.transform.rotation) as GameObject;
							newObj.transform.parent = gO.transform.parent;
							newObj.name = newObj.name.Replace("(Clone)", "");
							DestroyImmediate(gO);
						}
					}
				}
			}
			else
			{
				//Undo.RecordObjects(Selection.gameObjects, "Find Replace Objects");
				foreach (GameObject gO in FindObjectsOfType<GameObject>())
				{
					if (!byTag && gO.name.Contains(find))
					{
						GameObject newObj = Instantiate(replace, gO.transform.position, gO.transform.rotation) as GameObject;
						newObj.transform.parent = gO.transform.parent;
						newObj.name = newObj.name.Replace("(Clone)", "");
						DestroyImmediate(gO);
					}
					else if (byTag && gO.tag.Contains(find))
					{
						GameObject newObj = Instantiate(replace, gO.transform.position, gO.transform.rotation) as GameObject;
						newObj.transform.parent = gO.transform.parent;
						newObj.name = newObj.name.Replace("(Clone)", "");
						DestroyImmediate(gO);
					}
				}
			}
		}
	}
}
#endif
