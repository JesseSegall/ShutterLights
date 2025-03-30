using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FootIKHandler : MonoBehaviour
{
    private Animator animator;

    [Tooltip("Layers that represent the ground (e.g., Default)")]
    public LayerMask groundLayer;

    [Tooltip("Offset to keep the foot just above the ground")]
    public float footOffset = 0.05f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // This function is called by the Animator during the IK pass.
    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            // Adjust both feet
            SetFootIK(AvatarIKGoal.LeftFoot);
            SetFootIK(AvatarIKGoal.RightFoot);
        }
    }

    void SetFootIK(AvatarIKGoal foot)
    {
        // Get the current foot position from the animation
        Vector3 footPos = animator.GetIKPosition(foot);


        Debug.DrawRay(footPos + Vector3.up, Vector3.down * 2f, Color.red);

        // Cast a ray downward from slightly above the current foot position
        Ray ray = new Ray(footPos + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, groundLayer))
        {
            // Calculate the target position so the foot is just above the ground
            Vector3 targetPos = hit.point + Vector3.up * footOffset;
            animator.SetIKPosition(foot, targetPos);
            animator.SetIKPositionWeight(foot, 1);
            animator.SetIKRotationWeight(foot, 1);

            // Adjust the foot rotation to align with the ground normal.
            Quaternion footRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
            animator.SetIKRotation(foot, footRotation);
        }
    }
}