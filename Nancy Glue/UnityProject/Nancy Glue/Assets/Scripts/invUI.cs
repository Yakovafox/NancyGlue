using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class invUI : MonoBehaviour
{
    public GameObject testPrefab;
    public ItemDB ItemDB;
    public GameObject grid;
    public static Dictionary<int, ItemManager> itemsDict = new Dictionary<int, ItemManager>();
    public SaveLoadGameState SLGS;
    // public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        SLGS= GameObject.FindObjectOfType<SaveLoadGameState>();
    }


    // Start is called before the first frame update
    void Start()
    {
       // spriteRenderer = testPrefab.GetComponent<SpriteRenderer>();
       if (NewOrLoad.isLoad)
        {
            //check if there are items in the saved inv 
            SLGS.Load();
            if (IsNullOrEmpty(SLGS.inv.savedIDs))
            {
                //if empty then we're good
                Debug.Log("loaded inv is empty");
            }
            else
            {
                // set the empty inv text inactive
                Debug.Log("loaded inv is not empty");
                var invEmptyText = GameObject.Find("BlankTextInventory");
                if (invEmptyText != null && invEmptyText.activeSelf)
                    invEmptyText.SetActive(false);
            }

        }
    }

    public static bool IsNullOrEmpty(  Array array)
    {
        return (array == null || array.Length == 0);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void addItemToUI(Item itemToAdd)
    {
        
        //check if item is already in inventory UI



        
        //if not, add item to inventory

        

        GameObject itemOBJ = Instantiate(testPrefab, grid.transform);
        //Debug.Log("itemOBJ: " + itemOBJ);
        
        itemOBJ.GetComponent<ItemManager>().id = itemToAdd.id;
        //Debug.Log("itemOBJ id: " + itemOBJ.GetComponent<ItemManager>().id);
        itemOBJ.GetComponent<ItemManager>().title = itemToAdd.title;
        itemOBJ.GetComponent<ItemManager>().description = itemToAdd.description;
        //icon can be set here directly
        
        
        itemOBJ.GetComponent<ItemManager>().icon = itemToAdd.icon;

        itemOBJ.transform.GetChild(0).GetComponent<Image>().sprite = itemToAdd.icon;
        itemOBJ.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = itemToAdd.title;
        itemOBJ.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = itemToAdd.description;
        //testrun basic prefab 
        //add nested prefab to canvas
        //save to dict
        if (itemsDict.ContainsKey(itemToAdd.id)) return;
        itemsDict.Add(itemOBJ.GetComponent<ItemManager>().id, itemOBJ.GetComponent<ItemManager>());
    }

    public void removeItemFromUI(int targetId)
    {
        ItemManager target;
        itemsDict.TryGetValue(targetId, out target);

        Destroy(target.GameObject());
        itemsDict.Remove(targetId);

    }
    public void resetInvUi()
    {
        itemsDict.Clear();
    }
}
