using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class invUI : MonoBehaviour
{
    public GameObject testPrefab;
    public ItemDB ItemDB;
    public GameObject grid;
    public static Dictionary<int, ItemManager> itemsDict = new Dictionary<int, ItemManager>();
    // public SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
       // spriteRenderer = testPrefab.GetComponent<SpriteRenderer>();
       
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
        Debug.Log("itemOBJ: " + itemOBJ);
        
        itemOBJ.GetComponent<ItemManager>().id = itemToAdd.id;
        Debug.Log("itemOBJ id: " + itemOBJ.GetComponent<ItemManager>().id);
        itemOBJ.GetComponent<ItemManager>().title = itemToAdd.title;
        itemOBJ.GetComponent<ItemManager>().description = itemToAdd.description;
        //icon can be set here directly
        
        
        itemOBJ.GetComponent<ItemManager>().icon = itemToAdd.icon;

        itemOBJ.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = itemToAdd.icon;
        itemOBJ.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = itemToAdd.title;
        itemOBJ.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = itemToAdd.description;
        //testrun basic prefab 
        //add nested prefab to canvas
        //save to dict
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
