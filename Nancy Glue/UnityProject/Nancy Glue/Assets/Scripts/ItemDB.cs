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

    void Start()
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
        items = new List<Item>();
        var newItems2 = FindObjectsOfType<ItemData>();
        foreach (var data in newItems2)
        {
            items.Add(new Item(data.EvidenceItem));
        }
        
    }
}
