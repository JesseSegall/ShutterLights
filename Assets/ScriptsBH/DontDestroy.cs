using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy instance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
