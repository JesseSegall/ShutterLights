using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlatformAttach : MonoBehaviour {
    [FormerlySerializedAs("Tag That Player Will Attach To")]
    public string[] tagsPlayerWillAttachTo;

    private CharacterController _characterController;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (_characterController == null)
        {
            Debug.LogError("CharacterController not found on this GameObject.");
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the collided object has one of the allowed tags.
        bool isAllowed = false;
        foreach (string tag in tagsPlayerWillAttachTo)
        {
            if (hit.collider.CompareTag(tag))
            {
                isAllowed = true;
                break;
            }
        }

        // Only attach if the object is allowed.
        if (isAllowed)
        {
            if (transform.parent != hit.collider.transform)
            {
                Debug.Log("Attaching to object: " + hit.collider.name);
                transform.SetParent(hit.collider.transform);
            }
        }

    }


    public void DetachIfAttached()
    {
        if (transform.parent != null)
        {
            Debug.Log("Detaching from: " + transform.parent.name);
            transform.SetParent(null);
        }
    }
}
