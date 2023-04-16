using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeOnLoad : MonoBehaviour
{
    //set slider base off of player prefs
    private void Awake()
    {
        this.GetComponent<Slider>().value = PlayerPrefs.GetFloat("masterVolume");
    }
}
