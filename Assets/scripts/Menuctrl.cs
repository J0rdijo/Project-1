using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menuctrl : MonoBehaviour {

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
}
