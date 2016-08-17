using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class AddEvent : MonoBehaviour {
	public GUISkin GUISkinAddStep;
	public GUISkin GUISkinShowStep;
	public GUISkin GUISkinDelete;

	private string MyWrite;
	private JsonData jsonData;

	public string get_day_num;
	public string get_event_num;
	public string get_step_num;
	public string subject = "";
	public string step = "";
	public string old_step = "";
	public string step_dot ="";

	int event_num = ShowEvents.event_num;
	int condition = ShowEvents.condition;
	int select_num = home.num;
    int from_show_step = ShowSteps.from_show_step;

	int position = 6;
	Text title;
    Button button;
	InputField InputFieldName;

	public int tmp_step = 0;
	public string stringToEdit = "";

	public int position_old = 0;
	public string new_step = "";
	public int new_step_num = 0;

	public static string[] stepStringArray= new string[50];
	public string[] tmp_arr = new string[50]; 
	public static int select_step_num = 0;
	public static int tmp_step_num = 0;

	int if_edit = EditStep.edit;
	int tmp_step_num_edit = EditStep.tmp_step_num_edit;
	string[] stepStringArray_edit = EditStep.stepStringArray_edit;


    private Vector2 scrollPosition = Vector2.zero;

    public int choose = 0;

    void Start()
	{

        if (from_show_step == 1)
            if_edit = 0;
        else
            if_edit = 1;

		stepStringArray [0] = ""; //初始化array[0]
		MyWrite = File.ReadAllText (Application.persistentDataPath + "/day" + select_num + ".json");
		jsonData = JsonMapper.ToObject (MyWrite);
		get_event_num = jsonData ["event_num"].ToString ();
		stringToEdit = "請輸入步驟1之內容";
		tmp_step_num = 0; //初始化 step num

		if (condition == 1) {  //編輯模式下
			title = GameObject.Find ("Title").GetComponent<Text> ();
			title.text = "編輯步驟";
            title = GameObject.Find("send").GetComponent<Text>();
            title.text = "save";
            InputFieldName = GameObject.Find ("InputField").GetComponent<InputField> ();
			InputFieldName.text = jsonData ["subject" + event_num.ToString ()] [0] ["name"].ToString ();

			get_step_num = jsonData ["subject" + event_num.ToString ()] [0] ["step_num"].ToString ();

			if (if_edit != 1) {  //還沒編輯step過
				tmp_step_num = Int32.Parse (get_step_num); //查看原先有多少筆steps
				stringToEdit = "請輸入步驟" + (tmp_step_num + 1).ToString () + "之內容";
				for (int i = 1; i <= tmp_step_num; i++) {
					stepStringArray [i] = jsonData ["subject" + event_num.ToString ()] [0] ["step" + i.ToString ()].ToString ();
				}
				position_old = position + tmp_step_num;

				for (int j = 1; j <= tmp_step_num; j++) { //存入所有steps
					step += ("\"step" + j + "\":\"" + stepStringArray [j] + "\"");
					if (j != tmp_step_num)
						step += ",\n\t\t\t";
				}
				new_step = step;
				new_step_num = tmp_step_num;
			} else {
				tmp_step_num = tmp_step_num_edit;
				stringToEdit = "請輸入步驟" + (tmp_step_num + 1).ToString () + "之內容";
				stepStringArray = stepStringArray_edit;
				position_old = position + tmp_step_num;

				for (int j = 1; j <= tmp_step_num; j++) { //存入所有steps
					step += ("\"step" + j + "\":\"" + stepStringArray [j] + "\"");
					if (j != tmp_step_num)
						step += ",\n\t\t\t";
				}
				new_step = step;
				new_step_num = tmp_step_num;
			}
		}

        else
        {
            Button hideButton;
            hideButton = GameObject.Find("delete").GetComponent<Button>();
            hideButton.gameObject.SetActive(false);
            hideButton = GameObject.Find("edit").GetComponent<Button>();
            hideButton.gameObject.SetActive(false);
            hideButton = GameObject.Find("insert").GetComponent<Button>();
            hideButton.gameObject.SetActive(false);
			
			Button updatePositionButton ;
			updatePositionButton = GameObject.Find("Button").GetComponent<Button>();
			updatePositionButton.transform.position = new Vector3(Screen.width * 6 / 7 , Screen.height * 9 / 10, 0);
			
			updatePositionButton = GameObject.Find("backhomeButton").GetComponent<Button>();
			updatePositionButton.transform.position = new Vector3(Screen.width / 9, Screen.height * 9 / 10, 0);
			
			Text updatePositionText ;
			updatePositionText = GameObject.Find("NameText").GetComponent<Text>();
			updatePositionText.transform.position = new Vector3(Screen.width / 9, Screen.height * 4 / 5, 0);
			
			updatePositionText = GameObject.Find("Title").GetComponent<Text>();
			updatePositionText.transform.position = new Vector3(Screen.width / 2, Screen.height * 9 / 10, 0);			
            
			InputField updatePositionInputfield ;
			updatePositionInputfield = GameObject.Find("InputField").GetComponent<InputField>();
			updatePositionInputfield.transform.position = new Vector3(Screen.width * 6 / 11, Screen.height * 4 / 5, 0);			
			
        }
    }

	public void OnGUI()
	{


		GUI.skin = GUISkinAddStep;


		if (condition == 1) //編輯event模式
		{
            GUI.skin.verticalScrollbarThumb.fixedWidth = 5;
            GUI.skin.verticalScrollbar.fixedWidth = 5;
            scrollPosition = GUI.BeginScrollView(new Rect(0, Screen.height / 10 * 3, Screen.width, Screen.height / 2),
                                                 scrollPosition,
                                                 new Rect(0, 0, Screen.width * 5 / 6 - 15, tmp_step_num * Screen.height / 10));
            if (choose != 0)
            {
                GUI.Label(new Rect(Screen.width * 9 / 10, Screen.height / 10 * (choose - 1), Screen.width / 12, Screen.height / 12), "");
            } // 已完成標記

            if (tmp_step_num == 0)
				stepStringArray[1] = "請輸入步驟1之內容";
			for (int i = 0; i < tmp_step_num; i++) 
			{
				GUI.skin = GUISkinShowStep;
                GUI.skin.label.fontSize = Screen.height / 30;
                GUI.skin.button.fontSize = Screen.height / 20;
                GUI.Label (new Rect(0, Screen.height / 10 * i, Screen.width / 10 * 2, Screen.height / 10), "第" + (i + 1).ToString () + "步 : ");

				string[] split_step =  Regex.Split(stepStringArray [i + 1], "/f:", RegexOptions.IgnoreCase);  //將字串分為前面是步驟名稱 後面是是否完成

				if(GUI.Button (new Rect(Screen.width / 5 + 5, Screen.height / 10 * i, Screen.width / 10 * 7, Screen.height / 10), split_step[0]))
				{
                    select_step_num = i + 1;
                    choose = i + 1;
				}
			}

            GUI.EndScrollView();

            GUI.skin = GUISkinAddStep;
            GUI.skin.textField.fontSize = Screen.height / 20;
            stringToEdit = GUI.TextField(new Rect(Screen.width / 10, Screen.height / 10 * 8, Screen.width / 10 * 8, Screen.height / 10), stringToEdit, 10);
            GUI.skin.button.fontSize = Screen.height / 30;
            if (GUI.Button (new Rect(Screen.width / 10 * 3, Screen.height / 10 * 9, Screen.width / 10 * 4, Screen.height / 15), "新增此步驟"))   
			{
                from_show_step = 1;
                tmp_step_num++; //步驟數+1
				stepStringArray [tmp_step_num] = stringToEdit+"/f:0";
				stringToEdit = "請輸入步驟" + (tmp_step_num + 1).ToString () + "之內容";
				GUI.Button (new Rect(Screen.width / 10, Screen.height / 10 * 9, Screen.width / 10 * 2, Screen.height / 15), "新增步驟");
				step = "";  //初始化step string

				for (int j = 1; j <= tmp_step_num; j++) //存入所有steps
				{
					step += ("\"step" + j + "\":\"" + stepStringArray [j] + "\"");
					if (j != tmp_step_num)
						step += ",\n\t\t\t";
				}
				new_step = step;
				new_step_num = tmp_step_num;
			}

		}
		else //新增event模式
		{
            GUI.skin.verticalScrollbarThumb.fixedWidth = 10;
            GUI.skin.verticalScrollbar.fixedWidth = 10;
            scrollPosition = GUI.BeginScrollView(new Rect(Screen.width / 12, Screen.height / 4, Screen.width * 11 / 12, Screen.height / 20 * 11 ),
                                                 scrollPosition,
                                                 new Rect(0, 0, Screen.width * 11 / 12 - 15 , tmp_step_num * Screen.height / 10 ));
            //BeginScrollView->設置拉bar的介面，當超過Screen.height，拉bar就會出現 
            if (choose != 0)
            {
                GUI.Label(new Rect(Screen.width / 4, Screen.height / 10 * (choose - 1), Screen.width / 12, Screen.height / 12), "");
            } // 已完成步驟標記

            if (tmp_step_num == 0)
				stepStringArray[1] = "請輸入步驟1之內容";
			for (int i = 0; i < tmp_step_num; i++) 
			{
				GUI.skin = GUISkinShowStep;
                GUI.skin.label.fontSize = Screen.height / 30;
                GUI.skin.button.fontSize = Screen.height / 30;
                GUI.Label(new Rect(0, Screen.height / 10 * i, Screen.width / 10 * 2, Screen.height / 10), "第" + (i + 1).ToString() + "步 : ");

				string[] split_step =  Regex.Split(stepStringArray [i + 1], "/f:", RegexOptions.IgnoreCase);  //將字串分為前面是步驟名稱 後面是是否完成

				if(GUI.Button (new Rect (Screen.width / 5, Screen.height / 10 * i, Screen.width / 10 * 6, Screen.height / 10), split_step[0]))
				{
						
				}
			}

			if (tmp_step_num != 0) {
				GUI.skin = GUISkinDelete;  //印出刪除step按鍵
				if (GUI.Button (new Rect (Screen.width / 10 * 8, Screen.height / 10 * (tmp_step_num-1), Screen.width / 11, Screen.height / 11), "")) { //按下刪除最後步驟
					step = "";
					for (int j = 1; j < tmp_step_num; j++) { //存入所有steps
						step += ("\"step" + j + "\":\"" + stepStringArray [j] + "\"");
						if (j + 1 != tmp_step_num)
							step += ",\n\t\t\t";
					}
					new_step = step;
					tmp_step_num--;
					new_step_num = tmp_step_num;
					position_old--;
					stringToEdit = "請輸入步驟" + (tmp_step_num + 1).ToString () + "之內容";
				}
			}

            GUI.EndScrollView();

            GUI.skin = GUISkinAddStep;
            GUI.skin.textField.fontSize = Screen.height / 20;
            GUI.skin.button.fontSize = Screen.height / 30;
			stringToEdit = GUI.TextField (new Rect (Screen.width / 10, Screen.height / 10 * 8, Screen.width / 10 * 8, Screen.height / 10), stringToEdit, 10);

            if (GUI.Button(new Rect(Screen.width / 10 * 3, Screen.height / 10 * 9, Screen.width / 10 * 4, Screen.height / 15), "新增此步驟"))
            {
                tmp_step_num++; //步驟數+1
                stepStringArray[tmp_step_num] = stringToEdit+"/f:0";
                stringToEdit = "請輸入步驟" + (tmp_step_num + 1).ToString() + "之內容";
                GUI.Button(new Rect(Screen.width / 10, Screen.height / 10 * 9, Screen.width / 10 * 2, Screen.height / 15), "新增步驟");
                step = "";  //初始化step string

                for (int j = 1; j <= tmp_step_num; j++) //存入所有steps
                {
                    step += ("\"step" + j + "\":\"" + stepStringArray[j] + "\"");
                    if (j != tmp_step_num)
                        step += ",\n\t\t\t";
                }

            }

            //GUI.EndScrollView();
        }
	}

	public void EnterContent(Text enterText)
	{
		if (enterText.text == "")
		{
			if (condition == 0)
				SceneManager.LoadScene ("Event");
			else
				SceneManager.LoadScene ("EventStep");
		}
		else if (condition == 0)  //新增事件模式
		{
			get_day_num = jsonData ["day_num"].ToString ();

			int tmp_event_num = Int32.Parse (jsonData ["event_num"].ToString ());

			for (int i = 1; i <= tmp_event_num; i++)  //寫入原本的資料
			{
				get_step_num = jsonData ["subject" + i.ToString ()] [0] ["step_num"].ToString ();
				int tmp_old_step_num = Int32.Parse (get_step_num);

				old_step = ""; //初始化原本的step
				for (int x = 1; x <= tmp_old_step_num; x++) 
				{
					old_step += ("\"step" + x + "\":\"" + jsonData ["subject" + i.ToString ()] [0] ["step" + x.ToString ()].ToString () + "\"");
					if(x != tmp_old_step_num)
						old_step += ",\n\t\t\t";
				}
				if (tmp_old_step_num != 0)
					step_dot = ",";
				else
					step_dot = "";

				subject += "\n\t\"subject" + i.ToString () + "\":[\n\t\t{\n\t\t\t\"name\":\"" + jsonData ["subject" + i.ToString ()] [0] ["name"] + "\",\n\t\t\t\"step_num\":" + get_step_num +step_dot+"\n\t\t\t"+old_step+ "\n\t\t}\n\t],";
			}

			tmp_event_num++;
			if (tmp_step_num != 0)
				step_dot = ",";
			else
				step_dot = "";
			string final = "{\n\t\"day_num\":" + get_day_num + ",\n\t\"event_num\":" + tmp_event_num.ToString () + ", " + subject + "\n\t\"subject" + tmp_event_num.ToString () + "\":[\n\t\t{\n\t\t\t\"name\":\"" + enterText.text + "\",\n\t\t\t\"step_num\":"+tmp_step_num+step_dot+"\n\t\t\t"+step+"\n\t\t}\n\t]\n}";

			File.WriteAllText (Application.persistentDataPath + "/day" + select_num.ToString () + ".json", final);

			SceneManager.LoadScene ("Event");
		} 
		else  //進入編輯event模式
		{
			get_day_num = jsonData ["day_num"].ToString ();
			int tmp_event_num = Int32.Parse (jsonData ["event_num"].ToString ());

			//-------寫入在編輯事件之前之事件
			for (int i = 1; i < event_num; i++) 
			{
				get_step_num = jsonData ["subject" + i.ToString ()] [0] ["step_num"].ToString ();
				int e_tmp_step_num = Int32.Parse (get_step_num);
				step = "";//初始化step
				for (int y = 1; y <= e_tmp_step_num; y++) 
				{
					step += ("\"step" + y + "\":\"" + jsonData ["subject" + i.ToString ()] [0] ["step" + y.ToString ()].ToString () + "\"");
					if (y != e_tmp_step_num)
						step += ",\n\t\t\t";
				}
				if (e_tmp_step_num != 0)
					step_dot = ",";
				else
					step_dot = "";

				subject += "\n\t\"subject" + i.ToString () + "\":[\n\t\t{\n\t\t\t\"name\":\"" + jsonData ["subject" + i.ToString ()] [0] ["name"] + "\",\n\t\t\t\"step_num\":" + get_step_num + step_dot + "\n\t\t\t" + step + "\n\t\t}\n\t],";
			}

			//------------------寫入編輯的事件
			if (new_step_num != 0)
				step_dot = ",";
			else
				step_dot = "";
			subject += "\n\t\"subject" + event_num.ToString () + "\":[\n\t\t{\n\t\t\t\"name\":\"" + enterText.text + "\",\n\t\t\t\"step_num\":" + new_step_num + step_dot + "\n\t\t\t" + new_step + "\n\t\t}\n\t]";  //改變此event編輯的名稱

			if(event_num != tmp_event_num)
				subject += ",";

			//-----------------寫入在編輯事件之後的事件
			for (int j = event_num + 1; j <= tmp_event_num; j++) 
			{
				get_step_num = jsonData ["subject" + j.ToString ()] [0] ["step_num"].ToString ();
				int e_tmp_step_num = Int32.Parse (get_step_num);
				step = "";//初始化step
				for (int y = 1; y <= e_tmp_step_num; y++) 
				{
					step += ("\"step" + y + "\":\"" + jsonData ["subject" + j.ToString ()] [0] ["step" + y.ToString ()].ToString () + "\"");
					if (y != e_tmp_step_num)
						step += ",\n\t\t\t";
				}
				if (e_tmp_step_num != 0)
					step_dot = ",";
				else
					step_dot = "";


				subject += "\n\t\"subject" + j.ToString () + "\":[\n\t\t{\n\t\t\t\"name\":\"" + jsonData ["subject" + j.ToString ()] [0] ["name"] + "\",\n\t\t\t\"step_num\":" + get_step_num + step_dot + "\n\t\t\t" + step + "\n\t\t}\n\t]";
				if (j != tmp_event_num) 
					subject += ",";
			}

			string final = "{\n\t\"day_num\":"+get_day_num+",\n\t\"event_num\":"+tmp_event_num.ToString()+","+ subject +"\n}";
			File.WriteAllText (Application.persistentDataPath + "/day" + select_num.ToString () + ".json", final);

			SceneManager.LoadScene ("EventStep");

		}

	}

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

	public void GoEditStep()
	{
		if (choose != 0) 
		{
			ShowSteps.from_show_step = 0;
			SceneManager.LoadScene ("EditStep");
		}
	}

	public void DeleteStep()
	{
		if (choose != 0) 
		{
			from_show_step = 1;
			step = "";
			for (int j = 1; j < choose; j++) { //存入所有steps
				step += ("\"step" + j + "\":\"" + stepStringArray [j] + "\"");
				if (j + 1 != tmp_step_num)
					step += ",\n\t\t\t";
			}
			for (int j = (choose + 1); j <= tmp_step_num; j++) 
			{
				stepStringArray [j - 1] = stepStringArray [j];
				step += ("\"step" + (j-1) + "\":\"" + stepStringArray [j] + "\"");
				if(j != tmp_step_num)
					step += ",\n\t\t\t";
			}
			new_step = step;
			tmp_step_num--;
			new_step_num = tmp_step_num;
			//position_old--;
			stringToEdit = "請輸入步驟" + (tmp_step_num + 1).ToString () + "之內容";
		}
		choose = 0;

	}

	public void InsertStep()
	{
		if (choose != 0) 
		{
			from_show_step = 1;
			step = "";

			for (int j = choose; j <= tmp_step_num; j++) 
			{
				tmp_arr [j + 1] = stepStringArray [j];
			}
				
			for (int j = 1; j < choose; j++) { //存入所有steps
				step += ("\"step" + j + "\":\"" + stepStringArray [j] + "\"");
				if (j + 1 != tmp_step_num)
					step += ",\n\t\t\t";
			}
			stepStringArray [choose] = "請編輯插入之內容/f:0";
			step += ("\"step" + choose.ToString () + "\":\"請編輯插入之內容/f:0\",\n\t\t\t");
			tmp_step_num++;
			new_step_num = tmp_step_num;
			for (int j = (choose + 1); j <= tmp_step_num; j++) 
			{
				stepStringArray [j] = tmp_arr[j];
				step += ("\"step" + j + "\":\"" + stepStringArray [j] + "\"");
				if(j != tmp_step_num)
					step += ",\n\t\t\t";
			}
			new_step = step;
			stringToEdit = "請輸入步驟" + (tmp_step_num + 1).ToString () + "之內容";
		}

		choose = 0;
	}
		
}
