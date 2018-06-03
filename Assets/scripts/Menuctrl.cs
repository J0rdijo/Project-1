using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menuctrl : MonoBehaviour {

    public Text widescreen_txt;

    void Start()
    {
        if (GameObject.Find("Skin Portada") != null)
        {
            switch (PlayerPrefs.GetInt("Player Skin"))
            {
                case 1:
                    GameObject.Find("Skin Portada").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/pj_5u_first");
                    break;
                case 2:
                    GameObject.Find("Skin Portada").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/skin1");
                    break;
                case 3:
                    GameObject.Find("Skin Portada").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/skin3-g");
                    break;
                case 4:
                    GameObject.Find("Skin Portada").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/skin4");
                    break;
                case 5:
                    GameObject.Find("Skin Portada").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>("Sprites/skin-E");
                    break;

            }
        }

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReciveSkin(int value)
    {
        if(PlayerPrefs.GetInt("Levels Unlocked") > 25)
            PlayerPrefs.SetInt("Player Skin", value);
    }

    public void NewGame()
    {

        PlayerPrefs.SetInt("Levels Unlocked", -1);
        PlayerPrefs.SetInt("Player Skin", 1);
        SceneManager.LoadScene("Intro");
    }

    public void CheckLevel(int levelValue)
    {
        if (PlayerPrefs.GetInt("Levels Unlocked") >= levelValue)
            SceneManager.LoadScene(12 + levelValue);
    }

    public void ExitGameBtn()
    {
        Application.Quit();
    }

    public void fullScreenOff()
    {
        if (Screen.fullScreen == true)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Screen.fullScreen = true;
        }
    }

    public void Mute()
    {
        AudioListener.pause = !AudioListener.pause;
    }

}
