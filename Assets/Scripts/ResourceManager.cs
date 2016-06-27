using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {

	public static ResourceManager Instance;

	private void Awake() {

		SetInstance();
		GatherNodes();
	}

	private void GatherNodes() {

		ResourceNode[] nodes = GameObject.FindObjectsOfType<ResourceNode>();
		for (int i = 0; i < nodes.Length; i++) {
			nodes[i].transform.parent.SetParent(transform);
		}
	}

	private void SetInstance() {

		if (Instance == null) {
			Instance = this;
		} else {
			Destroy(this);
		}
	}
}
