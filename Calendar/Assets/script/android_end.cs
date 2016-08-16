using UnityEngine;
using System.Collections;

public class android_end : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))//當手指放開按鈕後才會動作
        {
            //相當於Android的返回按鈕
            Application.Quit();
        }
        if (Input.GetKeyUp(KeyCode.Menu))//當手指放開按鈕後才會動作
        {
            //相當於Android的選單鈕
        }
    }

}
