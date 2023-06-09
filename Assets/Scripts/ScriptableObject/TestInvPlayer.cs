using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInvPlayer : MonoBehaviour
{
    public InventoryObject inventory;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<Item>();
        if (other.gameObject.tag == "Item")
        {
            
            inventory.AddItem(item.item,1);
            Destroy(other.gameObject);

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
