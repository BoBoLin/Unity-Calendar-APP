using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowNum : MonoBehaviour {
	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		text.text = "access :\n"+ Application.persistentDataPath + "/[which day .json]";
	}

}
