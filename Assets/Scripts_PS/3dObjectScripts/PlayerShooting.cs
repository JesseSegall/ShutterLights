using UnityEngine;
using TMPro; // If you use TextMeshPro for ammo display

public class PlayerShooting : MonoBehaviour
{
    public GameObject orbPrefab;
    public Transform firePoint;
    public float orbSpeed = 15f;

    [Header("Ammo Settings")]
    public int orbAmmo = 10;  // Starting orb count
    public TextMeshProUGUI ammoText;  // Display ammo on screen (optional)

    void Start()
    {
        UpdateAmmoDisplay();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && orbAmmo > 0)
        {
            ShootOrb();
        }
    }

    void ShootOrb()
    {
        GameObject orb = Instantiate(orbPrefab, firePoint.position, firePoint.rotation);
        Rigidbody orbRb = orb.GetComponent<Rigidbody>();
        orbRb.velocity = firePoint.forward * orbSpeed;

        Destroy(orb, 5f);

        orbAmmo--; // Reduce ammo count by 1
        UpdateAmmoDisplay();
    }

    public void AddAmmo(int amount)
    {
        orbAmmo += amount;
        UpdateAmmoDisplay();
    }

    void UpdateAmmoDisplay()
    {
        if (ammoText != null)
            ammoText.text = $"Orbs: {orbAmmo}";
    }
}