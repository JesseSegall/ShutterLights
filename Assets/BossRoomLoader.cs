using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoomLoader : MonoBehaviour
{
    public Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.Log("Renderer could not be found");
        }
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Boss Room OnTrigger called");
        if (other.gameObject.CompareTag("Player") || (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            SceneManager.LoadScene("BossRoom");
        }
    }

}
