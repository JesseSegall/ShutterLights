using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionRoom1ToMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNext());
    }

    IEnumerator LoadNext(){
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainScene");
    }
}
