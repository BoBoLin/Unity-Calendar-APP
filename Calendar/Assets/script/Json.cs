using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class Json : MonoBehaviour {
	private string jsonString, MyWrite;
	private JsonData jsonData;
	public int subject_num = 1;

	// Use this for initialization
	void Start () {
		jsonString = File.ReadAllText (Application.dataPath + "/script/test.json");
		jsonData = JsonMapper.ToObject (jsonString);
		Debug.Log (jsonData ["name"]);
		Debug.Log (jsonData ["age"]);

		Debug.Log (jsonData ["item_bag"] [0] ["name"] + ":" + jsonData ["item_bag"] [0] ["price"]);
		Debug.Log (jsonData ["item_bag"] [1] ["name"] + ":" + jsonData ["item_bag"] [1] ["price"]);


		MyWrite = File.ReadAllText (Application.dataPath + "/script/WriteJson.json");
		jsonData = JsonMapper.ToObject (MyWrite);
		Debug.Log (jsonData ["day_num"]);
		Debug.Log (jsonData ["event_num"]);
		Debug.Log(jsonData["subject"+subject_num.ToString()][0]["name"]);

	
	}


}
