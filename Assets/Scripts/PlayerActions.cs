using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerActions : MonoBehaviour {

	public Transform WeaponHand;
	public GameObject Weapon;
	public LayerMask ActionableLayers;

	private Animator _animator;
	private bool isActing = false;
	private int _harvestingNodeHash;
	private int _swingingWeaponHash;
	private PlayerAvatarMovement _playerMovement;
	private Dictionary<string, Action<GameObject>> _actionTable = new Dictionary<string, Action<GameObject>>();

	public void DoAction() {

		if (!isActing) {
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ActionableLayers.value)) {
				DoActionWithEntity(hitInfo.transform.gameObject);
			}
		}
	}

	private void DoActionWithEntity(GameObject entityGO) {

		Action<GameObject> action;
		if (_actionTable.TryGetValue(LayerMask.LayerToName(entityGO.layer), out action)) {
			action(entityGO);
		} else {
			Debug.Log("Shrug...");
		}
	}

	private void Awake() {

		_animator = GetComponent<Animator>();
		_playerMovement = GetComponent<PlayerAvatarMovement>();
		_harvestingNodeHash = Animator.StringToHash("harvestingNode");
		_swingingWeaponHash = Animator.StringToHash("swingingWeapon");
		BuildActionTable();
	}

	private void BuildActionTable() {

		_actionTable.Add("ResourceNode", HarvestResource);
	}

	private void HarvestResource(GameObject resourceGO) {

		ResourceNode node = resourceGO.GetComponent<ResourceNode>();
		Debug.Log(node.GetNameString());
		StartCoroutine(MoveToResource(node));
	}

	private IEnumerator MoveToResource(ResourceNode node) {

		NavMeshObstacle obstacle = node.GetComponent<NavMeshObstacle>();
		float distance = Vector3.Distance(transform.position, node.transform.position);
		Vector3 movePos = Vector3.Lerp(transform.position, node.transform.position,
			(distance - obstacle.radius - _playerMovement.NavAgent.radius - 1.5f) / distance);
		_playerMovement.SetDestination(movePos);
		_playerMovement.sphere.position = movePos;

		while (Vector3.Distance(transform.position, movePos) > 0.5f) {
			yield return null;
		}
		KeyValuePair<Resource, int> harvest = node.Harvest();
		if (InventoryManager.Instance.Resources.ContainsKey(harvest.Key)) {
			InventoryManager.Instance.Resources[harvest.Key] += harvest.Value;
		} else {
			InventoryManager.Instance.Resources.Add(harvest.Key, harvest.Value);
		}
		Debug.Log("You have " + InventoryManager.Instance.Resources[harvest.Key] + " " + harvest.Key.ToString());
	}

	private void SwingWeapon() {

		_animator.SetTrigger(_swingingWeaponHash);
	}
}
