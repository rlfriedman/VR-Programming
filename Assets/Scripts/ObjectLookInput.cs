using UnityEngine;
using System.Collections;

public class ObjectLookInput : MonoBehaviour {
	public Camera centerCamera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;

		if (Physics.Raycast (centerCamera.transform.position, Vector3.forward, out hit)) {
			print(hit.transform.name);
		}

	}
}
