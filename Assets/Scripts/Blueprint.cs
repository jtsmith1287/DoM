using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Blueprint : MonoBehaviour {

	[Serializable]
	public class MaterialCount {
		public Resource Resource;
		public int Quantity;
	}

	public string Name;
	public List<MaterialCount> Materials = new List<MaterialCount>();
}
