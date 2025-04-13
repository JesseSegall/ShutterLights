using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class TransitionToMain : MonoBehaviour
{

    void Update() {
        if (Input.GetKeyDown(KeyCode.X)){
            Skip();
        }
    }
    public void Skip(){
        SceneManager.LoadScene("MainScene");
    }
}
