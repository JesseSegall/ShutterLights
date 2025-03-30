using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnRotationTiming : MonoBehaviour
{
    private Animator anim;
    private bool isRotating = true;
    private float startTime = 0f;
    private float repeatTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("ChangeRotationState", startTime, repeatTime);

    }

    void ChangeRotationState()
    {

        Debug.Log("Changing rotation state, isRotating is " + isRotating);
        if (isRotating)
        {
            anim.SetBool("RotationOn", true);
        }
        else
        {
            anim.SetBool("RotationOn", false);
        }
        isRotating = !isRotating;
    }
}
