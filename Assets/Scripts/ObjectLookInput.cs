using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using IronPython;
using IronPython.Modules;
using Microsoft.Scripting.Hosting;

// look at an object in VR to see more information about it
public class ObjectLookInput : MonoBehaviour {
	public GameObject centerCamera;
	public Text label;
	public InputField attributeField;
	public GameObject playerController;
	public bool labelsOn = true;
	IEnumerable objects;
	ObjectOperations operations;
	object currLookingAt;
	object lastLookingAt;
	
	void Start () {
		currLookingAt = null;
		lastLookingAt = null;
		objects = PythonInterpreter.scope.GetItems();
		operations = PythonInterpreter.engine.Operations;
	}

	// get an object's instance variable names
	ArrayList getInstanceVars(object instance) {  
		ArrayList instanceVars = new ArrayList ();
		foreach (var name in operations.GetMemberNames(instance)) {
			object member = operations.GetMember(instance, name);

			if (!operations.IsCallable(member) && !(name == "__doc__" || name == "__module__" || name == "_object")) { // found an instance variable!
				instanceVars.Add(name);
			}
		}
		return instanceVars;
	}

	// gets the first variable name the object is stored under
	string getVarName(object instance) { 
		foreach (KeyValuePair<string, object> obj in objects) {
			if (obj.Key != "__doc__") {
				if (obj.Value == instance) {
					return obj.Key;
				}
			}
		}
		return "";
	}

	// returns the IronPython OldInstance of the class we are currently looking at in VR
	object getInstance(GameObject selectedObj) { 
		foreach (KeyValuePair<string, object> obj in objects) {
			if (obj.Key != "__doc__")  {
				if (obj.Value.GetType() == typeof(IronPython.Runtime.Types.OldInstance)) {
					object instance = obj.Value;

					// each python class should have a getObject method which returns
					// the main game object for that class
					object method = PythonInterpreter.engine.Operations.GetMember(instance, "getObject");  
					
					GameObject instanceObj = (GameObject) PythonInterpreter.engine.Operations.Invoke(method);
					if (instanceObj == selectedObj) {
						return instance;
					}
				}
			}
		}
		return null;
	}

	// displays instance variables on screen near selected object
	void displayInstanceVars(ArrayList instanceVars, object instance) { 
		string display = "";

		for (int i = 0; i < instanceVars.Count; i++) {
			string name = (string)instanceVars[i];
			string value = operations.GetMember(instance, name).ToString();
			display += instanceVars[i] + " = " + value + "\n";
		}

		attributeField.text = display;
	}

	void Update () {

		objects = PythonInterpreter.scope.GetItems(); // get current objects
		if (!labelsOn) { // no labels, non-learning settings so don't do anything
			return;
		}

		RaycastHit hit;
		if (Physics.Raycast (centerCamera.transform.position, centerCamera.transform.forward, out hit)) { // if user looking at an object
						if (hit.transform.tag != "Scene" && hit.transform.tag != "Player") {
								attributeField.gameObject.SetActive(true);
								object instance;
								if (hit.transform.parent != null) { // if object a part of a larger one, display its name
										instance = getInstance(hit.transform.parent.gameObject);
										label.text = hit.transform.parent.name;
								} else {
										instance = getInstance(hit.transform.gameObject);
										label.text = hit.transform.name;
								}
								currLookingAt = instance;
								label.text = getVarName(instance);  // set label to var name

								ArrayList instanceVars = getInstanceVars(instance); // all instance variable names

								if (currLookingAt != lastLookingAt)  // allow user to edit text
									displayInstanceVars(instanceVars, instance);

								label.color = new Color (1, 1, 1, 1);
								float labelScaleX;
								if (playerController.transform.position.z > hit.transform.position.z) {  // orient label based on pos in world
										labelScaleX = -.05f;
								} else {
										labelScaleX = .05f;
								}
								label.transform.localScale = new Vector3(labelScaleX, .05f, 1f);
								label.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
								attributeField.transform.localScale = new Vector3(.1f, .05f, 1f);
								attributeField.transform.position = new Vector3 (hit.transform.position.x + 2, hit.transform.position.y, hit.transform.position.z);
								lastLookingAt = instance;
						}
		} 
		else {
			currLookingAt = null;
			lastLookingAt = null;
			label.text = "";
			attributeField.gameObject.SetActive(false);
		}
	}
}

