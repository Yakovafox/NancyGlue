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
            new Item(0, "item_chalkline", "Chalkline desc"),
            new Item(1,"item_crown", "It's a crown!"),
            new Item(2,"item_reel", "It's a reel!"),
            new Item(3,"item_slime_2", "It's slime!"),
            new Item(4,"item_stuffing", "It's stuffing!")
            
        };
    }
}
