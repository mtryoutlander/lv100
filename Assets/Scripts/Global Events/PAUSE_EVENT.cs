using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PAUSE_EVENT {
    public static event Action Pause;
    public static event Action Resume;
    public static void OnPause()
{
        Pause?.Invoke();
    }
    public static void OnResume()
{
        Resume?.Invoke();
    }
}
