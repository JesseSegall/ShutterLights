using UnityEngine;

public class ZombieCollision : MonoBehaviour
{
    void Start()
    {
        // Get this zombie's collider.
        Collider myCollider = GetComponent<Collider>();
        if (myCollider == null)
        {
            Debug.LogWarning("No collider found on " + gameObject.name);
            return;
        }

        // Find all other zombies in the scene by their tag.
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (GameObject zombie in zombies)
        {
            // Skip itself.
            if (zombie == gameObject)
                continue;

            Collider otherCollider = zombie.GetComponent<Collider>();
            if (otherCollider != null)
            {
                // Ignore collisions between this zombie and the other zombie.
                Physics.IgnoreCollision(myCollider, otherCollider);
            }
        }
    }
}