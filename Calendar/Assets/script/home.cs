using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class home : MonoBehaviour {

	public static int num = 0 ;

    public void TriggerButton1Behavior(int i)
    {
		
        switch (i)
        {
            default:
                break;
			case (1):
				num = 1;
				SceneManager.LoadScene ("Event");
				break;
			case (2):
				num = 2;
                SceneManager.LoadScene("Event");
                break;
            case (3):
				num = 3;
                SceneManager.LoadScene("Event");
                break;
            case (4):
				num = 4;
                SceneManager.LoadScene("Event");
                break;
            case (5):
				num = 5;
                SceneManager.LoadScene("Event");
                break;
            case (6):
				num = 6;
                SceneManager.LoadScene("Event");
                break;
            case (7):
				num = 7;
                SceneManager.LoadScene("Event");
                break;
        }
    }
}
