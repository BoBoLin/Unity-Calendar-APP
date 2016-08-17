using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text.RegularExpressions;

public class EditStep : MonoBehaviour {

	int select_step_num = AddEvent.select_step_num;  //第幾個step
	int step_num_edit = AddEvent.tmp_step_num;
	public static int tmp_step_num_edit;
	public static string[] stepStringArray_edit = AddEvent.stepStringArray; 
	public static int edit = 0;
	public string[] split_step = new string[2];

	InputField InputFieldName;

	public string get_step_content = "";

	// Use this for initialization
	void Start () {
		tmp_step_num_edit = step_num_edit;
		print (tmp_step_num_edit + "get step num");
		edit = 1;

		get_step_content = stepStringArray_edit[select_step_num];
		split_step =  Regex.Split(get_step_content, "/f:", RegexOptions.IgnoreCase);  //將字串分為前面是步驟名稱 後面是是否完成

		InputFieldName = GameObject.Find("InputField").GetComponent<InputField>();
		InputFieldName.text = split_step [0];

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape))//當手指放開按鈕後才會動作
		{
			//相當於Android的返回按鈕
			SceneManager.LoadScene("Add_Event");
		}
		if (Input.GetKeyUp(KeyCode.Menu))//當手指放開按鈕後才會動作
		{
			//相當於Android的選單鈕
		}
	
	}

	public void EnterContent(Text enterText)
	{
		stepStringArray_edit [select_step_num] = enterText.text + "/f:" + split_step [1];
		SceneManager.LoadScene ("Add_Event");

	}


}
