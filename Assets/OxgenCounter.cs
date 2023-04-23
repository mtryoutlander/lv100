using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class OxgenCounter : MonoBehaviour
{
    public float oxgen = 200;
    public Image[] oxgenBar;
    [HideInInspector] public float oxgenLossRate = 1;
    private bool isPaused;
    private float oxgenInSecondTank;
    private void Update()
    {
        if (oxgen > 0 && !isPaused)
        {
            oxgen -= oxgenLossRate * Time.deltaTime;
            if(oxgen > 100)
            {
                oxgenInSecondTank = oxgen - 100;
                oxgenBar[1].fillAmount= oxgenInSecondTank/100;
            }
            else 
            {
                oxgenBar[1].fillAmount= 0;
            }
            oxgenBar[0].fillAmount= oxgen/100;
        }
        else
        { }
    }
    private void Awake()
    {
        PAUSE_EVENT.Pause += Pause;
        PAUSE_EVENT.Resume += Resume;
    }
    private void OnDestroy()
    {
        PAUSE_EVENT.Pause -= Pause;
        PAUSE_EVENT.Pause -= Resume;
    }
    private void Pause()
    {
        isPaused = true;
    }
    private void Resume()
    {
        isPaused = false;
    }
}
