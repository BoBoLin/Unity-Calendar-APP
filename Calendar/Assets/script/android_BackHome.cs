using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class android_BackHome : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))//當手指放開按鈕後才會動作
        {
            //相當於Android的返回按鈕
            SceneManager.LoadScene("home");

        }
        if (Input.GetKeyUp(KeyCode.Menu))//當手指放開按鈕後才會動作
        {
            //相當於Android的選單鈕
        }

    }
}
