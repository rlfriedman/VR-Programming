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

public class ClassGuide : MonoBehaviour {
	public GameObject playerController;
	private ArrayList classInfo;
	private ObjectOperations operations;
	private IEnumerable objects;
	private List<ClassInfo> allClassSignatures;
	public InputField classListings;


	void Start () {
		objects = PythonInterpreter.scope.GetItems(); // get items currently in the python environment from the engine
		operations = PythonInterpreter.engine.Operations;
		allClassSignatures = generateClassInfo();
	//	print(allClassSignatures[0].name);
	//	print (allClassSignatures[0].constructorSignature);
		//foreach (string sig in allClassSignatures[0].methodSigs) {
	//		print(sig);
		//}
		displayClassInformation();

	}

	List<ClassInfo> generateClassInfo() {

		//object member = operations.GetMember(instance, name);
		
		//if (!operations.IsCallable(member) && !(name == "__doc__" || name == "__module__" || name == "_object")) { // found an instance variable!
		//}
		List<ClassInfo> classInformation = new List<ClassInfo>();

		foreach(KeyValuePair<string, object> obj in objects) {
			if (obj.Key != "__doc__") {
				if (obj.Value.GetType() == typeof(IronPython.Runtime.Types.OldClass)) { // if the object is a class
					ClassInfo classInfo = new ClassInfo();
					classInfo.name = obj.Key;
					bool constructor = false;
					object classObj = obj.Value;

					foreach (string op in operations.GetMemberNames(classObj)) { // for each of its members
						string memberSignature = "";
						object member = operations.GetMember(classObj, op);

						if (operations.IsCallable(member)) { // if it's a function (a method)
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

							foreach (var arg in arguments) { // parameters
								memberSignature += arg + " ";
							}
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
		string overallInfo = "";
		foreach (ClassInfo info in allClassSignatures) {
			string classSig = "";
			classSig += info.constructorSignature + "\n";
			foreach (string sig in info.methodSigs) {
				classSig += sig + "\n";
			}
			print (classSig);
			overallInfo += classSig + "\n";
			classListings.text = classSig;

		}
		print (overallInfo);
		classListings.text = overallInfo;


	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.F1) && !classListings.IsActive()) {
			classListings.gameObject.SetActive(true);
		}
		else if (Input.GetKeyDown(KeyCode.F1) && classListings.IsActive()) {
			classListings.gameObject.SetActive(false);
		}

	}
}
