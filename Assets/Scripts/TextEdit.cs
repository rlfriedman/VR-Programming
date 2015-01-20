using UnityEngine;
using System.Collections;

public class TextEdit : MonoBehaviour {
	public GUIText codeText;
	private int charsOnLine = 0;
	private int lastCharsOnLine = 0;
	private bool addedCube = false;
	// Update is called once per frame
	void Update () {

		foreach (char c in Input.inputString) {
			if (c == "\b"[0]) {
				if (codeText.text.Length != 0) {
					if (codeText.text[codeText.text.Length - 1] == "\n"[0]) {
						print ("newline deleted");
						print(lastCharsOnLine);
						charsOnLine = lastCharsOnLine;
					}
					charsOnLine -= 1;
					codeText.text = codeText.text.Substring(0, codeText.text.Length - 1);
				}
			}
			else if(c == "\n"[0] || c == "\r"[0]) {
				codeText.text += "\n";
				lastCharsOnLine = charsOnLine;
				charsOnLine = 0;
			}

			else {
				codeText.text += c;
				charsOnLine += 1;
				print (c);
			}
		}

		if (Input.GetKeyDown("tab")) {
			charsOnLine += 4;
			codeText.text += "    ";
		}

		if (charsOnLine >= 30) {
			codeText.text += "\n";
			lastCharsOnLine = charsOnLine;
			charsOnLine = 0;
		}
		if (codeText.text.Contains(" = Cube()") && !(addedCube)) {
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.position = new Vector3(0, 0, 0);
			addedCube = true;
		}
	}


}
