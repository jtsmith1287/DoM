using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System;

public class CraftingManager : MonoBehaviour {

	public static CraftingManager Instance;

	public Dictionary<string, Blueprint> Blueprints = new Dictionary<string, Blueprint>();
	public List<string> KnownBlueprints = new List<string>();

	public void CraftItem(string name) {
		
		if (CanCraftItem(name)) {
			foreach (Blueprint.MaterialCount mc in Blueprints[name].Materials) {
				InventoryManager.Instance.Resources[mc.Resource] -= mc.Quantity;
			}
			Debug.Log("Crafting " + name);
			InventoryManager.Instance.AddItem(name);
			GameObject item = (GameObject)Instantiate(Resources.Load("Prefabs/Craftables/" + name));
			item.transform.position = new Vector3(UnityEngine.Random.Range(-3f, 3f), 0, UnityEngine.Random.Range(-3f, 3f)); 
		}
	}

	public bool CanCraftItem(string name) {

		Blueprint bp = Blueprints[name];
		bool craftable = true;
		foreach (Blueprint.MaterialCount mc in bp.Materials) {
			if (InventoryManager.Instance.Resources.ContainsKey(mc.Resource)) {
				if (InventoryManager.Instance.Resources[mc.Resource] < mc.Quantity) {
					craftable = false;
					Debug.Log("Not enough resources: " + Enum.GetName(typeof(Resource), mc.Resource));
					return craftable;
				}
			} else {
				craftable = false;
				Debug.Log("Not enough resources: " + Enum.GetName(typeof(Resource), mc.Resource));
				return craftable;
			}
		}
		return craftable;
	}

	public List<string> DiscoverBlueprints(Dictionary<Resource, int> mats) {

		List<string> matchList = new List<string>();
		
		Resource bpResource;
		int bpQuantity;
		// Loop all blueprints to find a match.
		foreach (var kvPair in Blueprints) {
			// Assume the blueprint matches until we prove otherwise
			bool matching = true;
			// if the user chose less materials than this blueprint needs, we know right away it's not a match.
			if (mats.Count != kvPair.Value.Materials.Count) {
				continue;
			}
			for (int i = 0; i < kvPair.Value.Materials.Count; i++) {
				bpResource = kvPair.Value.Materials[i].Resource;
				bpQuantity = kvPair.Value.Materials[i].Quantity;
				int quantityToCompare;
				if (mats.TryGetValue(bpResource, out quantityToCompare)) {
					// Blueprint shares the same resource the user chose, now let's check quantity match
					if (quantityToCompare != bpQuantity) {
						// Same resource used, but incorrect quantity
						matching = false;
					}
				}				
			}
			if (matching) {
				matchList.Add(kvPair.Key);
			}
		}
		List<string> discovered = new List<string>();
		if (matchList.Count > 0) {
			foreach (string name in matchList) {
				if (!KnownBlueprints.Contains(name)) {
					//Debug.Log("You discovered " + name + "!");
					KnownBlueprints.Add(name);
					discovered.Add(name);
				}
			} 
		} else {
			Debug.Log("Nope... nothing uses that.");
		}

		return discovered;
	}

	private void Awake() {

		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}

		UnityEngine.Object[] objs = Resources.LoadAll("Prefabs/Craftables");
		foreach (GameObject obj in objs) {
			Blueprint bp = obj.GetComponent<Blueprint>();
			bp.Name = obj.name;
			Blueprints.Add(obj.name, bp);
		}
		Resources.UnloadUnusedAssets();
	}
}
