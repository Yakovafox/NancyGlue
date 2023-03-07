using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    void Awake()
    {
        BuildDB();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string itemName)
    {
        return items.Find(item => item.title == itemName);
    }

    


    
    void BuildDB()
    {
        items = new List<Item>() {
            new Item(0, "Test_Item", "TEST DESCRIPTION"),
            new Item(1,"Test_Item_2", "TEST DESCRIPTION 2")
            
        };
    }
}
