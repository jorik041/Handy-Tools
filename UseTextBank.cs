using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Handy {


	/// Note: there is Textmesh Pro support - it's just commented out!

	public class UseTextBank : MonoBehaviour {

		// [HideInInspector]
		// public TextMeshProUGUI textMeshUGUI;
		// [HideInInspector]
		// public TextMeshPro textMesh;
		[HideInInspector]
		public Text text;
		public TextBank textBank;
		[HideInInspector]
		public int selectedBank;
		public bool specialFixActive = true;

		[HideInInspector]
		public bool overrideText = false;

		private void Awake() {
			RefreshText();
		}

		public bool HasTextConnected() {
			// return !(textMeshUGUI == null && textMesh == null && text == null);
			return text != null;
		}

		public void SetText(string message) {	
			if (overrideText){
				return;
			}		
			if (text) {
				text.text = message;
				#if UNITY_EDITOR
				EditorUtility.SetDirty(text);
				#endif
			}
			// else if (textMeshUGUI) {
			// 	if (specialFixActive){
			// 		if (textMeshUGUI.font.name.Contains("Travel")){
			// 			message = message.ToLower();
			// 		}
			// 	}
			// 	textMeshUGUI.SetText(message);
			// 	#if UNITY_EDITOR
			// 	EditorUtility.SetDirty(textMeshUGUI);
			// 	#endif
			// }
			// else if (textMesh) {
			// 	if (specialFixActive){
			// 		if (textMesh.font.name.Contains("Travel")){
			// 			message = message.ToLower();
			// 		}
			// 	}
			// 	textMesh.SetText(message);
			// 	#if UNITY_EDITOR
			// 	EditorUtility.SetDirty(textMesh);
			// 	#endif
			// }
		}

		public string GetText(){
			return textBank.bankSets [0].textItems [selectedBank];
		}

		public void RefreshText() {
			if (overrideText){
				return;
			}
			SetText(textBank.bankSets [0].textItems [selectedBank]);
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(UseTextBank))]
	public class UseTextBankInspector : Editor {

		UseTextBank useTextBank;
		GameObject useText;

		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			#region ConnectTextComponent
			if (useTextBank == null) {
				useTextBank = (UseTextBank)target;
			}
			if (useText == null) {
				useText = (GameObject)Selection.activeGameObject;
			}
			if (useText == null || useTextBank == null) {
				return;
			}
			if (useTextBank.HasTextConnected() == false) {
				Text uText = useText.GetComponent<Text>();
				if (uText) {
					useTextBank.text = uText;
					return;
				}
				// TextMeshProUGUI tmproUgu = useText.GetComponent<TextMeshProUGUI>();
				// if (tmproUgu) {
				// 	useTextBank.textMeshUGUI = tmproUgu;
				// 	return;
				// }
				// TextMeshPro tmpro = useText.GetComponent<TextMeshPro>();
				// if (tmpro) {
				// 	useTextBank.textMesh = tmpro;
				// 	return;
				// }
			}

			#endregion

			if (useTextBank.textBank == null) {
				return;
			}

			if (bankValueTemp == -1){
				bankValueTemp = useTextBank.selectedBank;
			}

			if (useTextBank.textBank.bankSets.Count == 0){
				GUILayout.Label("This text bank has zero entries. Please re-save the Excel sheet & confirm it has more than one row.");
			}
			else{
				EditorGUI.BeginChangeCheck();
				bankValueTemp = EditorGUILayout.Popup(bankValueTemp, useTextBank.textBank.bankSets [0].textItems.ToArray());
				if (bankValueTemp != previousValue || EditorGUI.EndChangeCheck()){
					Undo.RecordObject(useTextBank, "Select TextBank");
					useTextBank.selectedBank = bankValueTemp;
					previousValue = useTextBank.selectedBank;
					useTextBank.RefreshText();
					EditorUtility.SetDirty(useTextBank);
					EditorUtility.SetDirty(useTextBank.gameObject);
				}
			}
		}

		private int bankValueTemp = -1;
		private int previousValue = -1;
	}
#endif
}