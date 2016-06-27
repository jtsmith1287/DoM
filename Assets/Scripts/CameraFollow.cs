using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	public Transform Target;

	void Start () {
		
		if (Target == null) {
			Target = GameObject.FindGameObjectWithTag("Player").transform;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Target != null && transform.position != Target.position) {
			transform.position = Target.position;
		}
	}
}
