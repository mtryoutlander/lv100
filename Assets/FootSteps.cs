using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioSource footSteps;
    public AudioClip[] footStepSounds;
    public GameObject charitor;
    private void Update()
    {
        if (charitor.GetComponent<Animator>().GetFloat("Speed") > 0.1f)
        {
            if (!footSteps.isPlaying)
            {
                footSteps.clip = footStepSounds[Random.Range(0, footStepSounds.Length)];
                footSteps.Play();
            }
        }
        else
        {
            footSteps.Stop();
        }
    }
}
