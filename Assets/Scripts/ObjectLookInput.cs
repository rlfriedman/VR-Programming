using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectLookInput : MonoBehaviour {
	public GameObject centerCamera;
	public Text label;
	public GameObject playerController;
	public bool labelsOn = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (labelsOn) {
			RaycastHit hit;
			if (Physics.Raycast (centerCamera.transform.position, centerCamera.transform.forward, out hit)) {
				print(hit.transform.name);
				if (hit.transform.tag != "Scene") {
					label.text = hit.transform.name;
					label.color = new Color(1, 1,1, 1);
					float labelScaleX;
					if (playerController.transform.position.z > hit.transform.position.z) {
						labelScaleX = -.05f;
					}
					else {
						labelScaleX = .05f;
					}
					label.transform.localScale = new Vector3(labelScaleX, .05f, 1f);
					label.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
				}
			}

			else {
				label.text = "";
			}
		}
	}
}
