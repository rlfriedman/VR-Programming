using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using IronPython;
using IronPython.Modules;
using Microsoft.Scripting.Hosting;


public class PythonInterpreter : MonoBehaviour {
	public InputField input = null;
	public Text errors;
	public static ScriptEngine engine;
	public static ScriptScope scope;
	public static ScriptSource source;

	protected string codeStr;
	protected string lastCodeStr;

	protected Dictionary<string, GameObject> createdObjects;

	public void Awake() {
		codeStr = input.text;
		lastCodeStr = input.text;
		engine = IronPython.Hosting.Python.CreateEngine(); // setup python engine
		scope = engine.CreateScope();
		setupPythonEngine();
	}

	public void setupPythonEngine() { // load in existing python classes and execute them so that they are in the scope
		engine.Runtime.LoadAssembly(typeof(GameObject).Assembly);
		source = engine.CreateScriptSourceFromFile("Assets/PythonScripts/InitialSetup.py"); 
		source.Execute(scope);
		source = engine.CreateScriptSourceFromFile("Assets/PythonScripts/PythonUnityPrimitive.py"); 
		source.Execute(scope);

		DirectoryInfo pythonDir = new DirectoryInfo("Assets/PythonClasses"); // get all .py files in the scripts dir
		FileInfo[] pythonFiles = pythonDir.GetFiles("*.py");

		foreach (FileInfo file in pythonFiles) {
			source = engine.CreateScriptSourceFromFile(file.ToString()); 
			source.Execute(scope);
		}
	}

	public string[] getCodeLines() {
		codeStr = input.text;
		return codeStr.Split('\n');
	}

	public void destroyGameObjects() {  // destroy any objects not stored in variables
		GameObject[] objects = FindObjectsOfType<GameObject>();

		for (int i = 0; i < objects.Length; i++) {
			string tag = objects[i].tag;
			if (tag != "Scene" && tag != "Player") {
				Destroy(objects[i]);
			}
		}
	}

	public void clearCreatedObjects() { // clear world for re-execution of code
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

	public void UpdateObjects() { // execute each class instance's update function, all must have an update

		IEnumerable objects = scope.GetItems();
		ArrayList updatedObjects = new ArrayList();

		foreach (KeyValuePair<string, object> obj in objects) {
			if (obj.Key != "__doc__")  {
				if (obj.Value.GetType() == typeof(IronPython.Runtime.Types.OldInstance) && !updatedObjects.Contains(obj.Value) && engine.Operations.GetMember(obj.Value, "update") != null) {
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
				errors.text = "";
			}
			catch(Exception e) { // display error message
				print (e.Message);
				errors.text = "Error: " + e.Message;
			}
		}
		lastCodeStr = codeStr;
	}
}
