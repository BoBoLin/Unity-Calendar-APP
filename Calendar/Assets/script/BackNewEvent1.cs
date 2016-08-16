using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackNewEvent1 : MonoBehaviour
{
	int condition = ShowEvents.condition;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))//當手指放開按鈕後才會動作
        {
            //相當於Android的返回按鈕
			if(condition == 0)
				SceneManager.LoadScene("Event");
			if (condition == 1)
				SceneManager.LoadScene ("EventStep");

        }
        if (Input.GetKeyUp(KeyCode.Menu))//當手指放開按鈕後才會動作
        {
            //相當於Android的選單鈕
        }

    }
}
