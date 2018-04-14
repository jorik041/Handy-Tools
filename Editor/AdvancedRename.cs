#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AdvancedRename : MonoBehaviour {

	public static void RenameNames(string newName, bool isSuffix, bool isPrefix, bool numberItems, bool sortInHierarchy, int padding, int startNum, int incrementBy){
		int indexCount = startNum;
		string padFormat = "";
		for (int padNum = 0; padNum < padding; padNum++) {
			padFormat += "0";
		}

		for (int itemIndex = 0; itemIndex < Selection.transforms.Length; itemIndex ++) {

			string thisItemName = Selection.transforms[itemIndex].name;

			// add name 
			if (!isPrefix && !isSuffix) {
				Debug.Log("Rename without suffix or prefix");
				Selection.transforms[itemIndex].name = newName;
			}
			else {
				if (isPrefix) {
					Debug.Log("rename with Prefix");
					Selection.transforms[itemIndex].name = newName + thisItemName;
				}
				if (isSuffix) {
					Debug.Log("rename with Suffix");
					Selection.transforms[itemIndex].name = thisItemName + newName;
				}
			}

			if (numberItems) {
				// add trailing number
				Selection.transforms[itemIndex].name += indexCount.ToString(padFormat);
				indexCount += incrementBy;
			}
		}

		if (sortInHierarchy) {
			for (int itemIndex = 0; itemIndex < Selection.transforms.Length; itemIndex ++) {
				Selection.transforms[itemIndex].SetSiblingIndex(itemIndex);
			}
		}
	}
}
public class RenameMaster : EditorWindow{
	public enum NameOperation
	{
		NewName,
		AddPrefix,
		AddSuffix
	}
	public NameOperation nameOperation;
	
	[MenuItem("Window/Advanced Rename")]
	public static void Init()
	{
		// Get existing open window or if none, make a new one:
		RenameMaster window = (RenameMaster)EditorWindow.GetWindow(typeof(RenameMaster));
		
		#if UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0
		window.title = "Rename";
		#else
		window.titleContent = new GUIContent ("Rename");
		#endif
		window.Show();
	}
	string rename = "";
	int numberPadding = 4;
	bool numberItems = true;
	bool sortInHierarchy = true;
	int startNumber = 0;
	int incrementBy = 1;
	
	void OnGUI()
	{
		GUILayout.Label("Rename Multiple Files", EditorStyles.boldLabel);
		GUILayout.Label("Note: Undo is not currently supported. Save your scene first!");
		rename = EditorGUILayout.TextField("New Name: ", rename);

		nameOperation = (NameOperation)EditorGUILayout.EnumPopup("Operation: ", nameOperation);

		numberItems = EditorGUILayout.Toggle("Number Items: ", numberItems);

		if (numberItems) {
			numberPadding = Mathf.Clamp(EditorGUILayout.IntField("Number Padding: ", numberPadding), 0, 10000);
			startNumber = EditorGUILayout.IntField("Start Number: ", startNumber);
			incrementBy = Mathf.Clamp(EditorGUILayout.IntField("Increment by (step): ", incrementBy), 1, 100000);
			sortInHierarchy = EditorGUILayout.Toggle("Sort in Hierarchy: ", sortInHierarchy);
		}

		if (Selection.transforms.Length > 0) {
			if (GUILayout.Button("Rename " + Selection.transforms.Length + " Item(s)"))
			{
				AdvancedRename.RenameNames(rename, nameOperation == NameOperation.AddSuffix, nameOperation == NameOperation.AddPrefix, numberItems, sortInHierarchy, numberPadding, startNumber, incrementBy);
			}
		}

		string sample = "";

		if (nameOperation == NameOperation.AddPrefix) {
			sample = rename + "[name]";
		}
		else if (nameOperation == NameOperation.AddSuffix) {
			sample = "[name]" + rename;
		}
		else if (nameOperation == NameOperation.NewName) {
			sample = rename;
		}

		if (numberItems) {
			string padFormat = "";
			for (int padNum = 0; padNum < numberPadding; padNum++) {
				padFormat += "0";
			}
			sample += startNumber.ToString(padFormat);
		}

		GUILayout.Space (10);

		GUILayout.Label("Preview: " + sample);
	}
}

#endif