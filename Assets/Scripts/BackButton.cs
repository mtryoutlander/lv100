using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    //make an add listener function for button clcik
    private Button button;
    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PAUSE_EVENT.OnResume);
    }
}
