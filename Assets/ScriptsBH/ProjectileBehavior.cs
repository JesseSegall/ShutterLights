using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Terrain"))
    {
        Destroy(gameObject);
    }
    if (collision.gameObject.CompareTag("Player")) 
        {
            Debug.Log("player collision");
            LightDecayStatusBar lightBar = collision.gameObject.GetComponentInChildren<LightDecayStatusBar>();
            LightDecay lightArea = collision.gameObject.GetComponentInChildren<LightDecay>();
            lightBar.GhostContact(2f);
            //lightArea.GhostContactAreaLight(2f);
        }
}
}