using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invUI : MonoBehaviour
{
    public GameObject testPrefab;
    public ItemDB ItemDB;
    public GameObject grid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addItemToUI(Item itemToAdd)
    {
        //check if item is already in inventory UI



        
        //if not, add item to inventory

        

        GameObject itemOBJ = Instantiate(testPrefab, transform.position, Quaternion.identity, grid.transform);
        Debug.Log("itemOBJ: " + itemOBJ);
        
        itemOBJ.GetComponent<ItemManager>().id = itemToAdd.id;
        Debug.Log("itemOBJ id: " + itemOBJ.GetComponent<ItemManager>().id);
        itemOBJ.GetComponent<ItemManager>().title = itemToAdd.title;
        itemOBJ.GetComponent<ItemManager>().description = itemToAdd.description;
        itemOBJ.GetComponent<ItemManager>().icon = itemToAdd.icon;
        
        //testrun basic prefab 
        //add nested prefab to canvas

    }
    /*
    public void removeItemFromUI(int itemId)
    {
        Destroy(
        
    }*/
}
