using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class presspace : MonoBehaviour {

        void Update()
        {
                if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene("Level O");
        }
    }

