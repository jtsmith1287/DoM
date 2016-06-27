using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerAvatarMovement : MonoBehaviour {

	public float RunSpeed;
	public float RotationSpeed;
	public bool Moving = false;
	public NavMeshAgent NavAgent;
	public Transform sphere;
	public LayerMask WalkableTerrain;

	private Animator _animator;
	private int _movingHash;

	private void Awake() {

		NavAgent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		SetAnimationHashes();
	}

	private void SetAnimationHashes() {

		_movingHash = Animator.StringToHash("moving");
	}

	public void SetDestination(Vector3 position) {

		NavAgent.SetDestination(position);
	}

	public void SetDestination() {

		RaycastHit hitInfo;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo)) {
			if (hitInfo.transform.CompareTag("TerrainElement")) {
				sphere.position = hitInfo.point;
				NavAgent.SetDestination(hitInfo.point); 
			}
		}
	}

	private void Update () {

		if (NavAgent.velocity.magnitude > 0f) {
			_animator.SetBool(_movingHash, true);
		} else if (_animator.GetBool(_movingHash)) {
			_animator.SetBool(_movingHash, false);
		}
	}
}
