using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int collectibleValue = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the score
            ScoreManager.instance.AddScore(collectibleValue);

            // Destroy the collectible after collection
            Destroy(gameObject);
        }
    }
}