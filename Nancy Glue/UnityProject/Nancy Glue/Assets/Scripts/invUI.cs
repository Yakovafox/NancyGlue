using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

//this script keeps the player inventory UI up top to date with the player inventory

public class invUI : MonoBehaviour
{
    public GameObject testPrefab;
    public ItemDB ItemDB;
    public GameObject grid;
    public static Dictionary<int, ItemManager> itemsDict = new Dictionary<int, ItemManager>();
    public SaveLoadGameState SLGS;
    
    private void Awake()
    {
        SLGS= GameObject.FindObjectOfType<SaveLoadGameState>();
    }



    //check if loaded inv is empty
    public void LoadInvUI()
    {
        if (!IsNullOrEmpty(SLGS.inv.savedIDs))
        {
            // set the empty inv text inactive 
            var invEmptyText = GameObject.Find("BlankTextInventory");
            if (invEmptyText != null && invEmptyText.activeSelf)
                invEmptyText.SetActive(false);
        }
    }

    public static bool IsNullOrEmpty(  Array array)
    {
        return (array == null || array.Length == 0);
    }

 
    //add item to inventory UI by ID
    public void addItemToUI(Item itemToAdd)
    {
        
        

        

        GameObject itemOBJ = Instantiate(testPrefab, grid.transform);
        
        
        itemOBJ.GetComponent<ItemManager>().id = itemToAdd.id;
        
        itemOBJ.GetComponent<ItemManager>().title = itemToAdd.title;
        itemOBJ.GetComponent<ItemManager>().description = itemToAdd.description;
        
        
        
        itemOBJ.GetComponent<ItemManager>().icon = itemToAdd.icon;

        itemOBJ.transform.GetChild(0).GetComponent<Image>().sprite = itemToAdd.icon;
        itemOBJ.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = itemToAdd.title;
        itemOBJ.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = itemToAdd.description;
        
        //save to dict if not already there
        if (itemsDict.ContainsKey(itemToAdd.id)) return;
        itemsDict.Add(itemOBJ.GetComponent<ItemManager>().id, itemOBJ.GetComponent<ItemManager>());
    }


    //remove item from inventory UI by ID

    public void removeItemFromUI(int targetId)
    {
        ItemManager target;
        itemsDict.TryGetValue(targetId, out target);

        Destroy(target.GameObject());
        itemsDict.Remove(targetId);

    }

    //wipe inventory UI

    public void resetInvUi()
    {
        itemsDict.Clear();
    }
}
