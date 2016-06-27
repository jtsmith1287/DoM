using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

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
}
