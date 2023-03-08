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
    //public UIInventory inventoryUI;

    private void Awake()
    {
        invUI = GameObject.Find("UICanvas").GetComponent<invUI>();
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
        
        


    }

    void Update()
    {
        
    }

    public void GiveItem(int id)
    {
        Item itemToAdd = itemDatabase.GetItem(id);
        characterItems.Add(itemToAdd);
        Debug.Log("added item to characterItems");
        invUI.addItemToUI(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public void GiveItem(string itemName)
    {
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
    //save inv on quit
    private void OnApplicationQuit()
    {
        //save item id's to list


        savedIDs = characterItems.Select(x => x.id).ToArray();



        //write the savedIDs list to a text file
        using (TextWriter tw = new StreamWriter("Inv.txt"))
        {
            foreach (int n in savedIDs)
                tw.WriteLine(n);

        }
        Debug.Log("saved item ids:");
        foreach (var x in savedIDs)
        {
            Debug.Log(x.ToString());

        }
    }
}
