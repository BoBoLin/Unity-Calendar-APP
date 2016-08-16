using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;
using LitJson;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class ShowSteps : MonoBehaviour {
	int get_event_num = ShowEvents.event_num;
	int get_day_num = home.num;
    private Vector2 scrollPosition = Vector2.zero;

    public GUISkin GUISkin;
	public GUISkin GUISkinAddStep;
	public GUISkin GUISkinDelete;

	Text text;
	private string MyWrite;
	private JsonData jsonData;

	public string get_step_num = "";
	public int tmp_step_num = 0;
    public static int from_show_step ;

	public int choose = 0;
	public string step = "";

	// Use this for initialization
	void Start () {

        from_show_step = 1;

        // ----  set Title Name
        if (get_day_num == 0)
			SceneManager.LoadScene ("home");
		else 
		{
			MyWrite = File.ReadAllText (Application.persistentDataPath + "/day" + get_day_num + ".json");
			jsonData = JsonMapper.ToObject (MyWrite);

			text = GameObject.Find ("Title").GetComponent<Text> ();
            //text.fontSize = Screen.height / 5 ;
			text.text = jsonData ["subject" + get_event_num.ToString ()] [0] ["name"].ToString ();
            

            get_step_num = jsonData ["subject" + get_event_num.ToString ()] [0] ["step_num"].ToString ();
			tmp_step_num = Int32.Parse (get_step_num);

		}
	}

	// Update is called once per frame
	void Update () {
        SelectButton_Update();
        if (Input.GetKeyUp(KeyCode.Escape))//當手指放開按鈕後才會動作
		{
			//相當於Android的返回按鈕
			SceneManager.LoadScene("Event");

		}
		if (Input.GetKeyUp(KeyCode.Menu))//當手指放開按鈕後才會動作
		{
			//相當於Android的選單鈕
		}
	
	}

	public void OnGUI()
	{
		GUI.skin = GUISkin;
        GUI.skin.button.fontSize = Screen.height / 30;
        GUI.skin.label.fontSize = Screen.height / 30;
        GUI.skin.verticalScrollbarThumb.fixedWidth = 10;
        GUI.skin.verticalScrollbar.fixedWidth = 10;
        scrollPosition = GUI.BeginScrollView(new Rect(Screen.width / 12, Screen.height / 6, Screen.width - Screen.width / 12, Screen.height * 4 / 5),
                                             scrollPosition,
                                             new Rect(0, 0, Screen.width * 11 / 12 - 15, tmp_step_num * Screen.height / 10));
        for (int i = 0; i < tmp_step_num; i++) 
		{
			GUI.Label (new Rect ( 0, Screen.height / 10 * i, Screen.width / 10 * 2, Screen.height / 10), "第" + (i + 1).ToString () + "步 : ");

			string[] split_step =  Regex.Split(jsonData ["subject" + get_event_num.ToString()] [0] ["step"+(i+1).ToString()].ToString (), "/f:", RegexOptions.IgnoreCase);  //將字串分為前面是步驟名稱 後面是是否完成

			if (GUI.Button (new Rect (Screen.width / 5, Screen.height / 10 * i , Screen.width / 10 * 6, Screen.height / 10), split_step[0])) 
			{
				choose = i + 1;
			}

			int finish_tmp = 0;
			finish_tmp = Int32.Parse (split_step [1]); //得到是否完成的值
			if(finish_tmp == 1)
			{
				GUI.skin = GUISkinDelete; //印出完成的圖案
				GUI.Label(new Rect(Screen.width/11*9, Screen.height / 10 *i , Screen.width / 12, Screen.height / 14), "");
			}
			GUI.skin = GUISkin; //初始化guiskin
		}

		GUI.skin = GUISkinAddStep;

		if (choose != 0)
		{
			GUI.Label(new Rect(Screen.width/10*8, Screen.height / 10 * (choose-1), Screen.width / 12, Screen.height / 12), "");
		}

        GUI.EndScrollView();
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

	public void FinishStep()
	{
		if (choose != 0) 
		{
			string[] split_step = Regex.Split (jsonData ["subject" + get_event_num.ToString ()] [0] ["step" + choose.ToString ()].ToString (), "/f:", RegexOptions.IgnoreCase); //將字串分為前面是步驟名稱 後面是是否完成

			jsonData ["subject" + get_event_num.ToString ()] [0] ["step"+choose.ToString()] = split_step[0]+"/f:1";

			JsonWriter jsonWriter = new JsonWriter ();
			jsonWriter.PrettyPrint = true;
			jsonWriter.IndentValue = 4;
			JsonMapper.ToJson (jsonData, jsonWriter);

			File.WriteAllText (Application.persistentDataPath + "/day" + get_day_num + ".json", jsonWriter.ToString ());

			choose = 0;

		}
			
	}

	public void Unfinish()
	{
		if(choose != 0)
		{
			string[] split_step = Regex.Split (jsonData ["subject" + get_event_num.ToString ()] [0] ["step" + choose.ToString ()].ToString (), "/f:", RegexOptions.IgnoreCase); //將字串分為前面是步驟名稱 後面是是否完成

			jsonData ["subject" + get_event_num.ToString ()] [0] ["step"+choose.ToString()] = split_step[0]+"/f:0";

			JsonWriter jsonWriter = new JsonWriter ();
			jsonWriter.PrettyPrint = true;
			jsonWriter.IndentValue = 4;
			JsonMapper.ToJson (jsonData, jsonWriter);

			File.WriteAllText (Application.persistentDataPath + "/day" + get_day_num + ".json", jsonWriter.ToString ());

			choose = 0;
		}
		
	}

}
