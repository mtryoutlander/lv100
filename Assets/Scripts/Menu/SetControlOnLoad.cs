using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetControlOnLoad : MonoBehaviour
{
    public GameObject inputValue;
    private void Start()
    {
        this.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ((KeyCode)PlayerPrefs.GetInt(inputValue.GetComponentInChildren<TMPro.TextMeshProUGUI>().text)).ToString();
    }
}
