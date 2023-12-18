using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventory, item;
    
    public List<InteractableObj> inventoryList = new List<InteractableObj>();
    private void Start()
    {
        inventory.SetActive(false);
    }
    private void Update()
    {
        
    }
    public void addList(string _name, string _photoPath)
    {
        inventoryList.Add(new InteractableObj(_name, _photoPath));
    }
    public void inventoryRelease()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(true);

        }
    }
}
