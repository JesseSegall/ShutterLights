using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{

    void Awake(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.X)) {
            LoadMainScene();
        }
    }

     public void LoadMainScene()
    {
        Debug.Log("button clicked");
        Application.Quit();
        //PlayerManager.Instance.SetSpawnPointID("SpawnPoint_MenuRespawn");
        //SceneManager.LoadScene("Room_1");
    }
}
