using UnityEngine;
using UnityEngine.UI;
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
	private bool alreadyExecuted = true;
	private int codeLineLen = 0;
	private int oldLineLen = 0;

	private ArrayList oldLines;
	private ArrayList newLines;

	private Dictionary<string, GameObject> createdObjects;

	void Start () {
		codeStr = input.text;
		lastCodeStr = input.text;

		oldLines = new ArrayList ();
		newLines = new ArrayList ();

		engine = IronPython.Hosting.Python.CreateEngine ();
		scope = engine.CreateScope ();

		engine.Runtime.LoadAssembly (typeof(GameObject).Assembly);
	//	string init = "import UnityEngine as unity";
	//	source = engine.CreateScriptSourceFromString (init);
		//source.Execute (scope);
	}

	string[] getCodeLines() {
		codeStr = input.text;
		return codeStr.Split('\n');
	}

	string updateLines() {
		string linesToRun = "";
		string[] currLines = getCodeLines();

		newLines = new ArrayList ();
		for (int i = 0; i < currLines.Length; i++) {
			newLines.Add(currLines[i]);
		}

		int oldLineNum = oldLines.ToArray ().Length;

		for (int i = 0; i < oldLines.ToArray().Length; i++) {
			if (oldLines[i] != currLines[i]) {
				linesToRun += currLines[i] + "\n";
			}
		}

		if (currLines.Length > oldLineNum) {
			int newLineNums = currLines.Length - oldLineNum;
			for (int i = newLineNums - 1; i < currLines.Length; i++) {
				linesToRun += currLines[i] + "\n";
			}

		}

		return linesToRun;
	}

	void BrokenUpdate() {
		string linesToRun = updateLines();
		source = engine.CreateScriptSourceFromString (linesToRun);
		source.Execute (scope);
		oldLines = newLines;
	}

	void clearCreatedObjects() {
		IEnumerable objects = scope.GetItems();

	//	print (scope.GetVariable<string>("m"));
	//	foreach (string name in scope.GetVariableNames()) {
	//		print (name);
	//	}
		// issue with import creating multiple import names...
		foreach (KeyValuePair<string, object> obj in objects) {
			print (obj.Key);
			if (obj.Key != "__doc__")  {
				if (obj.Value.GetType() == typeof(GameObject)) {
					GameObject gameObj = (GameObject)obj.Value;
					Destroy (gameObj);
					scope.RemoveVariable(obj.Key);
				}
			}
		}
	}

	void Update() {
		codeStr = input.text;
	//	print (codeText.guiText.text);
		if (codeStr != lastCodeStr) {
			source = engine.CreateScriptSourceFromString(codeStr);
			clearCreatedObjects();

			source.Execute(scope);
		}
		lastCodeStr = codeStr;
	}

	void OlderUpdate () {
		codeStr = input.text;
		string[] codeBody = codeStr.Split ('\n');
		codeLineLen = codeBody.Length;
	
		if (codeLineLen != oldLineLen) {
			print ("new code added");
			print (codeStr.Length);
			source = engine.CreateScriptSourceFromString(codeStr);

			source.Execute(scope);
			alreadyExecuted = true;
			//string came_from_script = scope.GetVariable<string>("m");  
			// Should be what we put into 'output' in the script.  
			//Debug.Log(came_from_script);       
		}
		lastCodeStr = codeStr;
		oldLineLen = codeLineLen;
	}
}
