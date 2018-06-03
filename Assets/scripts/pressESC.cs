using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pressESC : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Menu_1");
        else if (Input.GetKeyDown(KeyCode.Joystick1Button0))
            SceneManager.LoadScene("Menu_1");
        else if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("Menu_1");
    }
}

