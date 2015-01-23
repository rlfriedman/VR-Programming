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
	private ScriptEngine engine;
	private ScriptScope scope;
	private ScriptSource source;

	private Dictionary<string, GameObject> createdObjects;

	void Start () {
		codeStr = input.text;
		lastCodeStr = input.text;

		engine = IronPython.Hosting.Python.CreateEngine ();
		scope = engine.CreateScope ();

		engine.Runtime.LoadAssembly (typeof(GameObject).Assembly);
		string init = "import UnityEngine as unity";
		source = engine.CreateScriptSourceFromString (init);
		source.Execute (scope);
		source = engine.CreateScriptSourceFromFile ("Assets/PythonScripts/cubeCreate.py");
		source.Execute(scope);

	}

	string[] getCodeLines() {
		codeStr = input.text;
		return codeStr.Split('\n');
	}

	void destroyGameObjects() {
		GameObject[] objects = FindObjectsOfType<GameObject> ();

		for (int i = 0; i < objects.Length; i++) {
			string tag = objects[i].tag;
			if (tag != "Scene" && tag != "Player") {
				Destroy(objects[i]);
			}
		}
	}

	void clearCreatedObjects() {
		IEnumerable objects = scope.GetItems();

		// issue with import creating multiple import names...
		foreach (KeyValuePair<string, object> obj in objects) {
			//print (obj.Key);
			if (obj.Key != "__doc__")  {
				if (obj.Value.GetType() == typeof(GameObject)) {
					GameObject gameObj = (GameObject)obj.Value;
					Destroy (gameObj);
					scope.RemoveVariable(obj.Key);
				}
			}
		}
		destroyGameObjects ();
	}

	void Update() {
		codeStr = input.text;
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
