using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToRoom1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNext());
        ScoreManager.instance.ResetScore();
        ElapsedTime.instance.ResetTime();
    }

    IEnumerator LoadNext(){
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Room_1");
    }
}
