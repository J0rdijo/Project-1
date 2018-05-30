using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class BackgroundMusic : MonoBehaviour
{
    public AudioSource background;
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    private Scene actualScene;
    private Scene lastScene;

    // Use this for initialization
    void Start()
    {
        if (Time.realtimeSinceStartup > 3)
            DestroyObject(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        actualScene = SceneManager.GetActiveScene();
        if (actualScene.name != lastScene.name)// && actualScene.name != "Menu 1"
        {
            if (GameObject.Find("Robot") && gameplayMusic != background.clip)
            {
                changeMusic(gameplayMusic);
            }
            else if(GameObject.Find("MENUCTRL") && menuMusic != background.clip)
            {
                changeMusic(menuMusic);
            }
            lastScene = SceneManager.GetActiveScene();
        }
    }
    public void changeMusic(AudioClip music)
    {
        background.Stop();
        background.clip = music;
        background.Play();
    }
}
