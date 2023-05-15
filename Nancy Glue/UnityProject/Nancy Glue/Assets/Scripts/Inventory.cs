using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public List<Item> characterItems = new List<Item>();
    public int[] savedIDs;
    public ItemDB itemDatabase;
    public invUI invUI;
    public SaveLoadGameState SLGS;
    public GameManager1 gameManager;
    
    

    private void Awake()
    {
        invUI = GameObject.Find("UIGridPanel").GetComponent<invUI>();
        SLGS = GameObject.FindObjectOfType<SaveLoadGameState>();
        gameManager = GameObject.FindObjectOfType<GameManager1>();
        
    }


    //load inv from save
    public void LoadInventory()
    {
        for (int i = 0; i < savedIDs.Length; i++)
        {
            LoadInv(savedIDs[i]);
        }
    }
    //remove loaded in items from scene
    private void LoadInv(int id)
    {
        gameManager.UpdateScene(id);
    }
    //add item to inv by id
    public void GiveItem(int id)
    {
        
        Item itemToAdd = itemDatabase.GetItem(id);
        characterItems.Add(itemToAdd);
        
        invUI.addItemToUI(itemToAdd);
        
        
    }

    //add item to inv by name
    public void GiveItem(string itemName)
    {
        //check if item is already in inv
        foreach (Item item in characterItems)
        {
            if (item.title == itemName)
            {
                return;
            }
        }


        Item itemToAdd = itemDatabase.GetItem(itemName);
        characterItems.Add(itemToAdd);
        invUI.addItemToUI(itemToAdd);
        
    }
    // check for item in inv by id
    public Item CheckForItem(int id)
    {
        return characterItems.Find(item => item.id == id);
    }
    //remove item from inv by id
    public void RemoveItem(int id)
    {
        Item itemToRemove = CheckForItem(id);
        if (itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            invUI.removeItemFromUI(id);
            Debug.Log("Removed item: " + itemToRemove.title);
        }
    }
    //wipe inv
    public void resetInv()
    {
        characterItems.Clear();
        invUI.resetInvUi();
        
        
        


    }




    //prepare inv for save
    public void SaveInv()
    {
        savedIDs = characterItems.Select(x => x.id).ToArray();
    }


}
