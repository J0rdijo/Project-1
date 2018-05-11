using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menuctrl : MonoBehaviour {

    public Text widescreen_txt;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
	
    public void ExitGameBtn()
    {
        Application.Quit();
    }

    public void soundOnOff()
    {
        bool sound = true;
        if (!sound)
        {
            AudioListener.pause = true;
            sound = true;
        }
        else
        {
            AudioListener.pause = false;
            sound = false;
        }
    }

    public void fullScreenOff()
    {
        if (Screen.fullScreen == true)
        {
            Screen.fullScreen = false;
            widescreen_txt.text = "                                          WIDESCREEN";
        }
        else
        {
            Screen.fullScreen = true;
            widescreen_txt.text = "                                          FULLSCREEN";
        }
    }

}
