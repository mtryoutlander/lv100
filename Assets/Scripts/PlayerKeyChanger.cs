using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyChanger : MonoBehaviour
{
    private bool changeKey = false;
    private string keyName = "";
    private GameObject button;
    KeyCode[] mouseKeys;
    void Awake()
    {
        mouseKeys = new KeyCode[] { KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.Mouse2, KeyCode.Mouse3, KeyCode.Mouse4, KeyCode.Mouse5, KeyCode.Mouse6 };
    }
    //use unity new input system to change the keybindings  
    private void ChangeKey(string keyName, KeyCode newKey, GameObject button)
    {
        //set the new key
        PlayerPrefs.SetInt(keyName, (int)newKey);
        PlayerPrefs.Save();
        //find a button object in the scean and change the textmesh pro text  to the new key
        if(newKey == KeyCode.Mouse0)
        {
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Left Mouse";
        }
        else if (newKey == KeyCode.Mouse1)
        {
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Right Mouse";
        }
        else
        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = newKey.ToString();

    }
    public void StartChangeKey(string keyName)
    {
        changeKey = true;
        this.keyName = keyName;
    }
    public void SetButton(GameObject button)
    {
        this.button = button;
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && changeKey )
        {
            ChangeKey(keyName, e.keyCode, button);
            changeKey= false;
            keyName = "";
            button= null;
        }
        if (e.isMouse && changeKey)
        {
            ChangeKey(keyName, (KeyCode)(e.button+ (int)KeyCode.Mouse0), button);
            changeKey = false;
            keyName = "";
            button = null;
        }
       
    }
}
