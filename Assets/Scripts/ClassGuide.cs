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
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using System.Reflection;

// Generates a menu filled with class information - methods and constructors and what parameters they require 
public class ClassGuide : MonoBehaviour {
	public GameObject playerController;
	private ArrayList classInfo;
	private ObjectOperations operations;
	private IEnumerable objects;
	private List<ClassInfo> allClassSignatures;
	public GameObject classListings;
	public Text classListingText;
	
	void Start () {
		objects = PythonInterpreter.scope.GetItems(); // get items currently in the python environment from the engine
		operations = PythonInterpreter.engine.Operations;
		allClassSignatures = generateClassInfo();
		displayClassInformation();
	}

	List<ClassInfo> generateClassInfo() {
		List<ClassInfo> classInformation = new List<ClassInfo>();
		char[] removeEndChars =  new char[2] {',', ' '};

		foreach(KeyValuePair<string, object> obj in objects) {
			if (obj.Key != "__doc__" && obj.Key != "PythonUnityPrimitive") {
				if (obj.Value.GetType() == typeof(IronPython.Runtime.Types.OldClass)) { // if the object is a class
					ClassInfo classInfo = new ClassInfo();
					classInfo.name = obj.Key;
					bool constructor = false;
					object classObj = obj.Value;

					foreach (string op in operations.GetMemberNames(classObj)) { // for each of its members
						string memberSignature = "";
						object member = operations.GetMember(classObj, op);

						if (operations.IsCallable(member) && op != "update") { // if it's a function (a method) and not update which is in all classes
							if (op == "__init__") { // constructor found
								memberSignature += classInfo.name + "(";
								constructor = true;
							}

							else {
								memberSignature += op + "(";
								constructor = false;
							}

							object method = operations.GetMember(member, "__func__");
							Type methodType = method.GetType();
							PropertyInfo property = methodType.GetProperty("ArgNames",  BindingFlags.Instance | BindingFlags.NonPublic);
							var arguments = property.GetValue(method, null) as string[]; // all parameters to the method

							foreach (var arg in arguments) { // parameters, add to the string for the method
								if (arg != "self")
									memberSignature += arg + ", ";
							}
							memberSignature =  memberSignature.TrimEnd(removeEndChars);
							memberSignature += ")";
							if (constructor) {
								classInfo.constructorSignature = memberSignature;
							}
							else {
								classInfo.methodSigs.Add(memberSignature);
							}
						}
					}
					classInformation.Add(classInfo);
				}
			}
		}
		return classInformation;

	}

	void displayClassInformation() {
		string overallInfo = "Available Classes and Methods\n";

		foreach (ClassInfo info in allClassSignatures) {
			string classSig = "";
			classSig += info.constructorSignature + "\n";
			classSig += "Methods: " + "\n";
			foreach (string sig in info.methodSigs) {
				classSig += "\t" + sig + "\n";
			}
			overallInfo += classSig + "\n";
		}
		classListingText.text = overallInfo;

	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.F1) && !classListings.activeSelf) { // display the menu
			classListings.gameObject.SetActive(true);
		}
		else if (Input.GetKeyDown(KeyCode.F1) && classListings.activeSelf) {
			classListings.gameObject.SetActive(false);
		}

	}
}
