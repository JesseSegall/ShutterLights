using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 1; // Ammo added when collected

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            if(playerShooting != null)
            {
                playerShooting.AddAmmo(ammoAmount);
                //Destroy(gameObject);
            }
        }
    }
}