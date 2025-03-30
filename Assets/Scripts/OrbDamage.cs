
using UnityEngine;

public class OrbDamage : MonoBehaviour
{
    public float damageAmount = 3f;
    public AudioClip orbImpactSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by Orb");
            LightDecayStatusBar lightBar = other.GetComponentInChildren<LightDecayStatusBar>();
            if (lightBar != null)
            {
                lightBar.DamageTaken(damageAmount);

            }
            AudioSource.PlayClipAtPoint(orbImpactSound, transform.position);
            Debug.Log("Sound clip should play");


            Destroy(gameObject);
            Debug.Log("Orb should be destroyed ");
        }
        else if (other.CompareTag("Wall"))
        {
            Debug.Log("Wall hit by Orb");
            AudioSource.PlayClipAtPoint(orbImpactSound, transform.position);
            Destroy(gameObject);
        }
    }
}