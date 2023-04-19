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
    //public UIInventory inventoryUI;
    
    private void Awake()
    {
        invUI = GameObject.Find("UIGridPanel").GetComponent<invUI>();
        SLGS = GameObject.FindObjectOfType<SaveLoadGameState>();
        gameManager = GameObject.FindObjectOfType<GameManager1>();
    }


    void Start()
    {

        /*
        INV LOADING NEEDS TO GO IN UI
        //read inv ids from file 
        string[] lines = File.ReadAllLines("Inv.txt");
        int[] idsToLoad = Array.ConvertAll(lines, int.Parse);
        //add items to characterInv

        for (int i=0; i < idsToLoad.Length; i++)
        {
            GiveItem(idsToLoad[i]);
            Debug.Log("loaded inventory, added item with id: " + idsToLoad[i]);
        }

        */
        Debug.Log("check for load");
        if (NewOrLoad.isLoad == true)
        {
            Debug.Log("loading");
            SLGS.Load();
            //go through savedIDs
            for (int i = 0; i < savedIDs.Length; i++)
            {
                GiveItem(savedIDs[i]);
                Debug.Log(savedIDs[i]);
            }
        }
        else
            resetInv();
            Debug.Log("new game");


    }

    void Update()
    {
        
    }

    public void GiveItem(int id)
    {
        //check if item is already in inv

        foreach (Item item in characterItems)
        {
            if (item.id == id)
            {
                return;
            }
        }
        //if not
        Item itemToAdd = itemDatabase.GetItem(id);
        characterItems.Add(itemToAdd);
        Debug.Log("added item to characterItems");
        invUI.addItemToUI(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
        Debug.Log("remove item from scene");
        gameManager.UpdateScene(id);


    }

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
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public Item CheckForItem(int id)
    {
        return characterItems.Find(item => item.id == id);
    }

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

    public void resetInv()
    {
        characterItems.Clear();
        invUI.resetInvUi();
        
        
        


    }



    //save inv on quit
    private void OnApplicationQuit()
    {
        //save item id's to list


        savedIDs = characterItems.Select(x => x.id).ToArray();
        //call the save
        SLGS.SaveGame();
        Debug.Log("game saved to " + Application.persistentDataPath);
    }
}
