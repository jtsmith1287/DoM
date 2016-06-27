using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class DebugCheats : MonoBehaviour {

	[MenuItem("Debug Cheats/Unlock All Blueprints")]
	private static void UnlockAllBlueprints() {

		if (CraftingManager.Instance != null) {
			CraftingManager.Instance.KnownBlueprints = new List<string>(CraftingManager.Instance.Blueprints.Keys);
			foreach (string name in CraftingManager.Instance.KnownBlueprints) {
				Debug.Log("Debug Cheat | Unlocked: " + name);
			}
		} else {
			Debug.LogWarning("Not in play mode. Cannot unlock blueprints.");
		}
	}

	[MenuItem("Debug Cheats/Increase All Resources")]
	private static void IncreaseAllResources() {

		foreach(Resource r in Enum.GetValues(typeof(Resource))) {
			if (InventoryManager.Instance.Resources.ContainsKey(r)) {
				InventoryManager.Instance.Resources[r] += 10;
			} else {
				InventoryManager.Instance.Resources.Add(r, 10);
			}
		}
	}
}
