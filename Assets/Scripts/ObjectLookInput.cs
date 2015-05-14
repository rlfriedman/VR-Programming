using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using IronPython;
using IronPython.Modules;
using Microsoft.Scripting.Hosting;

// look at an object in VR to see more information about it
public class ObjectLookInput : MonoBehaviour {
	public GameObject centerCamera;
	public Text label;
	public GameObject playerController;
	public bool labelsOn = true;

	private IEnumerable objects;
	private ObjectOperations operations;
	private object currLookingAt;
	private object lastLookingAt;

	public GameObject attributeScroll;
	public Text attributeScrollText;
	
	void Start () {
		currLookingAt = null;
		lastLookingAt = null;
		objects = PythonInterpreter.scope.GetItems(); // get items currently in the python environment from the engine
		operations = PythonInterpreter.engine.Operations;
	}

	// get an object's instance variable names
	ArrayList getInstanceVars(object instance) {  
		ArrayList instanceVars = new ArrayList();
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
					if (method != null) { // if it has a getObject method it is something that can be looked at
						GameObject instanceObj = (GameObject) PythonInterpreter.engine.Operations.Invoke(method);
						if (instanceObj == selectedObj) {
							return instance;
						}
					}
				}
			}
		}
		return null;
	}

	// displays instance variables on screen near selected object
	void displayInstanceVars(ArrayList instanceVars, object instance, string className) { 
		string display = "Instance of class: " + className + "\nInstance Variables and Values:\n";

		for (int i = 0; i < instanceVars.Count; i++) {
			string name = (string)instanceVars[i];
			string value = operations.GetMember(instance, name).ToString();
			display += instanceVars[i] + " = " + value + "\n";
		}

		attributeScrollText.text = display;
	}

	void displayClassCode(string code) {
		attributeScrollText.text = code;
	}

	string getClassCode(string className) {
		StreamReader source = new StreamReader("Assets/PythonClasses/" + className + ".py");
		string contents = source.ReadToEnd();
		source.Close();
		return contents;
	}

	void Update() {

		objects = PythonInterpreter.scope.GetItems(); // get current objects
		if (!labelsOn) { // no labels, non-learning settings so don't do anything
			return;
		}

		RaycastHit hit;
		if (Physics.Raycast (centerCamera.transform.position, centerCamera.transform.forward, out hit)) { // if user looking at an object
			if (hit.transform.tag != "Scene" && hit.transform.tag != "Player") {
				object instance;
				string className = "";
				if (hit.transform.parent != null) { // if object a part of a larger one, display its name
						instance = getInstance(hit.transform.parent.gameObject);
						className = hit.transform.parent.name;
				} else {
						instance = getInstance(hit.transform.gameObject);
						className = hit.transform.name;
				}

				getClassCode(className);
				currLookingAt = instance;
				label.text = getVarName(instance);  // set label to var name

				ArrayList instanceVars = getInstanceVars(instance); // all instance variable names


				if (Input.GetKeyDown(KeyCode.F2) && !attributeScroll.activeSelf) {
					attributeScroll.gameObject.SetActive(true);
					attributeScroll.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 3, hit.transform.position.z + 2);
				}
				else if (Input.GetKeyDown(KeyCode.F2)) {
					attributeScroll.gameObject.SetActive(false);
				}

				if (currLookingAt != lastLookingAt) {  // allow user to edit text
					//displayClassCode(getClassCode(className));  // display the code for the class you are looking at
					displayInstanceVars(instanceVars, instance, className);  // display the instance variables for that object
				}

				label.color = new Color(1, 1, 1, 1);
				float labelScaleX;
				if (playerController.transform.position.z > hit.transform.position.z) {  // orient label based on pos in world
						labelScaleX = -.05f;
				} else {
						labelScaleX = .05f;
				}
				label.transform.localScale = new Vector3(labelScaleX, .05f, 1f);
				label.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 1, hit.transform.position.z);
		
				lastLookingAt = instance;
			}
		} 
		else {
			currLookingAt = null;
			lastLookingAt = null;
			label.text = "";

			if (Input.GetKeyDown(KeyCode.F2) && attributeScroll.activeSelf)
				attributeScroll.gameObject.SetActive(false);
		}
	}
}

