using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TransititionToBossRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNext());
    }

    IEnumerator LoadNext(){
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("BossRoom");
    }
}
