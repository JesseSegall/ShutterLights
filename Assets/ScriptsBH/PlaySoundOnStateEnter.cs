using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStateEnter : StateMachineBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioSource == null)
        {
            audioSource = animator.GetComponent<AudioSource>();
        }

        if (audioSource && soundClip)
        {
            audioSource.PlayOneShot(soundClip);
        }
    }
}
