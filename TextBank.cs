using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Handy {

	[System.Serializable]
	public class TextBank : ScriptableObject {
		public List<TextBankSet> bankSets = new List<TextBankSet>();
		public void ResetItems() {
			bankSets.Clear();
		}
		public TextBankSet GetBankSetByName(string name) {
			foreach (TextBankSet set in bankSets) {
				if (set.name == name) {
					return set;
				}
			}
			return null;
		}
		
		public int GetBankIndexByName(string name) {
			foreach (TextBankSet set in bankSets) {
				if (set.name == name) {
					return bankSets.IndexOf(set);
				}
			}
			return 0;
		}
		
		public string GetBankTextByIndex(int index, string set){
			TextBankSet bankSet = GetBankSetByName(set);
			if (bankSet == null){
				Debug.LogWarning("TextBank " + this.name + " does not have an entry for current set, '" + set + "'");				
				return "";
			}
			return bankSet.textItems[index];
		}
	}

	[System.Serializable]
	public class TextBankSet {
		public string name;
		public List<string> textItems = new List<string>();
	}
}