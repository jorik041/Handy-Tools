#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// excel import libraries
#pragma warning disable 0219 
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using System.IO;

namespace Handy {

	public class TextBankImporter : AssetPostprocessor {

		static void OnPostprocessAllAssets(string [] importedAssets, string [] deletedAssets, string [] movedAssets, string [] movedFromAssetPaths) {
			foreach (string assetPath in importedAssets) {
				if (assetPath.Contains(".xlsx") && assetPath.Contains("~$") == false) {
					if (assetPath.Contains("TextBank")) {
						CreateTextBank.NewTextBank(assetPath);
					}
				}
			}
		}
	}

	public class CreateTextBank : Editor {

		[MenuItem("Assets/Create/New TextBank")]
		public static void CreateNewScriptableObject() {
			TextBank asset = ScriptableObject.CreateInstance<TextBank>();
			AssetDatabase.CreateAsset(asset, "Assets/New TextBank.asset");
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

		public static void NewTextBank(string spreadsheetPath) {
			string assetPath = spreadsheetPath.Substring(0, spreadsheetPath.Length - ".xlsx".Length);
			assetPath += ".asset";
			TextBank newBank = null;
			if (File.Exists(assetPath)) {
				newBank = (TextBank)AssetDatabase.LoadMainAssetAtPath(assetPath);
				newBank.ResetItems();
				AssetDatabase.SaveAssets();
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = newBank;
				ImportTextBank(newBank, spreadsheetPath);
			}
			else {
				newBank = CreateInstance<TextBank>();
				AssetDatabase.CreateAsset(newBank, assetPath);
				AssetDatabase.SaveAssets();
				EditorUtility.FocusProjectWindow();
				Selection.activeObject = newBank;
				ImportTextBank(newBank, spreadsheetPath);
			}
			if (newBank) {
				EditorUtility.SetDirty(newBank);
			}
		}

		/// <summary>
		/// Fills TextBank bank with all of the sheets and left-most cells of an Excel spreadsheet
		/// </summary>
		/// <param name="bank"></param>
		/// <param name="path"></param>
		public static void ImportTextBank(TextBank bank, string path) {
			// find & open the excel file in read only mode
			FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			IWorkbook book = new XSSFWorkbook(stream);
			// import all sheets
			for (int sheetIndex = 0; sheetIndex < book.NumberOfSheets; sheetIndex++) {
				ISheet s = book.GetSheetAt(sheetIndex);
				TextBankSet newSet = new TextBankSet();
				newSet.name = s.SheetName;
				// look through each row of this sheet
				for (int rowIndex = 0; rowIndex <= s.LastRowNum; rowIndex++) {
					if (s.GetRow(rowIndex) != null) {
						if (s.GetRow(rowIndex).GetCell(0) != null) {
							newSet.textItems.Add(s.GetRow(rowIndex).GetCell(0).StringCellValue);
						}
						else {
							newSet.textItems.Add(" ");
						}
					}
				}
				bank.bankSets.Add(newSet);
			}
			stream.Close();
		}
	}
}
#endif