// Rachel Friedman 
// May 2015

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
public class PuzzleEnvironment : PythonInterpreter {
	public Text puzzleText;
	public Text puzzleTextRight;
	private int currLevel = 1;
	private bool levelCompleted = false; 
	public Material redSpaceSky;
	public Material defaultSky;
	private ObjectOperations operations;

	public void Awake() {
		codeStr = input.text;
		lastCodeStr = input.text;
		engine = IronPython.Hosting.Python.CreateEngine(); // setup python engine
		scope = engine.CreateScope();
		setupPythonEngine();
		source = engine.CreateScriptSourceFromFile("Assets/PythonPuzzles/Puzzle1.py"); 
		source.Execute(scope);
		input.text = "firstCube.setColor(red)";
		operations = engine.Operations;

	}

	GameObject getGameObjectForVar(string varName) {  // gets the GameObject represented by some variable
		object obj;
		scope.TryGetVariable<object>(varName, out obj);

		if (obj == null) { // no variable varName
			return null;
		}

		if (obj.GetType() != typeof(IronPython.Runtime.Types.OldInstance)) { // variable not storing an instance of a class
			return null;
		}

		object method;
		PythonInterpreter.engine.Operations.TryGetMember(obj, "getObject", out method);  

		if (method == null) { // no getObject method
			return null;
		}
		GameObject instanceObj = (GameObject) PythonInterpreter.engine.Operations.Invoke(method); // call the getObject method and return the obj
		return instanceObj;
	}

	void resetWeather() { // function to resetWeather on level change
		GameObject rain = GameObject.Find("Rain(Clone)");
		GameObject snow = GameObject.Find("Snow(Clone)");
		if (rain != null) {
			rain.SetActive(false);
		}
		if (snow != null) {
			snow.SetActive(false);
		}
	}

	void resetSkybox() {
		RenderSettings.skybox = defaultSky;
	}
	
	void resetScene() {
		input.text = "";
		puzzleTextRight.text = "";
		puzzleText.text = "";
		resetWeather(); //  makes sure weather doesn't persist after level reset
		resetSkybox(); //   makes sure skybox goes back to default
		source = engine.CreateScriptSourceFromFile("Assets/PythonPuzzles/Puzzle" + currLevel + ".py"); // pre-load Python code for the level
		source.Execute(scope);
	}

	bool checkPuzzleComplete() { // check to see if current puzzle is completed
		bool solved = false;
		if (currLevel == 1) {
			solved = checkLevel1Cond();
		}
		else if (currLevel == 2) {
			solved = checkLevel2Cond();
		}
		else if (currLevel == 3) {
			solved = checkLevel3Cond();
		}
		else if (currLevel == 4) {
			solved = checkLevel4Cond();
		}
		else if (currLevel == 5) {
			solved = checkLevel5Cond();
		}
		return solved;
	}

	bool checkLevel1Cond() {
		// Red cube has color green
		GameObject cube = getGameObjectForVar("firstCube");
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

	bool checkLevel3Cond() {
		// check if user made the cube spin
		object cube = scope.GetVariable("c2");

		string spinning = operations.GetMember(cube, "spinning").ToString();
		if (spinning == "True") {
			return true;
		}
		return false;
	}

	bool checkLevel4Cond() {
		// check if the user created a cube named c1
		GameObject cube = getGameObjectForVar("c1");

		if (cube != null) {
			if (cube.name == "Cube") {
				return true;
			}
		}
		return false;
	}

	bool checkLevel5Cond() {
		// check if the user changed the weather and created a snowman
		GameObject snowman = getGameObjectForVar("snow");

		if (snowman != null) {
			if (snowman.name == "Snowman" && GameObject.Find("Snow(Clone)") != null) {
				return true;
			}
		}
		return false;
	}

	void displayLevelCompleted() { // set puzzle text to proper response for level completion
		if (currLevel == 1) {
			puzzleText.text = "Great, you will get the hang of this in no time! \n\nPress F3 to move on.";
		}
		else if (currLevel == 2) {
			puzzleText.text = "Ooh, space! Try some of the other options for the sky and Press F3 to move on.";
		}
		else if (currLevel == 3) {
			puzzleTextRight.text = "";
			puzzleText.text = "Look at that cube go! Any object can be interacted with by using their methods in a similar way." +
				"as you have now seen from changing the sky and the cubes.\n\n As always, F3 to move along.";
		}
		else if (currLevel == 4) {
			puzzleTextRight.text = "";
			puzzleText.text = "Awesome! Now we can both create and interact with objects. " +
				"Objects you create are instances of the class you create them from.";
		}
		else if (currLevel == 5) {
			puzzleTextRight.text = "";
			puzzleText.text = "It's getting a little chilly in here! Good work combining your skills! That's all for now!";
		}
	}

	void setupLevel2() {
		puzzleText.text = "The world is yours to control! Why don't we travel to a red planet? You have access to the sky! " +
			"\n\nTry using one of the methods that comes up for the Sky when you hit F1.";
		input.text = "sky.setCloudy()";
	}

	void setupLevel3() {
		puzzleText.text = "You are doing great so far! Now, remember how you interacted with the world in the past few tasks?" +
			"Time to do that again on your own.";
		puzzleTextRight.text = "It would be great if we could make the blue cube spin. " +
			"Hit F1 to find something that might help. Remember you can look at any object to see its name.";
	}

	void setupLevel4() {
		input.text = "s = Sphere(0, 3, -70, yellow)";
		puzzleText.text = "Now you know how to interact with objects. But how do you create them?";
		puzzleTextRight.text = "The sphere in front of you exists as a result of the code you see." +
			"Can you create a cube named c1 in the same way? Cubes require an x, y, z and a color.";
	}

	void setupLevel5() {
		input.text = "weather = Weather()\nweather.setWeather(\"rain\")";
		puzzleText.text = "Looks like it's a little rainy! I'd prefer snow. Can you help with that?";
		puzzleTextRight.text = "The Weather class has a method setWeather which requires a word in quotes representing the weather." +
			"After you change the weather, could you make a snowman for me too? Call it snow.";
	}

	void nextLevel() { // setup the next level
		currLevel += 1;
		levelCompleted = false;
		clearCreatedObjects();
		resetScene();

		if (currLevel == 2) {
			setupLevel2();
		}
		else if (currLevel == 3) {
			setupLevel3();
		}
		else if (currLevel == 4) {
			setupLevel4();
		}
		else if (currLevel == 5) {
			setupLevel5();
		}
	}

	void Update () {
		if (levelCompleted && Input.GetKeyDown(KeyCode.F3) && !(currLevel == 5)) { // if level completed, go to next on until hit max level
			nextLevel();
		}

		codeStr = input.text;
		UpdateObjects();

		if (codeStr != lastCodeStr) {
			clearCreatedObjects();

			source = engine.CreateScriptSourceFromFile("Assets/PythonPuzzles/Puzzle" + currLevel + ".py"); // assumes each level has a Puzzle#.py file
			source.Execute(scope);
			source = engine.CreateScriptSourceFromString(codeStr);

			try {
				source.Execute(scope);
				errors.text = "";
				lastWorkingCode = codeStr;
			}
			catch(Exception e) { // display error message
				print (e.Message);
				errors.text = "Error: " + e.Message;
				if (lastWorkingCode != null) { // run the last working code instead of current code
					source = engine.CreateScriptSourceFromString(lastWorkingCode);
					source.Execute(scope);
				}
			}

			if (checkPuzzleComplete()) {
				levelCompleted = true;
				displayLevelCompleted();
			}
		}
		lastCodeStr = codeStr;
	}
}
