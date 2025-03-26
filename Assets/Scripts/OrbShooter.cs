using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbShooter : MonoBehaviour
{   // TODO: find sound for the orb being shot
   // public AudioClip orbShot;
    public GameObject orbPrefab;
    public Transform orbSpawn;

    public float orbSpeed = 7f;

    public Vector3 orbDirection = Vector3.forward;
    public float minShootInterval = 1f;
    public float maxShootInterval = 3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootRoutine());
    }
    IEnumerator ShootRoutine()
    {
        while (true)
        {
            // Wait for a random interval
            float interval = Random.Range(minShootInterval, maxShootInterval);
            // Yield tells unity to pause the func and it resumes after interval amount of seconds
            yield return new WaitForSeconds(interval);


            ShootOrb();
        }
    }
    // Update is called once per frame
    void ShootOrb()
    {
        // Point the orb will spawn
        Vector3 spawnPosition = orbSpawn != null ? orbSpawn.position : transform.position;

        // Generate the orb
        GameObject orb = Instantiate(orbPrefab, spawnPosition, Quaternion.identity);

        // Get the orb's rigidbody
        Rigidbody orbRigidbody = orb.GetComponentInChildren<Rigidbody>();

        if (orbRigidbody != null)
        {
            // Convert local direction to world space
            Vector3 worldDirection = transform.TransformDirection(orbDirection.normalized);

            // Apply force to shoot the orb
            orbRigidbody.AddForce(worldDirection * orbSpeed, ForceMode.Impulse);

            //  will prob change this so that it gets destroyed when it hits a wall instead.
            Destroy(orb, 4f);
        }
    }

}
