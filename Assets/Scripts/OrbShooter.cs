using System.Collections;
using UnityEngine;

public class OrbShooter : MonoBehaviour
{
    [Header("Orb Settings")]

    public GameObject orbPrefab;


    public Transform spawnPoint;

    [Tooltip("Impulse speed for the orb.")]
    public float orbSpeed = 10f;

    [Header("Firing Interval")]

    public float minShootInterval = 1f;

    public AudioClip orbLaunchSound;

    public float maxShootInterval = 3f;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Start a repeating coroutine to shoot at random intervals
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            // Pick a random time to wait before shooting
            float waitTime = Random.Range(minShootInterval, maxShootInterval);
            yield return new WaitForSeconds(waitTime);

            // Shoot an orb
            ShootOrb();
        }
    }

    private void ShootOrb()
    {

        if (orbPrefab == null)
        {
            Debug.LogWarning("No orb prefab assigned to OrbShooter!");
            return;
        }
        if (spawnPoint == null)
        {
            Debug.LogWarning("No spawnPoint assigned to OrbShooter!");
            return;
        }

        // Spawn the orb at the spawnPoint's position
        GameObject orb = Instantiate(orbPrefab, spawnPoint.position, spawnPoint.rotation);


        Rigidbody orbRb = orb.GetComponent<Rigidbody>();
        if (orbRb != null)
        {
            // Add an impulse force in the direction of spawnPoint.forward
            orbRb.AddForce(spawnPoint.forward * orbSpeed, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("The orb prefab does not have a Rigidbody!");
        }
        if (orbLaunchSound != null)
        {
            _audioSource.PlayOneShot(orbLaunchSound);
        }


    }
}