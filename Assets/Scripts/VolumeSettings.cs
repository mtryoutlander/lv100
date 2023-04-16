using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();
        UpdateActiveAudioSource();
    }

    private void UpdateActiveAudioSource()
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in sources)
        {
            source.volume = PlayerPrefs.GetFloat("masterVolume");
        }
    }
}
