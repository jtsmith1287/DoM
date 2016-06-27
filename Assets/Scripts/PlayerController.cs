using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	public PlayerAvatarMovement PlayerMovement;
	public PlayerActions PlayerActions;

	private void Awake() {

		PlayerMovement = GetComponent<PlayerAvatarMovement>();
		PlayerActions = GetComponent<PlayerActions>();
	}

	private void Update() {

		ProcessInputEvents();
	}

	private void ProcessInputEvents() {

		if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			if (Input.GetMouseButtonDown(0)) {
				PlayerActions.DoAction();
			}
			if (Input.GetMouseButton(1)) {
				PlayerMovement.SetDestination();
			} 
		}
	}
}
