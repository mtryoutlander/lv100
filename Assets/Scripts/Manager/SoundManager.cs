using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{

    /* This is the bare bones of a sound manager therefor needs some intergration beofre it can be functional
     * The code is based on the simple sound manager from code monkeys
     * https://www.youtube.com/watch?v=QL29aTa7J5Q
     * 
     * 
    //enum to store sounds
    public enum Sound
    {

    }

    private static Dictionary<Sound, float> soundTimerDictionary;
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    //initializer
    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
    }

    //3d sound
    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(Sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(Sound))
        {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();

            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static bool CanPlaySound(Sound)
    {
        switch (sound)
        {

        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound = sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound" + sound + "not found!");
        return null;
    }

    public static void AddButtonSounds(this Button_UI buttonUI)
    {

    }
    */
}

