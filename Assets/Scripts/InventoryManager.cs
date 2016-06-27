using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

	public static InventoryManager Instance;

	public Dictionary<Resource, int> Resources = new Dictionary<Resource, int>();
	public Dictionary<string, int> Items = new Dictionary<string, int>();

	private void Awake() {

		SetInstance();
	}

	private void SetInstance() {
		
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	internal void AddItem(string name) {
		
		if (Items.ContainsKey(name)) {
			Items[name]++;
		} else {
			Items.Add(name, 1);
		}
	}
}
