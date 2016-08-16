using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;

public class WriteJson : MonoBehaviour {


	// Use this for initialization
	void Start () {
		/*Test test_one = new Test ("alan", "gogo");
		JsonData test;
		test = JsonMapper.ToJson (test_one).ToString();
*/
		string[] items = {"1", "bb", "cc" };
		Day day_one = new Day (1, "睡覺", 3, items);
		/*JsonData jsonData;
		jsonData = JsonMapper.ToJson (day_one);
*/
		//string[] items_two = { "q", "w", "e" };
		//Day day_two = new Day (2, "刷牙", 2, items_two);
		JsonWriter jsonWriter = new JsonWriter ();
		jsonWriter.PrettyPrint = true;
		jsonWriter.IndentValue = 4;
		JsonMapper.ToJson (day_one, jsonWriter);
		File.WriteAllText (Application.dataPath + "/script/WriteJson.json", jsonWriter.ToString ());
		/*
		JsonWriter jsonWriter_two = new JsonWriter ();
		jsonWriter_two.PrettyPrint = true;
		jsonWriter_two.IndentValue = 4;
		JsonMapper.ToJson (day_two, jsonWriter_two);
		File.AppendAllText (Application.dataPath + "/script/WriteJson.json", jsonWriter_two.ToString ());
*/

	}

	public class Day{
		public int day_num;
		public string subject;
		public int count;
		public string[] items;

		public Day(int day_num, string subject, int count, string[] items){
			this.day_num = day_num;
			this.subject = subject;
			this.count = count;
			this.items = new string[count];
			for(int i = 0; i< count; i++)
				this.items[i] = items[i];
		}
	}

	public class Test{
		public string ok;
		public string go;
		public Test(string ok, string go){
			this.ok = ok ;
			this.go = go ;
		}
	}
	

}
