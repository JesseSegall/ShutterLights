using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class TransitionToMain : MonoBehaviour
{
    public AudioSource audioSource;
    private bool hasTriggered = false;

     void Start()
    {
        StartCoroutine(WaitForAudio());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.X)){
            Skip();
        }
    }

    IEnumerator WaitForAudio()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying); 
            SceneManager.LoadScene("MainScene");
    }


    public void Skip(){
        SceneManager.LoadScene("MainScene");
    }
}
