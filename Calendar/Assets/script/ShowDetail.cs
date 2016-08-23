using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using LitJson;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class ShowDetail : MonoBehaviour {

	int get_day_num = home.num;
	int get_event_num = ShowEvents.event_num;
//	private Vector2 scrollPosition = Vector2.zero;

	Text text;
	private string MyWrite;
	private JsonData jsonData;
	int choose_step_num = ShowSteps.choose_step_num;

	// Use this for initialization
	void Start () {
		if (get_day_num == 0)
			SceneManager.LoadScene ("home");
		else 
		{
			MyWrite = File.ReadAllText (Application.persistentDataPath + "/day" + get_day_num + ".json");
			jsonData = JsonMapper.ToObject (MyWrite);

			text = GameObject.Find ("Title").GetComponent<Text> ();

			string[] split_step =  Regex.Split(jsonData ["subject" + get_event_num.ToString ()] [0] ["step" + choose_step_num.ToString ()].ToString (), "/f:", RegexOptions.IgnoreCase);  //將字串分為前面是步驟名稱 後面是是否完成
			text.text = split_step[0];
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
