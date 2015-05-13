using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using IronPython;
using IronPython.Modules;
using Microsoft.Scripting.Hosting;

// version of the overall python code update system that accounts for pre-loaded environment objects to setup for a puzzle
public class Puzzle : PythonInterpreter {

	public void Awake() {
		codeStr = input.text;
		lastCodeStr = input.text;
		engine = IronPython.Hosting.Python.CreateEngine(); // setup python engine
		scope = engine.CreateScope();
		setupPythonEngine();
		source = engine.CreateScriptSourceFromFile("Assets/PythonScripts/Puzzle1.py"); 
		source.Execute(scope);
	}
	// Update is called once per frame
	void Update () {
		codeStr = input.text;
		UpdateObjects();
		
		if (codeStr != lastCodeStr) {
			clearCreatedObjects();
			source = engine.CreateScriptSourceFromFile("Assets/PythonScripts/Puzzle1.py"); 
			source.Execute(scope);
			source = engine.CreateScriptSourceFromString(codeStr);
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
	//	/if () {
		//	source = PythonInterpreter.engine.CreateScriptSourceFromFile("Assets/PythonScripts/Puzzle1.py"); 
		//	source.Execute(PythonInterpreter.scope);
		//}
	}
}
