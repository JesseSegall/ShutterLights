using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int collectibleValue = 1;
    private MeshRenderer meshRenderer;
    private Collider orbCollider;

     private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        orbCollider = GetComponent<Collider>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the score
            ScoreManager.instance.AddScore(collectibleValue);

            // Destroy the collectible after collection
            //Destroy(gameObject);
            meshRenderer.enabled = false;
            orbCollider.enabled = false;
        }
    }
}