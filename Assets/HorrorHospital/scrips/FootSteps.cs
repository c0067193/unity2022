using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClips;
    public AudioSource audioSource;

    public CharacterController controller;

    public float footstepThreshhold;
    public float footstepRate;
    private float LastFootstepTime;

    private void Update()
    {
        //if (controller.velocity.magnitude > footstepThreshhold)
        //{

            if (Time.time - LastFootstepTime > footstepRate)
            {

                LastFootstepTime = Time.time;
                audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);


            }
       // }
        else
        {
            audioSource.Pause();
        }
    }
}
