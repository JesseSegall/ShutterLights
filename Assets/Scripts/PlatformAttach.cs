using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttach : MonoBehaviour
{
    
    public string[] tagsPlayerWillAttachTo;

    private CharacterController _characterController;
    private Transform _currentPlatform;

    // How far the ray will shoot from feet
    public float raycastDistance = 1.2f;
    // Little extra offset so its slightly above feet
    public float raycastOffset = 0.2f;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (_characterController == null)
        {
            Debug.LogError("CharacterController not found on this GameObject.");
        }
    }

    void Update()
    {
        

        RaycastHit hit;
        // Set the raycast origin slightly above the player's position.
        Vector3 rayOrigin = transform.position + Vector3.up * raycastOffset;

        // Cast a ray downward to detect the platform beneath the player.
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance))
        {
            bool isAllowed = false;
            // Check if the hit collider has one of the allowed tags.
            foreach (string tag in tagsPlayerWillAttachTo)
            {
                if (hit.collider.CompareTag(tag))
                {
                    isAllowed = true;
                    break;
                }
            }

            if (isAllowed)
            {
                // Attach if not already attached to this platform.
                if (transform.parent != hit.collider.transform)
                {
                    Debug.Log("Attaching to object: " + hit.collider.name);
                    // keep our same world pos so its less janky movement
                    transform.SetParent(hit.collider.transform, true);
                    _currentPlatform = hit.collider.transform;
                }
            }
            else
            {
                // Detach if the object hit is not a valid platform.
                DetachIfAttached();
            }
        }
        else
        {
            // If nothing is hit, detach from any platform anyway.
            DetachIfAttached();
        }
    }

    public void DetachIfAttached()
    {
        if (transform.parent != null)
        {
            Debug.Log("Detaching from: " + transform.parent.name);
            transform.SetParent(null, true);
            _currentPlatform = null;
        }
    }
    void OnDrawGizmos()
    {
        // Just to see the ray
        Vector3 rayOrigin = transform.position + Vector3.up * raycastOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * raycastDistance);
    }
}
