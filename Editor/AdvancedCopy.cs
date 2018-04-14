using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdvancedCopy : EditorWindow {

	// settings
	private bool copyWorldSpace = true;
	private bool pasteWorldSpace = true;
	private bool showLog = true;

	[MenuItem("Window/Advanced Copy")]
	public static void OpenWindow() {
		AdvancedCopy window = (AdvancedCopy)EditorWindow.GetWindow(typeof(AdvancedCopy));
		window.titleContent = new GUIContent ("Advanced Copy");
		window.Show();
	}
	
	// width of the label to the left of buttons (transform, position, rotation, scale)
	private int leftLabelWidth = 80;
	private Vector2 scrollView;

	// force OnGUI to continue updating when unfocused so we can see it update while we select objects etc
	void Update() {
		Repaint();
	}
	
	// window ui
	void OnGUI(){
		if (Selection.transforms.Length == 0){
			GUILayout.BeginVertical();
			GUILayout.Label("Select one or more objects to begin", EditorStyles.helpBox);
			GUILayout.EndVertical();
		}
		if (Selection.transforms.Length > 0){
			GUILayout.BeginVertical();
			GUILayout.Label("First choose an attribute to copy, then paste it where you'd like it's value to go. You can copy and paste in local or world co-ordinates, or mix and match.", EditorStyles.helpBox);
			copyWorldSpace = GUILayout.Toggle(copyWorldSpace, "Copy in World Space");
			pasteWorldSpace = GUILayout.Toggle(pasteWorldSpace, "Paste in World Space");
			showLog = GUILayout.Toggle(showLog, "Output actions to Log");

			// start a scroll view that loops through all selected transforms
			// and creates gui elements to copy/paste attributes for them
			scrollView = GUILayout.BeginScrollView(scrollView, false, true);
			foreach (Transform selectedTransform in Selection.transforms){
				GUILayout.BeginVertical("Box");

				GUILayout.Label(selectedTransform.name, EditorStyles.boldLabel);

				GUILayout.BeginHorizontal();
				GUILayout.Label("Transform", GUILayout.Width(leftLabelWidth));
				if (GUILayout.Button("Copy")){
					CopyFrom(selectedTransform, CopyOptions.Transform, copyWorldSpace);
				}
				if (GUILayout.Button("Paste")){
					PasteTo(selectedTransform, CopyOptions.Transform, pasteWorldSpace);
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Position", GUILayout.Width(leftLabelWidth));
				if (GUILayout.Button("Copy")){
					CopyFrom(selectedTransform, CopyOptions.Position, copyWorldSpace);
				}
				if (GUILayout.Button("Paste")){
					PasteTo(selectedTransform, CopyOptions.Position, pasteWorldSpace);
				}
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				GUILayout.Label("Rotation", GUILayout.Width(leftLabelWidth));
				if (GUILayout.Button("Copy")){
					CopyFrom(selectedTransform, CopyOptions.Rotation, copyWorldSpace);
				}
				if (GUILayout.Button("Paste")){
					PasteTo(selectedTransform, CopyOptions.Rotation, pasteWorldSpace);
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("Scale", GUILayout.Width(leftLabelWidth));
				if (GUILayout.Button("Copy")){
					CopyFrom(selectedTransform, CopyOptions.Scale, copyWorldSpace);
				}
				if (GUILayout.Button("Paste")){
					PasteTo(selectedTransform, CopyOptions.Scale, pasteWorldSpace);
				}
				GUILayout.EndHorizontal();
				
				GUILayout.EndVertical();
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
		}
	}

	// store our copied variables in these vars
	private Vector3 savedPos;
	private Vector3 savedScale;
	private Quaternion savedRotation;

	// represent bool worldspace in a nice english way for debug logs
	private string WorldSpaceBoolToText(bool worldspace){
		if (worldspace){
			return " in world space";
		}
		else{
			return " in local space";
		}
	}

	// copy the defined transform's attributes
	public void CopyFrom(Transform fromTarget, CopyOptions copyOptions, bool useWorldSpace){
		// decide which copy channels to save
		bool copyPos = copyOptions == CopyOptions.Transform || copyOptions == CopyOptions.Position;
		bool copyRot = copyOptions == CopyOptions.Transform || copyOptions == CopyOptions.Rotation;
		bool Scale = copyOptions == CopyOptions.Transform || copyOptions == CopyOptions.Scale;

		// apply the values 
		if (copyPos){
			savedPos = useWorldSpace ? fromTarget.position : fromTarget.localPosition;
		}
		if (copyRot){
			savedRotation = useWorldSpace ? fromTarget.rotation : fromTarget.localRotation;
		}
		if (Scale){
			savedScale = useWorldSpace ? fromTarget.lossyScale : fromTarget.localScale;
		}

		// tell us what we did
		if (showLog){
			Debug.Log("<b>Advanced Copy:</b> Copied " + copyOptions + " from <b>" + fromTarget.name + "</b>" + WorldSpaceBoolToText(useWorldSpace));
		}	
	}

	// paste saved attributes to the defined target
	public void PasteTo(Transform toTarget, CopyOptions copyOptions, bool useWorldSpace){

		Undo.RecordObject(toTarget, "Advanced Paste");

		// decide which paste channels to apply
		bool pastePos = copyOptions == CopyOptions.Transform || copyOptions == CopyOptions.Position;
		bool pasteRot = copyOptions == CopyOptions.Transform || copyOptions == CopyOptions.Rotation;
		bool pasteScale = copyOptions == CopyOptions.Transform || copyOptions == CopyOptions.Scale;

		// apply the values 
		if (pastePos){
			if (useWorldSpace){
				toTarget.position = savedPos;
			}
			else{
				toTarget.localPosition = savedPos;
			}
		}
		if (pasteRot){
			if (useWorldSpace){
				toTarget.rotation = savedRotation;
			}
			else{
				toTarget.localRotation = savedRotation;
			}
		}
		if (pasteScale){
			if (useWorldSpace){
				// can't actually set lossyScale, so instead..
				// unparent the transform, set the scale, then re-parent
				Transform targetsParent = toTarget.parent;
				toTarget.SetParent(null);
				toTarget.localScale = savedScale;
				toTarget.SetParent(targetsParent);
			}
			else{
				toTarget.localScale = savedScale;
			}
		}

		// tell us what we did
		if (showLog){
			Debug.Log("<b>Advanced Copy:</b> Pasted " + copyOptions + " to <b>" + toTarget.name + "</b>" + WorldSpaceBoolToText(useWorldSpace));
		}
	}
}

// enum for copy attributes 
public enum CopyOptions {
	Transform,
	Position,
	Rotation,
	Scale
}