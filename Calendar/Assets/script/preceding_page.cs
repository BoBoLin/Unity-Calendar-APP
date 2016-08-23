using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class preceding_page : MonoBehaviour {
    public void TriggerButtonInAddEvent()
    {
        int condition = ShowEvents.condition;
        if (condition == 0)
            SceneManager.LoadScene("Event");
        if (condition == 1)
            SceneManager.LoadScene("EventStep");
    }

    public void TriggerButton1Behavior(int i)
    {
        switch (i)
        {
            default:
                break;
            case (1):
                SceneManager.LoadScene("home");
                break;
            case (2):
                SceneManager.LoadScene("Add_Event");
                break;
			case(3):
				SceneManager.LoadScene ("Event");
				break;
            case (4):
                SceneManager.LoadScene("EventStep");
                break;
			case(5):
				SceneManager.LoadScene ("EditDetail");
				break;
			case(6):
				SceneManager.LoadScene ("StepDetail");
				break;
        }
    }
}
