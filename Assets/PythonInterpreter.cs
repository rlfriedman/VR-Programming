using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IronPython;
using IronPython.Modules;
using Microsoft.Scripting.Hosting;



public class PythonInterpreter : MonoBehaviour {
	public Text codeText = null;
	private string codeStr;
	private string lastCodeStr;
	private ScriptEngine engine;
	private ScriptScope scope;
	private ScriptSource source;
	private bool alreadyExecuted = true;
	private int codeLineLen = 0;
	private int oldLineLen = 0;

	void Start () {
		codeStr = codeText.text;
		lastCodeStr = codeText.text;

		engine = IronPython.Hosting.Python.CreateEngine ();
		scope = engine.CreateScope ();

		engine.Runtime.LoadAssembly (typeof(GameObject).Assembly);
		string init = "import UnityEngine as unity";
		source = engine.CreateScriptSourceFromString (init);
		source.Execute (scope);
	}
	
	void Update () {
		codeStr = codeText.text;
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
