using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using IronPython;
using IronPython.Modules;
using Microsoft.Scripting.Hosting;



public class PythonInterpreter : MonoBehaviour {
	public InputField input = null;
	private string codeStr;
	private string lastCodeStr;
	public static ScriptEngine engine;
	public static ScriptScope scope;
	public static ScriptSource source;

	private Dictionary<string, GameObject> createdObjects;

	void Start () {
		codeStr = input.text;
		lastCodeStr = input.text;

		engine = IronPython.Hosting.Python.CreateEngine(); // setup python engine
		scope = engine.CreateScope();

		engine.Runtime.LoadAssembly(typeof(GameObject).Assembly);
		string init = "import UnityEngine as unity";
		source = engine.CreateScriptSourceFromString (init);
		source.Execute(scope); 
		source = engine.CreateScriptSourceFromFile ("Assets/PythonScripts/cubeCreate.py"); // load any external files in here and execute
		source.Execute(scope);
		source = engine.CreateScriptSourceFromFile ("Assets/PythonScripts/Cube.py"); // load any external files in here and execute
		source.Execute(scope);

	}

	string[] getCodeLines() {
		codeStr = input.text;
		return codeStr.Split('\n');
	}

	void destroyGameObjects() {  // destroy any objects not stored in variables
		GameObject[] objects = FindObjectsOfType<GameObject> ();

		for (int i = 0; i < objects.Length; i++) {
			string tag = objects[i].tag;
			if (tag != "Scene" && tag != "Player") {
				Destroy(objects[i]);
			}
		}
	}

	void clearCreatedObjects() { // clear world for re-execution of code
		IEnumerable objects = scope.GetItems();
		// clear out environment variables
		// issue with import creating multiple import names...
		foreach (KeyValuePair<string, object> obj in objects) {
			if (obj.Key != "__doc__")  {
				if (obj.Value.GetType() == typeof(IronPython.Runtime.Types.OldInstance)) {
					scope.RemoveVariable(obj.Key);
				}
			}
		}
		destroyGameObjects();
	}

	void UpdateObjects() { // execute each class instance's update function, all must have an update

		IEnumerable objects = scope.GetItems();
		ArrayList updatedObjects = new ArrayList();

		foreach (KeyValuePair<string, object> obj in objects) {
			if (obj.Key != "__doc__")  {
				if (obj.Value.GetType() == typeof(IronPython.Runtime.Types.OldInstance) && !updatedObjects.Contains(obj.Value)) {
					string updateCall = obj.Key + ".update()\n";
					updatedObjects.Add(obj.Value); // don't update already updated objects
					source = engine.CreateScriptSourceFromString(updateCall);
					source.Execute(scope);
				}
			}
		}
	}

	void Update() {
		codeStr = input.text;
		UpdateObjects();

		if (codeStr != lastCodeStr) {
			source = engine.CreateScriptSourceFromString(codeStr);
			clearCreatedObjects();
			try {
				source.Execute(scope);
			}
			catch(Exception e) { // should eventually display on screen
				print (e.Message);
			}
		}
		lastCodeStr = codeStr;
	}
}
