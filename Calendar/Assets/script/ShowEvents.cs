using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class ShowEvents : MonoBehaviour
{
	private string MyWrite;
	private JsonData jsonData;
	public int subject_num = 1;
	public int tmp_num = 0;
    public GUISkin GUISkin;
	public GUISkin GUISKinDelete;

	public string get_day_num;
	public string get_event_num;
	public string get_step_num;
	public int tmp_step_num= 0;
	public string subject;
	public string step;
	public string step_dot = "";

	int select_num = home.num;
	public static int event_num;
	public static int condition = 0; // 設定此condition 在新增事件頁面裡可以知道前一個頁面是從Event or Add_Event (0:Event  1:Add_Event)
    public Vector2 scrollPosition = Vector2.zero;
    void Start()
    {
		condition = 0;
		if (select_num == 0)  // 如果不正常進入入Event Scene, 回到home Scene
			SceneManager.LoadScene ("home");
		
		else if (File.Exists (Application.persistentDataPath + "/day"+select_num+".json"))  // 如果已經建立過此json, 直接read
		{
			MyWrite = File.ReadAllText (Application.persistentDataPath + "/day"+select_num+".json");
		}
		else   //如果還沒建立過, 建立自訂預設之json檔
		{
			FileInfo file = new FileInfo(Application.persistentDataPath + "/day"+select_num+".json");//在平台上建立一個路徑
			StreamWriter writer = file.CreateText(); 
			writer.Write("{\n\t\"day_num\":"+select_num+",\n\t\"event_num\":0\n}"); 
			writer.Close();   
			writer.Dispose(); 

			StreamReader reader = new StreamReader(Application.persistentDataPath + "/day"+select_num+".json");
			MyWrite = reader.ReadToEnd();
			reader.Close();
			reader.Dispose();
		}

		if (select_num != 0) 
		{
			jsonData = JsonMapper.ToObject (MyWrite);
			tmp_num = Int32.Parse (jsonData ["event_num"].ToString ());
		}
			
    }

    public void OnGUI()
    {
		get_day_num = jsonData ["day_num"].ToString ();
		get_event_num = jsonData ["event_num"].ToString ();

		int tmp_event_num = Int32.Parse (get_event_num);


		string dot="";
        GUI.skin.verticalScrollbarThumb.fixedWidth = 10;
        GUI.skin.verticalScrollbar.fixedWidth = 10;
        scrollPosition = GUI.BeginScrollView(new Rect(Screen.width * 7 / 100, Screen.height / 5, Screen.width - Screen.width * 7 / 100, Screen.height - Screen.height / 5),
                                             scrollPosition,
                                             new Rect(0, 0, Screen.width - Screen.width * 7 / 100 - 15, Screen.height / 10 * tmp_event_num));
        //BeginScrollView->設置拉bar的介面，當超過Screen.height，拉bar就會出現
        for (int i = 0; i < tmp_num; i++)
        {
			GUI.skin = GUISkin;
            GUI.skin.button.fontSize = Screen.height / 20;
            if (GUI.Button (new Rect(0, Screen.height / 10 * i, Screen.width / 10 * 6, Screen.height / 13), jsonData ["subject" + (i + 1).ToString ()] [0] ["name"].ToString ()))  //Screen.width/10 * 4, Screen.width/10
			{
				event_num = i + 1;
				condition = 1; //將控制變數更改
				SceneManager.LoadScene("EventStep");
			}

			GUI.skin = GUISKinDelete; //印出刪除案件與刪除事件觸發
			if (GUI.Button(new Rect(Screen.width / 10 * 7, Screen.height / 10 * i, Screen.width / 8, Screen.height / 13), "")) 
            {

				for (int x = 1; x < (i+1); x++)  //存入刪除之事件之前之所有事件
				{
					get_step_num = jsonData ["subject" + x.ToString ()] [0] ["step_num"].ToString ();
					tmp_step_num = Int32.Parse (get_step_num);
					step = "";//初始化step
					for (int y = 1; y <= tmp_step_num; y++) 
					{
						step += ("\"step" + y + "\":\"" + jsonData ["subject" + x.ToString ()] [0] ["step" + y.ToString ()].ToString () + "\"");
						if (y != tmp_step_num)
							step += ",\n\t\t\t";
					}
					if (tmp_step_num != 0)
						step_dot = ",";
					else
						step_dot = "";
						
					subject += "\n\t\"subject" + x.ToString () + "\":[\n\t\t{\n\t\t\t\"name\":\"" + jsonData ["subject" + x.ToString ()] [0] ["name"] + "\",\n\t\t\t\"step_num\":" + get_step_num + step_dot + "\n\t\t\t" + step + "\n\t\t}\n\t]";
					if (x + 1 != tmp_num) 
						subject += ",";

				}

				for (int x = i + 2; x <= tmp_num; x++)
				{
					get_step_num = jsonData ["subject" + x.ToString ()] [0] ["step_num"].ToString ();
					tmp_step_num = Int32.Parse (get_step_num);
					step = "";//初始化step
					for (int y = 1; y <= tmp_step_num; y++) 
					{
						step += ("\"step" + y + "\":\"" + jsonData ["subject" + x.ToString ()] [0] ["step" + y.ToString ()].ToString () + "\"");
						if (y != tmp_step_num)
							step += ",\n\t\t\t";
					}
					if (tmp_step_num != 0)
						step_dot = ",";
					else
						step_dot = "";

					subject += "\n\t\"subject" + (x - 1).ToString () + "\":[\n\t\t{\n\t\t\t\"name\":\"" + jsonData ["subject" + x.ToString ()] [0] ["name"] + "\",\n\t\t\t\"step_num\":" + get_step_num + step_dot + "\n\t\t\t" + step + "\n\t\t}\n\t]";
					if (x != tmp_num)
						subject += ",";
				}
				tmp_num --;

				if (tmp_num != 0)
					dot = ",";
				string final = "{\n\t\"day_num\":"+get_day_num+",\n\t\"event_num\":"+tmp_num.ToString()+ dot+ subject +"\n}";

				File.WriteAllText (Application.persistentDataPath + "/day"+select_num+".json", final);

                SceneManager.LoadScene("Event");
            }
        }

        GUI.EndScrollView();

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

}
