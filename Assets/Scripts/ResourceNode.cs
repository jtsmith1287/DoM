using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(NavMeshObstacle))]
public class ResourceNode : MonoBehaviour {

	public Resource ResourceType;
	public int Quantity;
	public ParticleSystem HitEffect;
	[Tooltip("In seconds")]
	public float RespawnTime;

	private Vector3 _harvestPosition;

	public string GetNameString() {

		return Enum.GetName(typeof(Resource), ResourceType);
	}

	public KeyValuePair<Resource, int> Harvest() {

		if (HitEffect != null) {
			HitEffect.Play(); 
		}
		StartCoroutine(Disable());
		return new KeyValuePair<Resource, int>(ResourceType, Quantity);
	}

	private IEnumerator Disable() {

		if (HitEffect != null) {
			yield return new WaitForSeconds(HitEffect.duration); 
		}
		transform.position = transform.position - new Vector3(0, GetComponent<Renderer>().bounds.size.y, 0);
		yield return new WaitForSeconds(RespawnTime);
		while (transform.position != _harvestPosition) {
			transform.position = Vector3.MoveTowards(transform.position, _harvestPosition, Time.deltaTime * 2);
			yield return null;
		}
	}

	private void Awake() {

		_harvestPosition = transform.position;
	}

	private void OnEnable() {

		Quantity = UnityEngine.Random.Range(1, 5);
	}
}
