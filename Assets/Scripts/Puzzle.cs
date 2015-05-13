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
	public Text puzzleText;
	private int currLevel = 1;
	private bool levelCompleted = false; 
	public Material redSpaceSky;

	public void Awake() {
		codeStr = input.text;
		lastCodeStr = input.text;
		engine = IronPython.Hosting.Python.CreateEngine(); // setup python engine
		scope = engine.CreateScope();
		setupPythonEngine();
		source = engine.CreateScriptSourceFromFile("Assets/PythonPuzzles/Puzzle1.py"); 
		source.Execute(scope);
		input.text = "firstCube.setColor(red)";
	}

	GameObject getObjectForVar(string varName) {
		object obj = scope.GetVariable(varName);
		object method = PythonInterpreter.engine.Operations.GetMember(obj, "getObject");  
		GameObject instanceObj = (GameObject) PythonInterpreter.engine.Operations.Invoke(method);
		return instanceObj;
	}

	void resetScene() {
		input.text = "";
		engine = IronPython.Hosting.Python.CreateEngine(); // setup python engine
		scope = engine.CreateScope();
		setupPythonEngine();
		source = engine.CreateScriptSourceFromFile("Assets/PythonPuzzles/Puzzle" + currLevel + ".py"); 
		source.Execute(scope);

	}

	bool checkPuzzleComplete() {
		bool solved = false;
		if (currLevel == 1) {
			solved = checkLevel1Cond();
		}
		else if (currLevel == 2) {
			solved = checkLevel2Cond();
		}

		return solved;
	}

	bool checkLevel1Cond() {
		// Red cube has color green
		GameObject cube = getObjectForVar("firstCube");
		if (cube.GetComponent<Renderer>().material.color == Color.green) {
			return true;
		}
		return false;
	}

	bool checkLevel2Cond() {
		// check if the user changed the skybox

		if (RenderSettings.skybox == redSpaceSky) {
			return true;
		}
		return false;
	}

	void displayLevelCompleted() {
		if (currLevel == 1) {
			puzzleText.text = "Great, you will get the hang of this in no time! \n\nPress F3 to move on.";
		}
		else if (currLevel == 2) {
			puzzleText.text = "Ooh, space! Try some of the other options for the sky and Press F3 to move on.";
		}

	}

	void setupLevel2() {
		resetScene();
		puzzleText.text = "The world is yours to control! I'd love to see a red planet and you have access to the sky above. " +
			"\nTry using one of the methods that comes up for the Sky when you hit F1.";
		input.text = "sky.setCloudy()";
	}

	void nextLevel() {
		currLevel += 1;
		levelCompleted = false;
		clearCreatedObjects();

		if (currLevel == 2) {
			setupLevel2();
		}

	}

	void Update () {

		if (levelCompleted && Input.GetKeyDown(KeyCode.F3)) {
			nextLevel();
		}

		codeStr = input.text;
		UpdateObjects();

		
		if (codeStr != lastCodeStr) {
			clearCreatedObjects();

			source = engine.CreateScriptSourceFromFile("Assets/PythonPuzzles/Puzzle" + currLevel + ".py"); 
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

			if (checkPuzzleComplete()) {
				levelCompleted = true;
				displayLevelCompleted();
			}
		}
		lastCodeStr = codeStr;
	}
}
