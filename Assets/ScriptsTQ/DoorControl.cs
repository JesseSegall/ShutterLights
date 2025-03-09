using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject doorCollider;
    private Animator anim;
    private DoorStarter doorStarter;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        doorStarter = doorCollider.GetComponent<DoorStarter>();
        if (doorStarter == null)
        {
            Debug.Log("Door Starter is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doorStarter.DoorsTriggered)
        {
            anim.SetBool("OpenDoor", true);
        }
    }
}
