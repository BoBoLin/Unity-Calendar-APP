using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;
using System.IO;
using System;
using System.Text.RegularExpressions;
public class AddDetail : MonoBehaviour {

	public GUISkin GUISkinAddStep;
	public GUISkin GUISkinShowStep;

	int get_day_num = home.num;
	int get_event_num = ShowEvents.event_num;
	private Vector2 scrollPosition = Vector2.zero;

	Text text;
	InputField InputFieldName;
	private string MyWrite;
	private JsonData jsonData;
	int choose_step_num = ShowSteps.choose_step_num;
	public int detail_num = 0;
	public int choose = 0;
	public static string[] detailString = new string[50]; 
	public string stringToEdit ="";
	public string detail = "";
	public string total_detail = "";
	string detail_dot = "";
	public string subject;

	// Use this for initialization
	void Start () {

		detailString [0] = "";
		stringToEdit = "請輸入步驟1之內容";
		MyWrite = File.ReadAllText (Application.persistentDataPath + "/day" + get_day_num + ".json");
		jsonData = JsonMapper.ToObject (MyWrite);
		InputFieldName = GameObject.Find ("InputField").GetComponent<InputField> ();

		string[] split_step =  Regex.Split(jsonData ["subject" + get_event_num.ToString ()] [0] ["step" + choose_step_num.ToString ()].ToString (), "/f:", RegexOptions.IgnoreCase);  //將字串分為前面是步驟名稱 後面是是否完成
		InputFieldName.text = split_step[0];

		detail_num = Int32.Parse (jsonData ["subject" + get_event_num.ToString ()] [choose_step_num] ["detail_num"].ToString ()); //取得detail數量

		for (int i = 1; i <= detail_num; i++)  //從後端拿出所有detail存入detailString Array中
		{
			detailString [i] = jsonData ["subject" + get_event_num.ToString ()] [choose_step_num] ["detail" + i.ToString ()].ToString ();
		}
		stringToEdit = "請輸入步驟" + (detail_num + 1).ToString () + "之內容";
	}
		
	public void OnGUI()
	{
		GUI.skin = GUISkinAddStep;

		GUI.skin.verticalScrollbarThumb.fixedWidth = 5;
		GUI.skin.verticalScrollbar.fixedWidth = 5;
		scrollPosition = GUI.BeginScrollView(new Rect(0, Screen.height / 10 * 3, Screen.width, Screen.height / 2),
			scrollPosition,
			new Rect(0, 0, Screen.width * 5 / 6 - 15, detail_num * Screen.height / 10));
		if (choose != 0)
		{
			GUI.Label(new Rect(Screen.width * 9 / 10, Screen.height / 10 * (choose - 1), Screen.width / 12, Screen.height / 12), "");
		} // 選取記號

		if (detail_num == 0)
			detailString [1] = "請輸入步驟1之內容";
		for (int i = 0; i < detail_num; i++) 
		{
			GUI.skin = GUISkinShowStep;
			GUI.skin.label.fontSize = Screen.height / 30;
			GUI.skin.button.fontSize = Screen.height / 20;
			GUI.Label (new Rect(0, Screen.height / 10 * i, Screen.width / 10 * 2, Screen.height / 10), (i + 1).ToString () + " : ");

			//string[] split_step =  Regex.Split(stepStringArray [i + 1], "/f:", RegexOptions.IgnoreCase);  //將字串分為前面是步驟名稱 後面是是否完成

			if (GUI.Button (new Rect (Screen.width / 5 + 5, Screen.height / 10 * i, Screen.width / 10 * 7, Screen.height / 10), detailString [i + 1])) 
			{
				choose = i + 1;
			}
		}

		GUI.EndScrollView();

		GUI.skin = GUISkinAddStep;
		GUI.skin.textField.fontSize = Screen.height / 20;
		stringToEdit = GUI.TextField(new Rect(Screen.width / 10, Screen.height / 10 * 8, Screen.width / 10 * 8, Screen.height / 10), stringToEdit, 50);
		GUI.skin.button.fontSize = Screen.height / 30;
		if (GUI.Button (new Rect(Screen.width / 10 * 3, Screen.height / 10 * 9, Screen.width / 10 * 4, Screen.height / 15), "新增此步驟"))   
		{
			//from_show_step = 1;
			detail_num++; //detail數+1
			detailString [detail_num] = stringToEdit;
			stringToEdit = "請輸入步驟" + (detail_num + 1).ToString () + "之內容";
			GUI.Button (new Rect(Screen.width / 10, Screen.height / 10 * 9, Screen.width / 10 * 2, Screen.height / 15), "新增步驟");
			detail = "";  //初始化step string

			for (int j = 1; j <= detail_num; j++) //存入所有steps
			{
				detail += ("\"detail" + j + "\":\"" + detailString [j] + "\"");
				if (j != detail_num)
					detail += ",\n\t\t\t";
			}

			//new_step = step;
			//new_step_num = detail_num;

		}
	}

	public void EnterContent(Text enterText)
	{
		if (enterText.text == "") 
		{
			SceneManager.LoadScene ("StepDetail");
		} 
		else 
		{
			if (detail_num != 0)
				detail_dot = ",";
			total_detail = "{\"name\":\"step" + choose_step_num + "\",\n\"detail_num\":" + detail_num.ToString () + detail_dot + detail + "}";

			//-------寫入在編輯事件之前之事件
			for (int i = 1; i < get_event_num; i++) 
			{
				JsonWriter jsonWriter = new JsonWriter ();
				jsonWriter.PrettyPrint = true;
				jsonWriter.IndentValue = 4;
				JsonMapper.ToJson (jsonData ["subject" + i.ToString ()], jsonWriter);
				subject += "\"subject" + i.ToString () + "\":" + jsonWriter.ToString () + ",";

			}

			//------寫入編輯的事件
			JsonWriter jsonWriter_now = new JsonWriter ();
			jsonWriter_now.PrettyPrint = true;
			jsonWriter_now.IndentValue = 4;
			JsonMapper.ToJson (jsonData ["subject" + get_event_num.ToString ()][0], jsonWriter_now);

			subject += "\"subject" + get_event_num.ToString () + "\":[" + jsonWriter_now.ToString () + ","; //將detail之前的資料寫入

			for (int i = 1; i < choose_step_num; i++) 
			{
				JsonWriter jsonWriter_now1 = new JsonWriter ();
				jsonWriter_now1.PrettyPrint = true;
				jsonWriter_now1.IndentValue = 4;
				JsonMapper.ToJson (jsonData ["subject" + get_event_num.ToString ()][i], jsonWriter_now1);
				subject += jsonWriter_now1.ToString () + ",";
			}
			subject += total_detail;
			int get_step_num = Int32.Parse (jsonData ["subject" + get_event_num.ToString ()] [0] ["step_num"].ToString ());
			if (choose_step_num != get_step_num)
				subject += ",";
			for (int i = (choose_step_num + 1); i <= get_step_num; i++) 
			{
				JsonWriter jsonWriter_now2 = new JsonWriter ();
				jsonWriter_now2.PrettyPrint = true;
				jsonWriter_now2.IndentValue = 4;
				JsonMapper.ToJson (jsonData ["subject" + get_event_num.ToString ()][i], jsonWriter_now2);
				subject += jsonWriter_now2.ToString ();
				if (i != get_step_num)
					subject += ",";
			}

			subject += "]";
			int get_total_event_num = Int32.Parse (jsonData ["event_num"].ToString ());
			if (get_event_num != get_total_event_num)
				subject += ",";

			//-------------寫入在編輯事件之後的事件
			for (int i = (get_event_num + 1); i <= get_total_event_num; i++) 
			{
				JsonWriter jsonWriter_post = new JsonWriter ();
				jsonWriter_post.PrettyPrint = true;
				jsonWriter_post.IndentValue = 4;
				JsonMapper.ToJson (jsonData ["subject" + i.ToString ()], jsonWriter_post);
				subject += "\"subject" + i.ToString () + "\":" + jsonWriter_post.ToString ();
				if (i != get_total_event_num)
					subject += ",";
			}

			string final = "{\n\t\"day_num\":" + get_day_num + ",\n\t\"event_num\":" + get_total_event_num.ToString () + "," + subject + "\n}";
			File.WriteAllText (Application.persistentDataPath + "/day" + get_day_num.ToString () + ".json", final);

			//---重新美化後寫入
			MyWrite = File.ReadAllText (Application.persistentDataPath + "/day" + get_day_num.ToString () + ".json");
			jsonData = JsonMapper.ToObject (MyWrite);
			JsonWriter jsonWriter_final = new JsonWriter ();
			jsonWriter_final.PrettyPrint = true;
			jsonWriter_final.IndentValue = 4;
			JsonMapper.ToJson (jsonData, jsonWriter_final);
			File.WriteAllText (Application.persistentDataPath + "/day" + get_day_num.ToString () + ".json", jsonWriter_final.ToString ());

			SceneManager.LoadScene ("StepDetail");
		}
	}



	// Update is called once per frame
	void Update()
	{
		SelectButton_Update();
	}

	Touch touch;
	void SelectButton_Update()
	{
		if (Input.touchCount > 0)
		{
			touch = Input.touches[0];
			if (touch.phase == TouchPhase.Moved)
			{
				if (touch.deltaPosition.y > 0)
					scrollPosition.y = scrollPosition.y + touch.deltaPosition.y + 20;
				else
					scrollPosition.y = scrollPosition.y + touch.deltaPosition.y - 20;
			}
		}
	}


	public void DeleteDetail()
	{
		if (choose != 0) 
		{
			//from_show_step = 1;

			//-----存入step detail要寫入後端之資料
			detail = "";  //初始化step string

			for (int j = 1; j < choose; j++) //存入所有steps
			{
				detail += ("\"detail" + j + "\":\"" + detailString [j] + "\"");
				if ((j + 1) != detail_num)
					detail += ",\n\t\t\t";
			}
			for (int j = (choose + 1); j <= detail_num; j++) 
			{
				detailString [j - 1] = detailString [j];
				detail += "\"detail" + (j - 1).ToString () + "\":\"" + detailString [j] + "\"";
				if (j != detail_num)
					detail += ",";
			}
			print (detail);

			detail_num--;
			stringToEdit = "請輸入細項" + (detail_num + 1).ToString () + "之內容";

		}
		choose = 0;

	}

}
