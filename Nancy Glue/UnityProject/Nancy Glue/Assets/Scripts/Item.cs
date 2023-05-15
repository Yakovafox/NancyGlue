using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D;
using UnityEngine;

public class Item 
{
    public int id;
    public string title;
    public string description;
    public Sprite icon;

    [SerializeField] private ItemScriptableObject _evidenceItem;
    //simple class to contain item data
    public Item(int id, string title, string description)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.icon = Resources.Load<Sprite>("UI/Sprites/" + title);
    }

    public Item(Item item)
    {
        this.id = item.id;
        this.title = item.title;
        this.description = item.description;
        this.icon = Resources.Load<Sprite>("UI/Sprites/" + item.title);
    }

    public Item(ItemScriptableObject evidenceItem) //overload for Item class, implementing scriptable objects.
    {
        id = evidenceItem.ItemID;
        title = evidenceItem.Title;
        description = evidenceItem.Description;
        icon = evidenceItem.Icon;
    }
}
