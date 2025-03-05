using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLoader : MonoBehaviour
{
    public Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        if(rend == null) {
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
        Debug.Log("OnTrigger called");
        if(other.attachedRigidbody != null){
            Player player = other.attachedRigidbody.gameObject.GetComponent<Player>();
            if(player != null) {
                SceneManager.LoadScene("Room_1");
            }
        }
    }
}
